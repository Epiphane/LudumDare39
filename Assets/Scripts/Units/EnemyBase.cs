using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : UnitWithHealth {

    new public bool isDead { get { return !isDummy && currentHealth <= 0; } }

    // Let the bodies hit the floor
    private float timeSpentDead = 0;
    public float timeUntilExpire = 10.0f;

    // Don't kill dummies
    public bool isDummy;
    private float damageCoodown;

    new public void TakeDamage (int damage) {
        if (isDummy) {
            damageCoodown = 3.0f;
        }

        base.TakeDamage(damage);
    }

	// Update is called once per frame
	new protected void Update () {
        base.Update();

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
            timeSpentDead += Time.deltaTime;

            if (timeSpentDead >= timeUntilExpire) {
                GameObject.Destroy(gameObject);
            }
        }
	}
}
