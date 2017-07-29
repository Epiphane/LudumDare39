﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour {

    public EnemyBase myInfo;

	// Use this for initialization
	void Start () {
        myInfo = transform.parent.parent.GetComponent<EnemyBase>();

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector2(500.0f / myInfo.maxHealth, 1);
    }
	
	// Update is called once per frame
	void Update () {
        RectTransform rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(myInfo.currentHealth, 100);
    }
}
