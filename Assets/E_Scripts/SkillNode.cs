using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode : MonoBehaviour {

	public List<SkillNode> parents;
	public List<SkillNode> kids;

	/// <summary>
	/// Parents are assigned by hand, kids are automatically added based on that
	/// </summary>
	public void UpdateKids() {
		// Look at my parents
		foreach (SkillNode p in parents) {
			// Hello parent, I am your child
			p.kids.Add(this);
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
		if (parents.Count > 0) {
			// Can't remove a skill with parents still alive
			print("U CAN'T DO THAT");
		}
	}
}
