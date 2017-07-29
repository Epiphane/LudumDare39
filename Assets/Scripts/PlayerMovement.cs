using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 15.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 direction = speed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        GetComponent<CharacterController>().SimpleMove(direction);
        //GetComponent<Rigidbody>().velocity = direction;
    }
}
