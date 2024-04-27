using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : EnemyBase
{
    //Config
    [SerializeField] float attackRange;
    [SerializeField] float attackRate;
    [SerializeField] int attackDamage;
    public float sightRange;
    private bool inSightRange, inAttackRange;

    //Connections
    NavMeshAgent navAgent;
    Animator anim;

    //Data
    float attackTimer;

    protected override void Awake()
    {
        base.Awake();
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (player == null) return;

        inSightRange = distanceFromPlayer < sightRange;
        inAttackRange = distanceFromPlayer < attackRange;

        if (!inSightRange && !inAttackRange) Idle();
        if (inSightRange) Chase();

        anim.SetFloat("Speed", navAgent.velocity.magnitude);

    }

    void Idle()
    {

    }

    void Chase()
    {
        navAgent.SetDestination(player.position);

        attackTimer += Time.deltaTime;

        if(attackTimer > attackRate)
        {
            attackTimer = 0;
            if (distanceFromPlayer < attackRange) anim.SetTrigger("Attack");
        }
    }

    public void Attack()
    {
        if (distanceFromPlayer < attackRange) player.GetComponent<Health>()?.Damage(attackDamage, Health.DamageType.Melee, this);
        audio.PlaySound("Attack");
    }

}
