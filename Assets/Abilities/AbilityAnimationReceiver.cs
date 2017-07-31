using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAnimationReceiver : MonoBehaviour {

    /* Here's the deal:
     * When an animation hits a named event, it tries to call the 
     * name as a function, by searching [the object that has the animator]
     * for any components with that function name. Since the object's info
     * (like health, etc) is stored in a parent object, this script catches
     * parent animation events and sends them to the relevant component.
     */

    // The actual unit
    public UnitWithHealth myUnit;

	// Use this for initialization
	void Start () {
        // Try this object first
        myUnit = GetComponent<UnitWithHealth>();

        if (myUnit != null)
            return;

        // Try the parent next
        myUnit = transform.parent.GetComponent<UnitWithHealth>();

        if (myUnit != null)
            return;

        Debug.LogWarning("Could not find UnitWithHealth component!");
    }

    public void BasicAttackComplete() {
        myUnit.GetComponent<BasicAttack>().Execute();
    }

    public void FireballComplete() {
        myUnit.GetComponent<Fireball>().Execute();
    }

    public void FlamestrikeComplete() {
        myUnit.GetComponent<Flamestrike>().Execute();
    }

    public void VolleyComplete() {
        myUnit.GetComponent<Volley>().Execute();
    }
}
