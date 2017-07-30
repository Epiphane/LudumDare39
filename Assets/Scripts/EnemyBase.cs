using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour {

    public int maxHealth;
    private int _currentHealth;
    public int currentHealth { get { return _currentHealth; } }
    public bool isDead { get { return !isDummy && _currentHealth < 0; } }
    public float radius = 1.0f;

    // Let the bodies hit the floor
    private float timeSpentDead = 0;
    public float timeUntilExpire = 10.0f;

    // Don't kill dummies
    public bool isDummy;
    private float damageCoodown;

    // Damage :(
    public Transform damageTextRoot;
    public GameObject damageText;

    public void TakeDamage (int damage) {
        _currentHealth -= damage;

        if (isDummy) {
            damageCoodown = 3.0f;
        }

        if (_currentHealth < 0) {
            _currentHealth = 0;
        }

        GameObject dmg = GameObject.Instantiate(damageText, damageTextRoot.position, damageTextRoot.rotation);
        TextMeshPro tmp = dmg.GetComponent<TextMeshPro>();

        tmp.text = damage + "";
    }

    public void Heal (int damage) {
        _currentHealth = Mathf.Min(_currentHealth + damage, maxHealth);
    }

	// Use this for initialization
	void Start () {
        _currentHealth = maxHealth;
    }

    void Die() {
        GetComponent<Animator>().SetBool("dead", true);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CharacterController>().enabled = false;
    }

	// Update is called once per frame
	void Update () {
		if (isDummy) {
            if (damageCoodown > 0) {
                damageCoodown -= Time.deltaTime;
            }
            else {
                if (currentHealth < maxHealth) {
                    Heal(20);
                    damageCoodown = 0.2f;
                }
            }

            return;
        }

        if (isDead) {
            Die();

            timeSpentDead += Time.deltaTime;

            if (timeSpentDead >= timeUntilExpire) {
                GameObject.Destroy(gameObject);
            }
        }
	}
}
