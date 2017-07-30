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

    public void TakeDamage (int damage) {
        _currentHealth -= damage;

        if (_currentHealth < 0) {
            _currentHealth = 0;
        }

        GameObject dmg = GameObject.Instantiate(damageText, damageTextRoot.position, damageTextRoot.rotation);
        TextMeshPro tmp = dmg.GetComponent<TextMeshPro>();

        tmp.color = Color.red;
        tmp.text = damage + "";
    }

    public void Heal (int damage) {
        _currentHealth = Mathf.Min(_currentHealth + damage, maxHealth);

        GameObject dmg = GameObject.Instantiate(damageText, damageTextRoot.position, damageTextRoot.rotation);
        TextMeshPro tmp = dmg.GetComponent<TextMeshPro>();

        tmp.color = Color.green;
        tmp.text = damage + "";
    }

	// Use this for initialization
	void Start () {
        _currentHealth = maxHealth;
    }
}
