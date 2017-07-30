using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour {

    public enum Spell {
        Fireball, // Q
        None
    };

    // For telling the PlayerMovement component that we're in the middle of something duuuuude
    public Spell _isIndicating = Spell.None; // Spell index
    public bool isCasting { get { return _isCasting; } }
    public bool _isCasting;
    public GameObject currentSpell;
    public Transform indicatorRoot;

    public Spell qSpell;

    public GameObject fireballIndicator;

	// Use this for initialization
	void Start () {
		
	}

    private string Cast = "Fire1";

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            IndicateAbility(qSpell);
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

    void IndicateAbility(Spell ability) {
        if (_isIndicating == ability) {
            // TODO Cancel spell on pressing the letter again?
            return;
        }

        if (currentSpell != null) {
            GameObject.Destroy(currentSpell);
            currentSpell = null;
        }

        switch (_isIndicating = ability) {
        case Spell.Fireball:
            currentSpell = GameObject.Instantiate(fireballIndicator, indicatorRoot);
            break;
        default:
            Debug.Log("Indicating nothing :O");
            return;
        }
    }

    void CastAbility(Vector3 point) {
        _isCasting = true;
        switch (_isIndicating) {
        case Spell.Fireball:
            GetComponent<Fireball>().Cast(point);
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
