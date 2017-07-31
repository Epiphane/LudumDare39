using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Schools { Magic, Deception, Strength };

public struct Skill {
	public int maxPoints, currPoints;
	public float[] values;
	public string[] textOverrideValues; // If you need some other number here for the tooltip, put it here
	public string tooltip;
	public Skill(int mp, float val1, float val2, float val3, string t, string tc, string b) {
		maxPoints = mp;
		currPoints = mp;
		values = new float[3] { val1, val2, val3 };
		tooltip = string.Format ("<color={0}>{1}</color>\n{2}", tc, t, b);
		textOverrideValues = new string[0] { };
	}

	public float GetValue() {
		return values [currPoints - 1];
	}
}

/// <summary>
/// I handle anything to do with skills, and anything to do with EXP.
/// Ask for my super sexy singleton in your area
/// </summary>
public class SkillManager : MonoBehaviour {

	// When you "level up,"  you get to remove a point from something. YAYYY
	int magicPointsToRemove = 0;
	int deceptionPointsToRemove = 0;
	int strengthPointsToRemove = 0;

	int totalMagicLevels = 0;
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

	public static Dictionary<string, int> skillPoints = new Dictionary<string, int>()
	{
		{ "fireball", 3 },
		{ "flamestrike", 3 },
	};

	// Use this for initialization
	void Start () {
		UpdateSkillKids ();
	}

	public static Dictionary<string, Skill> currentSkills = new Dictionary<string, Skill>()
	{
		{ "fireball", new Skill(3, 20, 30, 40, "Fireball", "red", "Blast an enemy with a fireball for $VALUE damage.") }
	};
	
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
