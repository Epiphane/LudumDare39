using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSmoother : MonoBehaviour {

    public Transform toDirect;
    public Quaternion direction;

	// Use this for initialization
	void Start () {
        if (toDirect == null) {
            toDirect = transform;
        }
        direction = toDirect.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        toDirect.rotation = Quaternion.Slerp(toDirect.rotation, direction, smoothing);
    }

    public float smoothing = 0.2f;

    public void IWantToFace(Vector3 _direction) {
        _direction.y = 0;

        direction = Quaternion.LookRotation(_direction, Vector3.up);
    }
}
