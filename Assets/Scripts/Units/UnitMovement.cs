using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour {

    public float speed = 10.0f;
    public float accel = 4.0f;
    public float maxSpeed = 2.0f;

    public float attackRange {
        get {
            if (!isMovingToTarget) {
                return 0;
            }
            return radius + target.radius;
        }
    }
    private float radius = 0.5f;

    // Enemy targeting
    public bool isMovingToTarget { get { return _isMovingToTarget; } }
    protected UnitWithHealth target = null;
    protected bool _isMovingToTarget;

    // Movement
    protected Vector3 destination;

    // Direction stuff
    public DirectionSmoother director;

    // Animator
    public Animator animator;

    // Character controller
    private CharacterController charController;
    private UnitWithHealth myInfo;

    // Use this for initialization
    void Start () {
        // Try and default to the director attached to this GameObject
        if (director == null) {
            director = GetComponent<DirectionSmoother>();
            if (director == null) {
                director = gameObject.AddComponent<DirectionSmoother>();
            }
        }

        UnitWithHealth healthComponent = GetComponent<UnitWithHealth>();
        if (healthComponent != null) {
            radius = healthComponent.radius;
        }

        // Try and default to the animator attached to this GameObject
        if (animator == null) {
            animator = GetComponent<Animator>();

            if (animator == null) {
                animator = GetComponentInChildren<Animator>();
            }
        }

        charController = GetComponent<CharacterController>();

        destination = transform.position;

        myInfo = GetComponent<UnitWithHealth>();
        myInfo.animator = animator;

        if (animator != null) {
            // Make sure there's an AbilityAnimationReceiver
            if (animator.GetComponent<AbilityAnimationReceiver>() == null) {
                AbilityAnimationReceiver receiver = animator.gameObject.AddComponent<AbilityAnimationReceiver>();

                receiver.myUnit = myInfo;
            }
        }
    }

    public void MoveTo(Vector3 dest) {
        _isMovingToTarget = false;
        destination = dest;
    }

    // Returns true on success
    public bool Target(GameObject obj, bool moveTo) {
        target = obj.GetComponent<UnitWithHealth>();
        if (moveTo && target != null) {
            destination = target.transform.position;
            _isMovingToTarget = true;
        }

        return target != null;
    }

    public void FocusTarget() {
        destination = target.transform.position;
        _isMovingToTarget = true;
    }

    // Attempt to attack
    void AttemptAttack() {
        UnitSpells spells = GetComponent<UnitSpells>();
        BasicAttack attack = GetComponent<BasicAttack>();
        if (spells == null || attack == null)
            return;

        if (!spells.isCasting) {
            attack.targetedEnemy = target;
            spells.BasicAttack();
        }
    }

    void MoveToDestination() {
        UnitSpells spells = GetComponent<UnitSpells>();
        if (spells != null && spells.isCasting) {
            animator.SetFloat("moveSpeed", 0);
            return;
        }

        Vector3 direction = destination - transform.position;
        direction.y = 0;

        // For moving towards enemies, maintain a distance between
        float magnitude = direction.magnitude;
        if (magnitude - attackRange < 0.5f) {
            if (isMovingToTarget) {
                director.IWantToFace(direction);
            }
            animator.SetFloat("moveSpeed", 0);
            return;
        }

        if (direction.magnitude > maxSpeed) {
            direction.Normalize();
            direction *= maxSpeed;
        }

        float moveSpeed = direction.magnitude;
        director.IWantToFace(direction);
        animator.SetFloat("moveSpeed", moveSpeed);
        charController.SimpleMove(direction);
    }
    
    protected void FixedUpdate () {
        if (myInfo != null && myInfo.isDead) {
            return;
        }

        if (target == null || target.isDead) {
            target = null;
            if (_isMovingToTarget) {
                // Stop moving to destination
                destination = transform.position;
            }
            _isMovingToTarget = false;
        }

        MoveToDestination();

        if (isMovingToTarget) {
            FocusTarget();
            Vector3 direction = transform.position - destination;
            direction.y = 0;
            if (direction.magnitude <= attackRange + 0.5f) {
                AttemptAttack();
            }
        }
    }
}
