using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFromOriginIndicator : SpellIndicator {

    void Awake() {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void FollowMouse (Vector3 mouse) {
        Vector3 direction = mouse - transform.position;
        direction.y = 0;

        transform.rotation = 
            Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.z), Vector3.up) *
            Quaternion.AngleAxis(90, Vector3.right);
    }
}
