using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.PyroParticles;

public class Flamestrike : Ability {

    Stats[] levels = {
        new Stats(0, 0, 0, 0),
        new Stats(0, 0, 50, 0.5f),
        new Stats(0, 0, 50, 0.6f),
        new Stats(0, 0, 50, 0.7f)
    };

    float[] scale = { 0, 0.5f, 1.0f, 1.5f };

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
        int points = SkillManager.currentSkills["flamestrike"].currPoints;

        GameObject flamestrike = GameObject.Instantiate(flamestrikePrefab);
        flamestrike.transform.position = castPosition;
        flamestrike.transform.localScale = new Vector3(scale[points], 1, scale[points]);

        DamageArea damager = flamestrike.GetComponentInChildren<DamageArea>();
        damager.DPS = levels[points];
        damager.caster = caster;

        GetComponent<UnitSpells>().DoneCasting();
    }
}
