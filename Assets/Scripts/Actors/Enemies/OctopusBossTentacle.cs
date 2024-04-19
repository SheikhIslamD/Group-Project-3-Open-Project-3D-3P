using System.Collections;
using UnityEngine;


public class OctopusBossTentacle : EnemyBase
{
    //Config
    [SerializeField] Health baseHealth;

    //Connections
    Health health;
    Animator anim;

    //Data
    public bool attacking;
    public bool dead;


    protected override void Awake()
    {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
    }

    public void BeginAttack(bool counterClockwise)
    {
        attacking = true;
        anim.SetBool("CounterClockwise", counterClockwise);
        anim.SetTrigger("Attack");
    }

    public void EndAttack()
    {
        attacking = false;
        anim.ResetTrigger("Attack");
    }

    public void OnHealthChange(Health.Interaction args)
    {
        if(args.type != Health.DamageType.Melee)
        {
            args.Interrupt();
            return;
        }

        args.customIdentifier = "TentacleDamaged";
        baseHealth.ChangeHealth(args);
        anim.SetTrigger("Parried");
    }

    protected override void OnHealthDeplete() => dead = true;
}
