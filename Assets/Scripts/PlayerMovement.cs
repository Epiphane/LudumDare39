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

    // Enemy targeting
    private EnemyBase targetedEnemy = null;
    private GameObject eIndicator = null;
    private bool isMovingToEnemy;

    // Movement
    private Vector3 destination;
    public GameObject moveIndicator;

    // Direction stuff
    private DirectionSmoother director;

    // Use this for initialization
    void Start () {
        director = GetComponent<DirectionSmoother>();
    }

    private string Select = "Fire1";
    private string Move = "Fire2";

    void HitEnemy() {
        if (targetedEnemy != null) {
            // DO damage
        }
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
                if (offFromEnemy.magnitude < 2.0f) {
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
        PlayerSpells spells = GetComponent<PlayerSpells>();
        BasicAttack attack = GetComponent<BasicAttack>();
        if (spells != null && !spells.isCasting) {
            attack.targetedEnemy = targetedEnemy;
            spells.BasicAttack();
        }
    }

    void MoveToDestination() {
        PlayerSpells spells = GetComponent<PlayerSpells>();
        if (spells != null && spells.isCasting) {
            GetComponent<Animator>().SetFloat("moveSpeed", 0);
            return;
        }

        Vector3 direction = destination - transform.position;
        direction.y = 0;

        // For moving towards enemies, maintain a distance between
        float magnitude = direction.magnitude;
        if (magnitude - attackRange < 0.5f) {
            if (isMovingToEnemy) {
                director.IWantToFace(direction);
            }
            GetComponent<Animator>().SetFloat("moveSpeed", 0);
            return;
        }

        if (direction.magnitude > maxSpeed) {
            direction.Normalize();
            direction *= maxSpeed;
        }

        float moveSpeed = direction.magnitude;
        director.IWantToFace(direction);
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

        if (Input.GetKey(KeyCode.Space) && eIndicator != null) {
            // GO back to attacking
            destination = eIndicator.transform.position;
            isMovingToEnemy = true;
        }

        PlayerSpells spells = GetComponent<PlayerSpells>();
        if (Input.GetButtonDown(Select) && (spells == null || !spells.isIndicating)) {
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
