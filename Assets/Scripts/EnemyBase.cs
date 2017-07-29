using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    public int maxHealth;
    private int _currentHealth;
    public int currentHealth { get { return _currentHealth; } }
    public float radius = 1.0f;

    // Don't kill dummies
    public bool isDummy;
    private float damageCoodown;

    public void TakeDamage (int damage) {
        _currentHealth -= damage;

        if (isDummy) {
            damageCoodown = 3.0f;
        }
    }

    public void Heal (int damage) {
        _currentHealth = Mathf.Min(_currentHealth + damage, maxHealth);
    }

	// Use this for initialization
	void Start () {
        _currentHealth = maxHealth;
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

        if (currentHealth < 0) {
            GameObject.Destroy(gameObject);
        }
	}
}
