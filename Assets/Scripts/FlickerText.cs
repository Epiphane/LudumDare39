using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlickerText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	int frameTillFlicker = 2;
	void Update () {
		if (frameTillFlicker-- == 0) {
			this.GetComponent<TextMeshProUGUI>().materialForRendering.SetFloat ("_SpecularPower", Random.Range (1.1f, 2.5f));
			frameTillFlicker = 2;
		}

		//this.GetComponent<CanvasRenderer> ().material.SetFloat ("_SpecularPower", Random.Range (0.1f, 3.5f));
	}
}
