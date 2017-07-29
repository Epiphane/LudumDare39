using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkToDeath : MonoBehaviour {

    public float timeToLive = 0.15f;
    private float scale = 1.0f;

	// Use this for initialization
	void Start () {
        scale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.one * scale;

        scale -= Time.deltaTime / timeToLive;
        if (scale < 0)
            GameObject.Destroy(gameObject);
    }
}
