using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.PyroParticles;

public class Volley : Ability {

    public GameObject indicatorPrefab;
    public GameObject arrowPrefab;

    private UnitMovement myMovement;

    void Start() {
        myMovement = GetComponent<UnitMovement>();
    }

    private Vector3 castDirection;

    public override void DoCast(Vector3 mouse) {
        // Animate the fireball
        myMovement.animator.SetTrigger("volley");

        castDirection = mouse - transform.position;
        castDirection.y = 0;

        myMovement.director.IWantToFace(castDirection);
    }

    public override void Execute() {
        for (float angle = -15.0f; angle <= 15.0f; angle += 5.0f) {
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * castDirection;
            direction.Normalize();

            GameObject arrow = GameObject.Instantiate(arrowPrefab);
            arrow.transform.position = transform.position + Vector3.up + direction;
            arrow.transform.rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.AngleAxis(90, Vector3.right);

            ProjectileScript projectile = arrow.GetComponent<ProjectileScript>();
            projectile.onCollision = Collide;
            projectile.velocity = 12.0f * direction;
            projectile.life = 0.65f;
        }

        GetComponent<UnitSpells>().DoneCasting();
    }

    bool Collide(ProjectileScript script, GameObject other) {
        UnitWithHealth enemy = other.GetComponent<UnitWithHealth>();
        if (enemy != null) {
            enemy.TakeDamage(50);
            return true;
        }

        // Not an enemy
        return false;
    }
}
