using UnityEngine;

public class BasicAttack : Ability {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        /*float timeBetweenAttacks = 1.0f / attackSpeed;

        if (Time.time > lastAttack + timeBetweenAttacks) {
            GetComponent<Animator>().SetTrigger("basicAttack");

            lastAttack = Time.time;
            isAttacking = true;
        }*/
    }

    [HideInInspector]
    public UnitWithHealth targetedEnemy;

    public override void DoCast(Vector3 mouse) {
        // Animate the attack
        Animator anim = GetComponent<Animator>();
        if (anim != null) {
            anim.SetTrigger("basicAttack");
        }

        Vector3 castDirection = targetedEnemy.transform.position - transform.position;

        GetComponent<DirectionSmoother>().IWantToFace(castDirection);
    }

    void BasicAttackComplete() {
        targetedEnemy.TakeDamage(30);

        GetComponent<UnitSpells>().DoneCasting();
    }
}
