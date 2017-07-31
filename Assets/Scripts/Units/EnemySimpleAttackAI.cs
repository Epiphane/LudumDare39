﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleAttackAI : MonoBehaviour {

    public float detectionRadius = 5.0f;

    private UnitMovement movement;

	// Use this for initialization
	void Start () {
        movement = GetComponent<UnitMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        UnitWithHealth myHealth = GetComponent<UnitWithHealth>();

        if (myHealth.currentHealth < myHealth.maxHealth) {
            // Fight back!
            movement.Target(GameObject.FindGameObjectWithTag("Player"), true);
        }

        if (!movement.isMovingToTarget) {
            Collider[] overlaps = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach (Collider overlap in overlaps) {
                if (overlap.tag == "Player") {
                    movement.Target(overlap.gameObject, true);
                }
            }
        }
	}
}