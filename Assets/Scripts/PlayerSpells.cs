using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour {

    public enum Spell {
        BasicAttack,
        Fireball, // Q
        Flamestrike, // W
        None
    };

    private GameObject currentSpell;

    // For telling the PlayerMovement component that we're in the middle of something duuuuude
    public bool isCasting { get { return _isCasting; } }
    public bool isIndicating { get { return _isIndicating != Spell.None; } }
    public Spell _isIndicating = Spell.None; // Spell index
    private bool _isCasting;
    public Transform indicatorRoot;

    public Spell qSpell;
    public Spell wSpell;

    public GameObject fireballIndicator;
    public GameObject flamestrikeIndicator;

    // Use this for initialization
    void Start () {
		
	}

    private string Cast = "Fire1";

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            IndicateAbility(qSpell);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            IndicateAbility(wSpell);
        }

        if (currentSpell != null) {
            CharacterController controller = GetComponent<CharacterController>();
            Plane plane = new Plane(Vector3.up, transform.position.y - controller.height / 2);

            float dist;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out dist)) {
                Vector3 point = ray.GetPoint(dist);

                currentSpell.GetComponent<SpellIndicator>().FollowMouse(point);

                if (Input.GetButtonDown(Cast)) {
                    CastAbility(point);

                    GameObject.Destroy(currentSpell);
                    currentSpell = null;
                }
            }
        }
    }

    public bool CanCast(Spell ability) {
        switch (ability) {
        case Spell.BasicAttack:
            return GetComponent<BasicAttack>().isOffCooldown;
        case Spell.Fireball:
            return GetComponent<Fireball>().isOffCooldown;
        default:
            return true;
        }
    }

    /* Returns whether or not the ability is ready for casting */
    public bool IndicateAbility(Spell ability) {
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
            currentSpell = GameObject.Instantiate(fireballIndicator, indicatorRoot);
            break;
        case Spell.Flamestrike:
            currentSpell = GameObject.Instantiate(flamestrikeIndicator, indicatorRoot);
            break;
        default:
            Debug.Log("Indicating nothing :O");
            break;
        }

        return true;
    }

    public void BasicAttack() {
        if (CanCast(Spell.BasicAttack)) {
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
