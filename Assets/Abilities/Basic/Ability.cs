using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {

    public class Stats {
        public int basePhysical;
        public float scalingPhysical;
        public int baseMagic;
        public float scalingMagic;

        public Stats(int basePhysical, float scalingPhysical, int baseMagic, float scalingMagic) {
            this.basePhysical    = basePhysical;
            this.scalingPhysical = scalingPhysical;
            this.baseMagic       = baseMagic;
            this.scalingMagic    = scalingMagic;
        }
    };

    protected UnitWithHealth caster;

    public float cooldown = 1.0f;
    private float lastCast = 0;
    public bool isOffCooldown {
        get {
            return Time.time > lastCast + cooldown || lastCast == 0;
        }
    }

	// Use this for initialization
	public void Awake () {
        caster = GetComponent<UnitWithHealth>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Cast(Vector3 mouse) {
        lastCast = Time.time;

        DoCast(mouse);
    }

    public abstract void DoCast(Vector3 mouse);

    public abstract void Execute(); // For when the animation hits the magic moment
}
