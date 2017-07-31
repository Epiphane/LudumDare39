using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpells : MonoBehaviour {

    public enum Spell {
        None,
        BasicAttack,
        Fireball, // Q
        Flamestrike, // W
        Volley
    };

    protected GameObject currentSpell;

    // For telling the PlayerMovement component that we're in the middle of something duuuuude
    public bool isCasting { get { return _isCasting; } }
    public bool isIndicating { get { return _isIndicating != Spell.None; } }
    public Spell _isIndicating = Spell.None; // Spell index
    private bool _isCasting;
    public Transform indicatorRoot;

    public bool CanCast(Spell ability) {
        switch (ability) {
        case Spell.BasicAttack:
            return GetComponent<BasicAttack>().isOffCooldown;
        case Spell.Fireball:
            return GetComponent<Fireball>().isOffCooldown;
        case Spell.Volley:
            return GetComponent<Volley>().isOffCooldown;
        default:
            return true;
        }
    }

    /* Returns whether or not the ability is ready for casting */
    public bool IndicateAbility(Spell ability) {
        if (ability == Spell.None)
            return true; // ????

        if (_isIndicating == ability) {
            // TODO Cancel spell on pressing the letter again?
            return true;
        }

        if (!CanCast(ability)) {
            return false;
        }

        if (currentSpell != null) {
            GameObject.Destroy(currentSpell);
            currentSpell = null;
        }

        switch (_isIndicating = ability) {
        case Spell.BasicAttack:
            break; // No indicator
        case Spell.Fireball:
            currentSpell = GameObject.Instantiate(GetComponent<Fireball>().indicatorPrefab, indicatorRoot);
            break;
        case Spell.Flamestrike:
            currentSpell = GameObject.Instantiate(GetComponent<Flamestrike>().indicatorPrefab, indicatorRoot);
            break;
        case Spell.Volley:
            currentSpell = GameObject.Instantiate(GetComponent<Volley>().indicatorPrefab, indicatorRoot);
            break;
        default:
            Debug.Log("Indicating nothing :O");
            break;
        }

        return true;
    }

    public void BasicAttack() {
        if (CanCast(Spell.BasicAttack)) {
            _isCasting = true;
            GetComponent<BasicAttack>().Cast(Vector3.zero);
        }
    }

    public void CastAbility(Vector3 point) {
        _isCasting = true;
        switch (_isIndicating) {
        case Spell.BasicAttack:
            GetComponent<BasicAttack>().Cast(Vector3.zero);
            break;
        case Spell.Fireball:
            GetComponent<Fireball>().Cast(point);
            break;
        case Spell.Flamestrike:
            GetComponent<Flamestrike>().Cast(point);
            break;
        case Spell.Volley:
            GetComponent<Volley>().Cast(point);
            break;
        default:
            Debug.Log("Casting nothing :O");
            DoneCasting();
            return;
        }
    }

    public void DoneCasting() {
        _isIndicating = Spell.None;
        _isCasting = false;
    }
}
