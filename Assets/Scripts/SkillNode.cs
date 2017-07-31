using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillNodeState { CanDestroy, CannotDestroy, Destroyed }

public class SkillNode : MonoBehaviour {

	public List<SkillNode> parents;
	public List<SkillNode> kids;

	public SkillNodeState state;

	public string skillKey;

	public Text myLabel;
	public Image overlay;

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

	public void SetState(SkillNodeState newState) {
		state = newState;
		switch (newState) {
		case SkillNodeState.CanDestroy:
			// Overlay = 0 alpha
			overlay.color = Color.clear;
			myLabel.color = Color.white;
			break;
		case SkillNodeState.CannotDestroy:
			overlay.color = new Color(0, 0, 0, 0.3f);
			myLabel.color = Color.gray;
			break;
		case SkillNodeState.Destroyed:
			// Overlay = 80 alpha
			// Text color = red
			overlay.color = new Color(0, 0, 0, 0.8f);
			myLabel.color = Color.red;
			break;
		}
	}

	// Update this node's state based on the nodes around it
	public void UpdateState() {
		if (state == SkillNodeState.Destroyed) {
			return;
		}

		SetState(SkillNodeState.CanDestroy);

		foreach (SkillNode p in parents) {
			if (p.kids.Count == 1) {
				SetState(SkillNodeState.CannotDestroy);
				return;
			}
		}
	}

	// Use this for initialization
	void Start () {
		Skill s = SkillManager.currentSkills [skillKey];
		tooltip.transform.Find("tt_text").GetComponent<TMPro.TextMeshProUGUI>().text = s.tooltip.Replace ("$VALUE", s.GetValue().ToString());

		myLabel.text = s.currPoints.ToString () + "/" + s.maxPoints.ToString ();
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
		Skill s = SkillManager.currentSkills [skillKey];
		if (state != SkillNodeState.CanDestroy) {
			print ("CAN'T DO IT");
		} else {
			Skill oneLess = s.MinusOne ();
			SkillManager.currentSkills [skillKey] = oneLess;
			myLabel.text = oneLess.currPoints.ToString () + "/" + oneLess.maxPoints.ToString ();
			print ("Howdy " + oneLess.currPoints);
			GameObject.FindObjectOfType<SkillManager> ().SkillPointDestroyed (s.school);

			if (s.currPoints > 1) {
				tooltip.transform.Find ("tt_text").GetComponent<TMPro.TextMeshProUGUI> ().text = oneLess.tooltip.Replace ("$VALUE", oneLess.GetValue ().ToString ());
			} else {
				SetState (SkillNodeState.Destroyed);
				tooltip.transform.Find ("cover").GetComponent<Image> ().color = new Color (0, 0, 0, 0.75f);
				foreach (SkillNode k in kids) {
					k.parents.Remove (this);
					k.UpdateState ();
				}
			}
		}
	}
}
