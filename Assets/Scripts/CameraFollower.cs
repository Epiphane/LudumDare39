using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public Transform toFollow;
    public float smooth;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - toFollow.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 destination = toFollow.position + offset;
        Vector3 position = transform.position;
        
        transform.position = Vector3.Lerp(position, destination, smooth * Time.deltaTime);
    }
}
