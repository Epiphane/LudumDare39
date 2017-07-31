using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : UnitMovement {

    // Enemy targeting
    private GameObject eIndicator = null;
    
    // Movement
    public GameObject moveIndicator;

    private string Select = "Fire1";
    private string Move = "Fire2";

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
            FocusTarget();
        }
        else {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layer_mask = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out hit, 100, layer_mask)) {
                destination = hit.point;
            }

            _isMovingToTarget = false;
            if (eIndicator != null) {
                Vector3 offFromEnemy = eIndicator.transform.position - destination;
                if (offFromEnemy.magnitude < 2.0f) {
                    destination = eIndicator.transform.position;
                    _isMovingToTarget = true;
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
            if (Target(hit.collider.gameObject, isMovingToTarget)) {
                Vector3 enemyPosition = target.transform.position;
                enemyPosition.y = 0.45f;

                // Remove the last indicator
                if (eIndicator != null) {
                    GameObject.Destroy(eIndicator);
                }

                // Create an indicator under the enemy
                eIndicator = CreateMoveIndicator(enemyPosition, true, target.radius + 1.0f);

                return true;
            }
        }

        return false;
    }
    
    new protected void FixedUpdate () {
        if (target == null) {
            GameObject.Destroy(eIndicator);
            eIndicator = null;
        }
        else {
            Vector3 enemyPosition = target.transform.position;
            enemyPosition.y = 0.45f;

            eIndicator.transform.position = enemyPosition;
        }

        if (Input.GetKey(KeyCode.Space) && target != null) {
            // GO back to attacking
            FocusTarget();
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

            if (!isMovingToTarget) {
                // Create an indicator of where you're moving
                CreateMoveIndicator(destination + Vector3.up * 0.1f);
            }
        }
        if (Input.GetButton(Move)) {
            // Drag
            IssueMove(true);
        }

        base.FixedUpdate();
    }
}
