using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchEnemies : MonoBehaviour {

    [HideInInspector]
    public LevelManager levelManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        EnemyBase existingEnemy = GameObject.FindObjectOfType<EnemyBase>();
        if (existingEnemy == null) {
            levelManager.Advance();

            GameObject.Destroy(gameObject);
        }
	}
}
