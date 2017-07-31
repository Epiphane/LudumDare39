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
	public Schools school;
	public Skill(int mp, Schools s, float val1, float val2, float val3, string t, string tc, string b) {
		maxPoints = mp;
		currPoints = mp;
		school = s;
		values = new float[3] { val1, val2, val3 };
		tooltip = string.Format ("<color={0}>{1}</color>\n{2}", tc, t, b);
		textOverrideValues = new string[0] { };
	}

	public float GetValue() {
		if (currPoints == 0) {
			return 0;
		}
		return values [currPoints - 1];
	}

	public Skill MinusOne() {
		Skill urgh = new Skill ();
		urgh.maxPoints = maxPoints;
		urgh.currPoints = currPoints - 1;
		urgh.values = values;
		urgh.tooltip = tooltip;
		urgh.school = school;
		return urgh;
	}
}

/// <summary>
/// I handle anything to do with skills, and anything to do with EXP.
/// Ask for my super sexy singleton in your area
/// </summary>
public class SkillManager : MonoBehaviour {

	// When you "level up,"  you get to remove a point from something. YAYYY
	int magicPointsToRemove = 3;
	int deceptionPointsToRemove = 0;
	int strengthPointsToRemove = 0;

	int totalMagicLevels = 0;
	int totalDeceptionLevels = 0;
	int totalStrengthLevels = 0;

	// Current EXP
	public int magicXP = 0;
	public int deceptionXP = 0;
	public int strengthXP = 0;

	public GameObject skillCanvas;
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
		CheckGoodToGo ();
	}

	public static Dictionary<string, Skill> currentSkills = new Dictionary<string, Skill>()
	{
		{ "fireball", new Skill(3, Schools.Magic, 20, 30, 40, "Fireball", "red", "Blast an enemy with a fireball for $VALUE damage.") },
		{ "flamestrike", new Skill(3, Schools.Magic, 20, 30, 40, "Flamestrike", "red", "Douse an area in fire for $VALUE damage per second.") },
		{ "volley", new Skill(3, Schools.Deception, 20, 30, 40, "Volley", "green", "Shoot a volley of arrows in a cone for $VALUE damage.") }
	};
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			ShowSkillScreen ();
		}
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

		UpdateSkillKids ();
		CheckGoodToGo ();
	}

	public void ContinueButton() {
		skillCanvas.gameObject.SetActive (false);
	}

	public GameObject magic_remove_text;
	public GameObject strength_remove_text;
	public GameObject deception_remove_text;

	void CheckGoodToGo() {
		magic_remove_text.SetActive (magicPointsToRemove > 0);
		deception_remove_text.SetActive (deceptionPointsToRemove > 0);
		strength_remove_text.SetActive (strengthPointsToRemove > 0);

		magic_remove_text.GetComponent<TMPro.TextMeshProUGUI> ().text = string.Format ("<size=26>POWER DOWN</size>\n<size=20>{0} REMAINING</size>", magicPointsToRemove);
		deception_remove_text.GetComponent<TMPro.TextMeshProUGUI> ().text = string.Format ("<size=40>POWER DOWN</size>\n<size=20>REMOVE {0} UPGRADE POINTS \nTO CONTINUE</size>", deceptionPointsToRemove);
		strength_remove_text.GetComponent<TMPro.TextMeshProUGUI> ().text = string.Format ("<size=40>POWER DOWN</size>\n<size=20>REMOVE {0} UPGRADE POINTS \nTO CONTINUE</size>", strengthPointsToRemove);

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

		print ("hey " + magicPointsToRemove);
		CheckGoodToGo ();

		//SkillNode[] skillz = FindObjectsOfType(typeof(SkillNode)) as SkillNode[];
		//foreach (SkillNode skill in skillz) {
		//	skill.UpdateState ();
		//}
	}
}
