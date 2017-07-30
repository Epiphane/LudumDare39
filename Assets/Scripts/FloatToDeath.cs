using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatToDeath : MonoBehaviour {

    public float timeToLive = 0.15f;
    private float progress = 0.0f;
    private Vector3 startPos;
    private Vector3 dposition;

	// Use this for initialization
	void Start () {
        progress = 0.0f;

        startPos = transform.position;
        float v = 1.7f;
        dposition = new Vector3(Random.Range(-v, v), 1, Random.Range(-v, v));
    }
	
	// Update is called once per frame
	void Update () {
        progress += Time.deltaTime / timeToLive;
        if (progress > 1) {
            GameObject.Destroy(gameObject);
            return;
        }

        float p = progress * 1.5f;
        transform.position = startPos + new Vector3(progress / 3, 2 * p - p * p, 0);
        transform.localScale = 0.25f * Vector3.one * Mathf.Sqrt(1 - progress);
    }
}
