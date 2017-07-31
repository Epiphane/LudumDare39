using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : UnitSpells {

    public Spell qSpell;
    public Spell wSpell;
    public Spell eSpell;
    public Spell rSpell;
    public Spell dSpell;
    public Spell fSpell;

    public AbilityButton qUI;
    public AbilityButton wUI;
    public AbilityButton eUI;
    public AbilityButton rUI;
    public AbilityButton dUI;
    public AbilityButton fUI;

    private string Cast = "Fire1";

    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            IndicateAbility(qSpell);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            IndicateAbility(wSpell);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            IndicateAbility(eSpell);
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

    new public void CastAbility(Vector3 point) {
        base.CastAbility(point);

        switch (_isIndicating) {
        case Spell.BasicAttack:
            break;
        case Spell.Fireball:
            qUI.SetCooldown(GetComponent<Fireball>().cooldown);
            break;
        case Spell.Flamestrike:
            wUI.SetCooldown(GetComponent<Flamestrike>().cooldown);
            break;
        case Spell.Volley:
            eUI.SetCooldown(GetComponent<Volley>().cooldown);
            break;
        default:
            return;
        }
    }
}
