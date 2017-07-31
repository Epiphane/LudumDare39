using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.PyroParticles;

public class Flamestrike : Ability {

    public GameObject indicatorPrefab;
    public GameObject flamestrikePrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private Vector3 castPosition;

    public override void DoCast(Vector3 mouse) {
        // Animate the cast
        Animator anim = GetComponent<Animator>();
        if (anim != null) {
            anim.SetTrigger("brandAttack");
        }

        Vector3 direction = mouse - transform.position;
        direction.y = 0;

        if (direction.magnitude > 9.75f) {
            direction.Normalize();
            direction.Scale(9.75f * Vector3.one);
        }

        castPosition = transform.position + direction;
        GetComponent<DirectionSmoother>().IWantToFace(direction);
    }

    void FlamestrikeComplete() {
        GameObject flamestrike = GameObject.Instantiate(flamestrikePrefab);
        flamestrike.transform.position = castPosition;

        GetComponent<PlayerSpells>().DoneCasting();
    }

    void Collide(FireProjectileScript script, Vector3 pos, GameObject other) {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null) {
            enemy.TakeDamage(50);
        }
    }
}
