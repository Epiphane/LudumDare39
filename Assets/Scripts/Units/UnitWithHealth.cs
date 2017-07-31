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

    // Damage :(
    public Transform damageTextRoot;
    public GameObject damageText;

    // Animator
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
