using UnityEngine;

public class BasicAttack : Ability {

    private UnitMovement myMovement;
    
    void Start () {
        myMovement = GetComponent<UnitMovement>();
    }

    [HideInInspector]
    public UnitWithHealth targetedEnemy;

    public override void DoCast(Vector3 mouse) {
        // Animate the attack
        myMovement.animator.SetTrigger("basicAttack");

        Vector3 castDirection = targetedEnemy.transform.position - transform.position;

        myMovement.director.IWantToFace(castDirection);
    }

    public override void Execute() {
        caster.DealDamage(targetedEnemy, new Stats(0, 1, 0, 0));

        GetComponent<UnitSpells>().DoneCasting();
    }
}
