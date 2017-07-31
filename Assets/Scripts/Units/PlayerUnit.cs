using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUnit : UnitWithHealth {

    new public float armor {
        get {
            return 10 * SkillManager.currentSkills["thick skin"].currPoints + b_armor;
        }
    }
    new public float intelligence {
        get {
            return 10 * SkillManager.currentSkills["intellect"].currPoints + b_intelligence;
        }
    }

    new public void TakeDamage (int damage, UnitWithHealth from) {
        base.TakeDamage(damage, from);

        if (SkillManager.currentSkills["bramble skin"].currPoints > 0) {
            from.TakeDamage(damage / 10, this);
        }
    }

    public GameObject gameOverScreen;

    new protected void Update() {
        if (isDead) {
            gameOverScreen.SetActive(true);
        }
    }
}
