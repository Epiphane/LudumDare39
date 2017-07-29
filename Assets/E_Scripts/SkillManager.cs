using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateSkillKids() {
		SkillNode[] skillz = FindObjectsOfType(typeof(SkillNode)) as SkillNode[];
		foreach (SkillNode skill in skillz) {
			skill.kids.Clear ();
		}

		foreach (SkillNode skill in skillz) {
			skill.UpdateKids();
		}
	}
}
