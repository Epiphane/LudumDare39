using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	public int magicPointsToRemove = 0;
    public int deceptionPointsToRemove = 0;
    public int strengthPointsToRemove = 0;

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
		{ "fireball", new Skill(3, Schools.Strength, 57, 77, 97, "Fireball", "red", "Throw a flaming boulder for $VALUE mixed damage.") },
        { "bramble skin", new Skill(1, Schools.Strength, 10, 10, 15, "Bramble Skin", "red", "Taking damage from enemies deals $VALUE% back.") },
        { "thick skin", new Skill(3, Schools.Strength, 10, 20, 30, "Think Skin", "red", "Gain an extra $VALUE armor.") },
        { "molten trail", new Skill(2, Schools.Strength, 40, 60, 80, "Molten Trail", "red", "The ground behind you burns enemies for $VALUE mixed damage per second.") },
        { "strength", new Skill(3, Schools.Strength, 10, 20, 30, "Raw Power", "red", "Gain $VALUE power to all physical attacks.") },

        { "flamestrike", new Skill(3, Schools.Magic, 70, 74, 78, "Flamestrike", "purple", "Douse an area in fire for $VALUE magic damage per second.") },
        { "intellect", new Skill(3, Schools.Magic, 10, 20, 30, "Intellect", "purple", "Gain $VALUE power to all magic attacks.") },
        { "burn", new Skill(1, Schools.Magic, 20, 20, 20, "Residual Burn", "purple", "Damaging enemies with magic burns them for $VALUE extra damage.") },
        { "exploding corpse", new Skill(1, Schools.Magic, 75, 95, 115, "Weak spot", "purple", "Enemies magic resist is halved") },
        { "cooldown", new Skill(1, Schools.Magic, 10, 10, 10, "Fast Acting", "purple", "Cooldowns are reduced by $VALUE%.") },

        { "volley", new Skill(3, Schools.Deception, 60, 70, 80, "Volley", "green", "Shoot a volley of arrows in a cone for $VALUE physical damage.") },
        { "aspect of ice", new Skill(3, Schools.Deception, 60, 70, 80, "Volley", "green", "Damaging enemies slows them temporarily.") },
        { "dash", new Skill(1, Schools.Deception, 40, 40, 40, "Dash", "green", "Taking damage provides a burst of $VALUE% movement speed.") },
        { "constitution", new Skill(3, Schools.Deception, 200, 400, 600, "Constitution", "green", "Gain $VALUE health.") },
        { "light steps", new Skill(1, Schools.Deception, 200, 400, 600, "Light Steps", "green", "Enemies may not notice you.") },
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

    public GameObject curse;

    public void ShowCurse() {
        curse.SetActive(true);
    }

	public void ShowSkillScreen() {
        curse.SetActive(false);
        skillCanvas.gameObject.SetActive (true);

		UpdateSkillKids ();
		CheckGoodToGo ();
	}

    public GameObject startGame;
    public void StartButton() {
        startGame.SetActive(false);
        GameObject.FindObjectOfType<LevelManager>().SpawnLevel();
    }

    public void TryAgain() {
        SceneManager.LoadScene("Arena");
    }

	public void ContinueButton() {
		skillCanvas.gameObject.SetActive (false);

        GameObject.FindObjectOfType<LevelManager>().NextLevel();
	}

	public GameObject magic_remove_text;
	public GameObject strength_remove_text;
	public GameObject deception_remove_text;

	void CheckGoodToGo() {
		magic_remove_text.SetActive (magicPointsToRemove > 0);
		deception_remove_text.SetActive (deceptionPointsToRemove > 0);
		strength_remove_text.SetActive (strengthPointsToRemove > 0);

		magic_remove_text.GetComponent<TMPro.TextMeshProUGUI> ().text = string.Format ("<size=32>POWER DOWN</size>\n<size=20>{0} REMAINING</size>", magicPointsToRemove);
		deception_remove_text.GetComponent<TMPro.TextMeshProUGUI> ().text = string.Format ("<size=32>POWER DOWN</size>\n<size=20>{0} REMAINING</size>", deceptionPointsToRemove);
		strength_remove_text.GetComponent<TMPro.TextMeshProUGUI> ().text = string.Format ("<size=32>POWER DOWN</size>\n<size=20>{0} REMAINING</size>", strengthPointsToRemove);

		if (deceptionPointsToRemove == 0 &&
		    magicPointsToRemove == 0 &&
		    strengthPointsToRemove == 0) {
			continueButton.gameObject.SetActive (true);
		} else {
			continueButton.gameObject.SetActive (false);
		}
	}

    public bool NeedsRemoval(Schools whichSchool) {
        switch (whichSchool) {
        case Schools.Deception:
            return (deceptionPointsToRemove > 0);
        case Schools.Magic:
            return (magicPointsToRemove > 0);
        case Schools.Strength:
            return (strengthPointsToRemove > 0);
        }

        return false;
    }

	public void SkillPointDestroyed(Schools whichSchool) {
		switch (whichSchool) {
		case Schools.Deception:
            if (deceptionPointsToRemove <= 0) return;

			deceptionPointsToRemove--;
			break;
		case Schools.Magic:
            if (magicPointsToRemove <= 0) return;

            magicPointsToRemove--;
			break;
		case Schools.Strength:
            if (strengthPointsToRemove <= 0) return;

            strengthPointsToRemove--;
			break;
		}

		CheckGoodToGo ();

		//SkillNode[] skillz = FindObjectsOfType(typeof(SkillNode)) as SkillNode[];
		//foreach (SkillNode skill in skillz) {
		//	skill.UpdateState ();
		//}
	}
}
