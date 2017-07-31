using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitWithHealth : MonoBehaviour {

    public int maxHealth;
    private int _currentHealth;
    public int currentHealth { get { return _currentHealth; } }
    public bool isDead { get { return _currentHealth <= 0; } }
    public float radius = 1.0f;

    // Stats
    // b_ prefix means base value. Don't change this unless you're fundamentally changing the unit
    public int b_moveSpeed = 100; // Normal walking speed == 100
    public int b_strength = 0; // Physical damage
    public int b_intelligence = 0; // Magic damage
    public int b_armor = 0; // Physical resistance
    public int b_magicResist = 0; // I wonder what this could be

    // This is where the magic happens
    public float moveSpeed { get { return b_moveSpeed; } }
    public float strength { get { return b_strength; } }
    public float intelligence { get { return b_intelligence; } }
    public float armor { get { return b_armor; } }
    public float magicResist { get { return b_magicResist; } }

    // Damage :(
    public Transform damageTextRoot;
    public GameObject damageText;

    // Animator
    [HideInInspector]
    public Animator animator = null; // Redundant with UnitMovement

    public void TakeDamage (int damage) {
        if (_currentHealth < 0) {
            _currentHealth = 0;
            return;
        }

        _currentHealth -= damage;

        GameObject dmg = GameObject.Instantiate(damageText, damageTextRoot.position, damageTextRoot.rotation);
        TextMeshPro tmp = dmg.GetComponent<TextMeshPro>();

        tmp.color = Color.red;
        tmp.text = damage + "";
    }

    public int ComputeDamage(UnitWithHealth other, Ability.Stats stats) {
        float physical = stats.basePhysical + stats.scalingPhysical * strength;
        float magic    = stats.baseMagic    + stats.scalingMagic    * intelligence;

        float armor = other.armor;
        float magicResist = other.magicResist;

        Debug.Log(physical);
        Debug.Log(armor);

        // lol http://leagueoflegends.wikia.com/wiki/Armor
        physical *= 100 / (100 + armor);
        magic *= 100 / (100 + magicResist);

        return (int) (physical + magic);
    }

    public void DealDamage(UnitWithHealth other, Ability.Stats stats) {
        other.TakeDamage(ComputeDamage(other, stats));
    }

    public void Heal (int damage) {
        if (isDead) {
            return;
        }

        _currentHealth = Mathf.Min(_currentHealth + damage, maxHealth);

        GameObject dmg = GameObject.Instantiate(damageText, damageTextRoot.position, damageTextRoot.rotation);
        TextMeshPro tmp = dmg.GetComponent<TextMeshPro>();

        tmp.color = Color.green;
        tmp.text = damage + "";
    }

	// Use this for initialization
	void Start () {
        _currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Die() {
        animator.SetBool("dead", true);
        //GetComponent<Rigidbody>().useGravity = false;
        //GetComponent<CharacterController>().enabled = false;
        GetComponent<CharacterController>().height = 0;
        GetComponent<CharacterController>().radius = 0.2f;
    }

    // Update is called once per frame
    protected void Update() {
        if (isDead) {
            Die();
        }
    }
}
