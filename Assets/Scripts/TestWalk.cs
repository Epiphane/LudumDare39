using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWalk : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public ParticleSystem powerupParticles;

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			transform.Translate (0.0f, 0.0f, 0.1f);
			GetComponent<Animator> ().SetFloat ("moveSpeed", 1.0f);
			transform.localRotation = Quaternion.AngleAxis (0, Vector3.up);
		} else if (Input.GetKey (KeyCode.S)) {
			transform.Translate (0.0f, 0.0f, 0.1f);
			GetComponent<Animator> ().SetFloat ("moveSpeed", 1.0f);
			transform.localRotation = Quaternion.AngleAxis (180, Vector3.up);
		} else {
			GetComponent<Animator> ().SetFloat ("moveSpeed", 0.0f);
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			GetComponent<Animator> ().SetTrigger ("fireballThrow");
		} else if (Input.GetKeyDown (KeyCode.E)) {
			GetComponent<Animator> ().SetTrigger ("brandAttack");
		}
	}

	public void Fart() {
		powerupParticles.Play ();
	}
}
