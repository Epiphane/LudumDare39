using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {

    public float cooldown = 1.0f;
    public float lastCast = 0.0f;
    public bool isOffCooldown {
        get {
            return Time.time > lastCast + cooldown || lastCast == 0;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Cast(Vector3 mouse) {
        lastCast = Time.time;

        DoCast(mouse);
    }

    public abstract void DoCast(Vector3 mouse);
}
