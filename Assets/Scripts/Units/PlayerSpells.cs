using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : UnitSpells {

    public Spell qSpell;
    public Spell wSpell;

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
}
