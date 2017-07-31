using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public delegate bool ProjectileCollisionDelegate(ProjectileScript script, GameObject other);

    public Vector3 velocity;
    public float life = 0.25f;

    public ProjectileCollisionDelegate onCollision;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().velocity = this.velocity;

        life -= Time.deltaTime;
        if (life < 0)
            GameObject.Destroy(gameObject);
	}

    void OnTriggerEnter(Collider other) {
        if (onCollision != null && onCollision(this, other.gameObject)) {
            GameObject.Destroy(gameObject);
        }
    }
}
