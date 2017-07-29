using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTooltip : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject tooltip;

	public void RevealTooltip() {
		tooltip.SetActive (true);
	}

	public void HideTooltip() {
		tooltip.SetActive (false);
	}
}
