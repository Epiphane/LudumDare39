using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public Transform toFollow;
    public float smooth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 destination = toFollow.position;
        Vector3 position = transform.position;
        destination.y = position.y;

        //if ((destination.z - position.z < .05) && (destination.z - position.z > 0)) {
        //    moveTheCamera = false;
        //} else if (moveTheCamera == true) {
        //if (Mathf.Approximately(transform.eulerAngles.y, 0)) {
        //    targetPosition2 = (transform.position + (targetPosition * -5));
        //}
        //if (Mathf.Approximately(transform.eulerAngles.y, 180)) {
        //    targetPosition2 = (transform.position + (targetPosition * -5));
        //}
        
        transform.position = Vector3.Lerp(position, destination, smooth * Time.deltaTime);
        //}
    }
}
