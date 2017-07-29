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

    public float attackRange = 1.0f;
    public float attackSpeed = 1.0f;
    private float lastAttack = 0;

    // Enemy targeting
    private EnemyBase targetedEnemy = null;
    private GameObject eIndicator = null;
    private bool isMovingToEnemy;

    // Movement
    private Vector3 destination;
    public GameObject moveIndicator;

    // Use this for initialization
    void Start () {
        velocity = Vector3.zero;
    }

    private string Select = "Fire1";
    private string Move = "Fire2";

    void HitEnemy() {
        if (targetedEnemy != null) {
            // DO damage
        }
    }

    void AttackComplete() {
        lastAttack = Time.time;
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
            isMovingToEnemy = true;
        }
        else {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layer_mask = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out hit, 100, layer_mask)) {
                destination = hit.point;
            }
            isMovingToEnemy = false;
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

    // Attempt to attack
    void AttemptAttack() {
        float timeBetweenAttacks = 1.0f / attackSpeed;

        if (Time.time > lastAttack + timeBetweenAttacks) {
            GetComponent<Animator>().SetTrigger("fireballThrow");

            lastAttack = Time.time;
        }
    }

    void MoveToDestination() {
        Vector3 direction = destination - transform.position;
        direction.y = 0;

        // For moving towards enemies, maintain a distance between
        float magnitude = direction.magnitude;
        if (magnitude - (isMovingToEnemy ? attackRange : 0) < 0.5f) {
            GetComponent<Animator>().SetFloat("moveSpeed", 0);
            //transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            return;
        }

        if (direction.magnitude > maxSpeed) {
            direction.Normalize();
            direction *= maxSpeed;
        }

        float moveSpeed = direction.magnitude;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        GetComponent<Animator>().SetFloat("moveSpeed", moveSpeed);
        GetComponent<CharacterController>().SimpleMove(direction);

        // Normalize to the attack radius
        if (isMovingToEnemy) {
            direction = transform.position - destination;
            direction.y = 0;
            magnitude = direction.magnitude;
            if (magnitude <= attackRange) {
                direction.Normalize();

                GetComponent<CharacterController>().SimpleMove(destination + direction * attackRange);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetButtonDown(Select)) {
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

        MoveToDestination();

        Vector3 direction = destination - transform.position;
        if (isMovingToEnemy) {
            direction = transform.position - destination;
            direction.y = 0;
            float magnitude = direction.magnitude;
            if (magnitude <= attackRange + 0.5f) {
                AttemptAttack();
            }
        }
    }
}
