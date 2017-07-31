﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!ready) {
			cdLeft -= Time.deltaTime;
			if (cdLeft < 0) {
				ready = true;
				gameObject.transform.Find ("darken").GetComponent<Image>().color = Color.clear;
				gameObject.transform.Find ("timeleft").GetComponent<Text> ().color = Color.clear;
			}

			gameObject.transform.Find ("timeleft").GetComponent<Text> ().text = ((int)cdLeft + 1).ToString();
		}
	}

	public bool ready = true;
	public float cdLeft = 0;

	public void SetCooldown(float cd) {
		gameObject.transform.Find ("darken").GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
		cdLeft = cd;

		gameObject.transform.Find ("timeleft").GetComponent<Text> ().color = Color.white;
		gameObject.transform.Find ("timeleft").GetComponent<Text> ().text = ((int)cdLeft).ToString();

		ready = false;
	}

    public Sprite[] below;
    public Sprite[] above;
    public Image belowImage;
    public Image aboveImage;

    public void SetTier(int tier) {
        belowImage.sprite = below[tier];
        aboveImage.sprite = above[tier];
    }
}
