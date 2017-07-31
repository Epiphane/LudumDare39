using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour {

    public float DPS;
    public float delay;
    public float duration;

    private float _delay;
    private float _duration;
    private float tick;

	// Use this for initialization
	void Start () {
        tick = 0.25f;
        _delay = delay;
        _duration = duration;
    }

    // https://github.com/justonia/UnityExtensions/blob/master/PhysicsExtensions.cs
    private Vector3 AbsVec3(Vector3 v) {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    // https://github.com/justonia/UnityExtensions/blob/master/PhysicsExtensions.cs
    public void ToWorldSpaceCapsule(CapsuleCollider capsule, out Vector3 point0, out Vector3 point1, out float radius) {
        var center = capsule.transform.TransformPoint(capsule.center);
        radius = 0f;
        float height = 0f;
        Vector3 lossyScale = AbsVec3(capsule.transform.lossyScale);
        Vector3 dir = Vector3.zero;

        switch (capsule.direction) {
        case 0: // x
            radius = Mathf.Max(lossyScale.y, lossyScale.z) * capsule.radius;
            height = lossyScale.x * capsule.height;
            dir = capsule.transform.TransformDirection(Vector3.right);
            break;
        case 1: // y
            radius = Mathf.Max(lossyScale.x, lossyScale.z) * capsule.radius;
            height = lossyScale.y * capsule.height;
            dir = capsule.transform.TransformDirection(Vector3.up);
            break;
        case 2: // z
            radius = Mathf.Max(lossyScale.x, lossyScale.y) * capsule.radius;
            height = lossyScale.z * capsule.height;
            dir = capsule.transform.TransformDirection(Vector3.forward);
            break;
        }

        if (height < radius * 2f) {
            dir = Vector3.zero;
        }

        point0 = center + dir * (height * 0.5f - radius);
        point1 = center - dir * (height * 0.5f - radius);
    }

    // Update is called once per frame
    void Update () {
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        if (_delay > 0) {
            _delay -= Time.deltaTime;
            return;
        }

        _duration -= Time.deltaTime;
        if (_duration < 0) {
            return;
        }

        tick -= Time.deltaTime;
        while (tick < 0) {
            tick += 0.25f;

            CapsuleCollider c = GetComponent<CapsuleCollider>();

            Vector3 point0, point1;
            float radius;
            ToWorldSpaceCapsule(c, out point0, out point1, out radius);

            int layer_mask = LayerMask.GetMask("Interactable");
            Collider[] overlapping = Physics.OverlapCapsule(point0, point1, radius, layer_mask);
            foreach (Collider overlap in overlapping) {
                EnemyBase enemy = overlap.GetComponent<EnemyBase>();

                if (enemy != null) {
                    enemy.TakeDamage(Mathf.FloorToInt(DPS / 4.0f));
					print ("Here's where you could say like, damage *= " + SkillManager.skillPoints ["flamestrike"]);
                }
            }
        }
    }
}
