using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    private float delay = 0.3f;
    private float clickTime = 0;

    public float speed = 15.0f;
    public float accel = 1.0f;
    private float currentSpeed = 0;
    public float maxSpeed = 15.0f;
    private Vector3 velocity;

    // Enemy targeting
    private EnemyBase targetedEnemy = null;
    private GameObject eIndicator = null;

    // Movement
    private Vector3 destination;
    public GameObject moveIndicator;

    // Use this for initialization
    void Start () {
        velocity = Vector3.zero;
    }

    private string Move = "Fire2";

    void HitEnemy() {
        if (targetedEnemy != null) {
            // DO damage
        }
    }

    GameObject CreateMoveIndicator(Vector3 position, bool isPersistent = false) {
        GameObject indicator = GameObject.Instantiate(moveIndicator);
        indicator.transform.position = position;
        indicator.GetComponent<ShrinkToDeath>().enabled = !isPersistent;

        return indicator;
    }

    /* Returns true if an enemy got targeted */
    bool IssueMove() {
        bool didSelectSomething = AttemptSelect();

        if (didSelectSomething) {
            destination = eIndicator.transform.position;
        }
        else {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layer_mask = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out hit, 100, layer_mask)) {
                destination = hit.point;
            }
        }

        return didSelectSomething;
    }

    /* Returns true if an enemy got selected */
    bool AttemptSelect() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layer_mask = LayerMask.GetMask("Interactable");
        if (Physics.Raycast(ray, out hit, 100, layer_mask)) {
            targetedEnemy = hit.collider.GetComponent<EnemyBase>();
            if (targetedEnemy != null) {
                Vector3 enemyPosition = targetedEnemy.transform.position;
                enemyPosition.y = 0.45f;

                if (eIndicator != null) {
                    GameObject.Destroy(eIndicator);
                }

                // Create an indicator under the enemy
                eIndicator = CreateMoveIndicator(enemyPosition, true);
                return true;
            }
        }

        return false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetButtonDown("Fire1")) {
            AttemptSelect();
        }

        if (Input.GetButtonDown(Move)) {
            clickTime = Time.time;
            IssueMove();
        }
        if (Input.GetButtonUp(Move)) {
            if (Time.time - clickTime <= delay) {
                // Click
                IssueMove();

                // Create an indicator of where you're moving
                CreateMoveIndicator(destination + Vector3.up * 0.1f);
            }
        }
        if (Input.GetButton(Move)) {
            if (Time.time - clickTime > delay) {
                // Drag
                IssueMove();
            }
        }

        Vector3 direction = destination - transform.position;
        direction.y = 0;
        if (direction.magnitude > 2 * currentSpeed * Time.fixedDeltaTime) {
            direction.Normalize();
            direction.Scale(Vector3.one * currentSpeed);
            currentSpeed += accel * Time.fixedDeltaTime;
        }
        else {
            currentSpeed = direction.magnitude;

            if (currentSpeed > maxSpeed)
                currentSpeed = maxSpeed;
        }

        velocity = Vector3.Slerp(velocity, direction, 0.5f);
        if (velocity.magnitude > maxSpeed) {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        if (velocity.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        GetComponent<CharacterController>().SimpleMove(velocity);
    }
}
