using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.PyroParticles;

public class Fireball : Ability {

    public GameObject fireballPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private Vector3 castDirection;

    public override void Cast(Vector3 mouse) {
        // Animate the fireball
        Animator anim = GetComponent<Animator>();
        if (anim != null) {
            anim.SetTrigger("fireballThrow");
        }

        castDirection = mouse - transform.position;
        castDirection.y = 0;

        transform.rotation = Quaternion.LookRotation(castDirection, Vector3.up);
    }

    void FireballComplete() {
        GameObject fireball = GameObject.Instantiate(fireballPrefab);
        fireball.transform.position = transform.position + Vector3.up + castDirection.normalized;

        FireProjectileScript projectile = fireball.GetComponent<FireProjectileScript>();
        fireball.transform.rotation = Quaternion.LookRotation(castDirection, Vector3.up);
        projectile.CollisionDelegate = Collide;

        GetComponent<PlayerSpells>().DoneCasting();
    }

    void Collide(FireProjectileScript script, Vector3 pos, GameObject other) {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null) {
            enemy.TakeDamage(50);
        }
    }
}
