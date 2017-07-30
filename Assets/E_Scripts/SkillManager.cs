using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Schools { Magic, Deception, Strength };

/// <summary>
/// I handle anything to do with skills, and anything to do with EXP.
/// Ask for my super sexy singleton in your area
/// </summary>
public class SkillManager : MonoBehaviour {

	// When you "level up,"  you get to remove a point from something. YAYYY
	int magicPointsToRemove = 0;
	int deceptionPointsToRemove = 0;
	int strengthPointsToRemove = 0;

	int totalMagicevels = 0;
	int totalDeceptionLevels = 0;
	int totalStrengthLevels = 0;

	// Current EXP
	public int magicXP = 0;
	public int deceptionXP = 0;
	public int strengthXP = 0;

	public Canvas skillCanvas;
	public Button continueButton;

	// How much XP u need to do a level
	public static int[] levelReqs = {
		5, 10, 15, 20, 25, 30, 40, 50, 60
	};

	// Use this for initialization
	void Start () {
		UpdateSkillKids ();
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

		foreach (SkillNode skill in skillz) {
			skill.UpdateState ();
		}
	}

	public void ShowSkillScreen() {
		skillCanvas.gameObject.SetActive (true);
		
		CheckGoodToGo ();
	}

	public void ContinueButton() {
		skillCanvas.gameObject.SetActive (false);
	}


	void CheckGoodToGo() {
		if (deceptionPointsToRemove == 0 &&
		    magicPointsToRemove == 0 &&
		    strengthPointsToRemove == 0) {
			continueButton.gameObject.SetActive (true);
		} else {
			continueButton.gameObject.SetActive (false);
		}
	}


	public void SkillPointDestroyed(Schools whichSchool) {
		switch (whichSchool) {
		case Schools.Deception:
			deceptionPointsToRemove--;
			break;
		case Schools.Magic:
			magicPointsToRemove--;
			break;
		case Schools.Strength:
			strengthPointsToRemove--;
			break;
		}

		CheckGoodToGo ();

		SkillNode[] skillz = FindObjectsOfType(typeof(SkillNode)) as SkillNode[];
		foreach (SkillNode skill in skillz) {
			skill.UpdateState ();
		}
	}
}
