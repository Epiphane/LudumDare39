﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillNodeState { CanDestroy, CannotDestroy, Destroyed }

public class SkillNode : MonoBehaviour {

	public List<SkillNode> parents;
	public List<SkillNode> kids;

	public SkillNodeState state;

	/// <summary>
	/// Parents are assigned by hand, kids are automatically added based on that
	/// </summary>
	public void UpdateKids() {
		// Look at my parents
		foreach (SkillNode p in parents) {
			if (p == null) {
				Debug.LogWarning ("OH NO NULL PARENT FOUND IN " + gameObject.name);
			}
			// Hello parent, I am your child
			p.kids.Add(this.GetComponent<SkillNode>());
		}
	}

	// Update this node's state based on the nodes around it
	public void UpdateState() {
		if (state == SkillNodeState.Destroyed) {
			return;
		}

		state = SkillNodeState.CanDestroy;

		foreach (SkillNode p in parents) {
			if (p.kids.Count == 1) {
				state = SkillNodeState.CannotDestroy;
				return;
			}
		}
	}

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

	public void SkillChosen() {
		if (state != SkillNodeState.CanDestroy) {
			print ("CAN'T DO IT");
		} else {
			print ("CAN DO IT");

		}
	}
}
