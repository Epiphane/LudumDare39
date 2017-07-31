using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.PyroParticles;

public class Flamestrike : Ability {

    public GameObject indicatorPrefab;
    public GameObject flamestrikePrefab;

    private UnitMovement myMovement;

    void Start() {
        myMovement = GetComponent<UnitMovement>();
    }

    private Vector3 castPosition;

    public override void DoCast(Vector3 mouse) {
        // Animate the cast
        myMovement.animator.SetTrigger("brandAttack");

        Vector3 direction = mouse - transform.position;
        direction.y = 0;

        if (direction.magnitude > 9.75f) {
            direction.Normalize();
            direction.Scale(9.75f * Vector3.one);
        }

        castPosition = transform.position + direction;
        GetComponent<DirectionSmoother>().IWantToFace(direction);
    }

    public override void Execute() {
        GameObject flamestrike = GameObject.Instantiate(flamestrikePrefab);
        flamestrike.transform.position = castPosition;

        GetComponent<UnitSpells>().DoneCasting();
    }

    void Collide(FireProjectileScript script, Vector3 pos, GameObject other) {
        UnitWithHealth enemy = other.GetComponent<UnitWithHealth>();
        if (enemy != null) {
            enemy.TakeDamage(50);
        }
    }
}
