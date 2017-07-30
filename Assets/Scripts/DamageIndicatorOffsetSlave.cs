using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorOffsetSlave : MonoBehaviour {

    private Transform parent;
    public DamageIndicatorOffset sourceOfTruth;

    // Use this for initialization
    void Start() {
        if (sourceOfTruth == null) {
            sourceOfTruth = GameObject.Find("DamageIndicatorOffset").GetComponent<DamageIndicatorOffset>();
        }

        // Reparent me
        parent = transform.parent;
        transform.parent = transform.parent.parent;
    }

    void Update() {
        if (parent == null) {
            GameObject.Destroy(gameObject);
            return;
        }

        transform.position = parent.position + sourceOfTruth.offset;
    }
}
