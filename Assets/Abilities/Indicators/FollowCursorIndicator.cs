using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursorIndicator : SpellIndicator {

    public Transform objectToFollow;
    public float radius;

    void Awake() {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void FollowMouse (Vector3 mouse) {
        Vector3 distance = mouse - transform.position;
        distance.y = 0;

        if (distance.magnitude > radius) {
            distance.Normalize();
            distance.Scale(radius * Vector3.one);
        }

        objectToFollow.position = transform.position + distance;
    }
}
