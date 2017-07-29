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

    private Transform targetedEnemy;
    private bool walking;
    private bool enemyClicked;
    private Vector3 destination;
    public GameObject moveIndicator;

    // Use this for initialization
    void Start () {
        velocity = Vector3.zero;
    }

    private string Move = "Fire2";

    void IssueMove() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layer_mask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out hit, 100, layer_mask)) {
            if (hit.collider.CompareTag("Enemy")) {
                targetedEnemy = hit.transform;
                enemyClicked = true;
            } else {
                walking = true;
                enemyClicked = false;
                destination = hit.point;
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetButtonDown(Move)) {
            clickTime = Time.time;
            IssueMove();
        }
        if (Input.GetButtonUp(Move)) {
            if (Time.time - clickTime <= delay) {
                // Click
                IssueMove();

                // Create an indicator of where you're moving
                GameObject indicator = GameObject.Instantiate(moveIndicator);
                indicator.transform.position = destination + Vector3.up * 0.1f;
            }
        }
        if (Input.GetButton(Move)) {
            //if (Time.time - clickTime > delay) {
                // Drag
                IssueMove();
            //}
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

        GetComponent<CharacterController>().SimpleMove(velocity);
    }
}
