﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.PyroParticles;

public class Fireball : Ability {

    public GameObject indicatorPrefab;
    public GameObject fireballPrefab;

    private UnitMovement myMovement;

    Stats[] levels = {
        new Stats(0, 0, 0, 0),
        new Stats(0, 0, 30, 0.6f),
        new Stats(0, 0, 50, 0.6f),
        new Stats(0, 0, 70, 0.6f)
    };

    void Start() {
        myMovement = GetComponent<UnitMovement>();
    }

    private Vector3 castDirection;

    public override void DoCast(Vector3 mouse) {
        // Animate the fireball
        myMovement.animator.SetTrigger("fireballThrow");

        castDirection = mouse - transform.position;
        castDirection.y = 0;

        myMovement.director.IWantToFace(castDirection);
    }

    public override void Execute() {
        GameObject fireball = GameObject.Instantiate(fireballPrefab);
        fireball.transform.position = transform.position + Vector3.up + castDirection.normalized;
        fireball.transform.rotation = Quaternion.LookRotation(castDirection, Vector3.up);

        FireProjectileScript projectile = fireball.GetComponent<FireProjectileScript>();
        projectile.CollisionDelegate = Collide;

        GetComponent<UnitSpells>().DoneCasting();
    }

    void Collide(FireProjectileScript script, Vector3 pos, GameObject other) {
        UnitWithHealth enemy = other.GetComponent<UnitWithHealth>();
        if (enemy != null) {
            caster.DealDamage(enemy, levels[SkillManager.currentSkills["fireball"].currPoints]);
        }
    }
}
