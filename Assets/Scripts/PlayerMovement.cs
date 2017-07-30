using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    public float speed = 15.0f;
    public float accel = 1.0f;
    public float maxSpeed = 15.0f;

    public float attackRange {
        get {
            if (!isMovingToEnemy) {
                return 0;
            }
            return radius + targetedEnemy.radius;
        }
    }
    public float radius = 0.5f;
    public float attackSpeed = 1.0f;
    private float lastAttack = 0;
    private bool isAttacking = false;

    // Enemy targeting
    private EnemyBase targetedEnemy = null;
    private GameObject eIndicator = null;
    private bool isMovingToEnemy;

    // Movement
    private Vector3 destination;
    public GameObject moveIndicator;

    // Use this for initialization
    void Start () {
    }

    private string Select = "Fire1";
    private string Move = "Fire2";

    void HitEnemy() {
        if (targetedEnemy != null) {
            // DO damage
        }
    }

    void BasicAttackComplete() {
        targetedEnemy.TakeDamage(30);
        isAttacking = false;
    }

    GameObject CreateMoveIndicator(Vector3 position, bool isPersistent = false, float scale = 1.0f) {
        GameObject indicator = GameObject.Instantiate(moveIndicator);
        indicator.transform.position = position;
        indicator.transform.localScale = new Vector3(scale, scale, 1);
        indicator.GetComponent<ShrinkToDeath>().enabled = !isPersistent;

        return indicator;
    }

    /* Returns true if an enemy got targeted */
    bool IssueMove(bool isDrag = false) {
        bool didSelectSomething = isDrag ? false : AttemptSelect();

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
            if (eIndicator != null) {
                Vector3 offFromEnemy = eIndicator.transform.position - destination;
                if (offFromEnemy.magnitude < 1.0f) {
                    destination = eIndicator.transform.position;
                    isMovingToEnemy = true;
                }
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
                eIndicator = CreateMoveIndicator(enemyPosition, true, targetedEnemy.radius + 1.0f);

                // Did we already have a target?
                if (isMovingToEnemy) {
                    destination = eIndicator.transform.position;
                }

                return true;
            }
        }

        return false;
    }

    // Attempt to attack
    void AttemptAttack() {
        if (isAttacking) {
            return;
        }

        float timeBetweenAttacks = 1.0f / attackSpeed;

        if (Time.time > lastAttack + timeBetweenAttacks) {
            GetComponent<Animator>().SetTrigger("basicAttack");

            lastAttack = Time.time;
            isAttacking = true;
        }
    }

    void MoveToDestination() {
        PlayerSpells spells = GetComponent<PlayerSpells>();
        if (isAttacking || (spells != null && spells.isCasting)) {
            return;
        }

        Vector3 direction = destination - transform.position;
        direction.y = 0;

        // For moving towards enemies, maintain a distance between
        float magnitude = direction.magnitude;
        if (magnitude - attackRange < 0.5f) {
            if (isMovingToEnemy) {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
            GetComponent<Animator>().SetFloat("moveSpeed", 0);
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
    }
    
    void FixedUpdate () {
        if (targetedEnemy != null) {
            if (targetedEnemy.isDead) {
                targetedEnemy = null;
                GameObject.Destroy(eIndicator);
                eIndicator = null;
                isMovingToEnemy = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && eIndicator != null) {
            // GO back to attacking
            destination = eIndicator.transform.position;
            isMovingToEnemy = true;
        }

        PlayerSpells spells = GetComponent<PlayerSpells>();
        if (Input.GetButtonDown(Select) && (spells == null || !spells.isCasting)) {
            AttemptSelect();
        }

        if (Input.GetButtonDown(Move)) {
            IssueMove();
        }
        if (Input.GetButtonUp(Move)) {
            // Click
            IssueMove();

            if (!isMovingToEnemy) {
                // Create an indicator of where you're moving
                CreateMoveIndicator(destination + Vector3.up * 0.1f);
            }
        }
        if (Input.GetButton(Move)) {
            // Drag
            IssueMove(true);
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
