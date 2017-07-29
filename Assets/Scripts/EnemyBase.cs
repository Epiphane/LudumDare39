using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    public int health;

    // Don't kill dummies
    public bool isDummy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isDummy) {
            return;
        }

        if (health < 0) {
            GameObject.Destroy(gameObject);
        }
	}
}
