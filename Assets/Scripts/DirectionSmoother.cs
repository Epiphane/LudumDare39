using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSmoother : MonoBehaviour {

    public Quaternion direction;

	// Use this for initialization
	void Start () {
        direction = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Slerp(transform.rotation, direction, smoothing);
    }

    public float smoothing = 0.5f;

    public void IWantToFace(Vector3 _direction) {
        _direction.y = 0;

        direction = Quaternion.LookRotation(_direction, Vector3.up);
    }
}
