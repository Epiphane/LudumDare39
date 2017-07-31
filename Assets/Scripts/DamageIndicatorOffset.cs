using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorOffset : MonoBehaviour {

    public Transform sourceOfTruth;
    public Vector3 offset { get { return _offset; } }
    private Vector3 _offset;

	// Use this for initialization
	void Awake () {
        _offset = sourceOfTruth.position - sourceOfTruth.parent.position;
        Debug.Log(sourceOfTruth.parent.position);
        Debug.Log(sourceOfTruth.position);
    }
}
