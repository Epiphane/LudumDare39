using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellIndicator : MonoBehaviour {

    void Awake() {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void FollowMouse(Vector3 mouse);
}
