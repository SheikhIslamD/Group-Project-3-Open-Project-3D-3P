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
    public float sightRange, fightRange;
    private bool inSightRange, inAttackRange;

    //Connections
    NavMeshAgent navAgent;

    //Data
    float attackTimer;

    protected override void Start()
    {
        base.Start();
        navAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        inSightRange = distanceFromPlayer < sightRange;
        inAttackRange = distanceFromPlayer < fightRange;

        if (!inSightRange && !inAttackRange) Idle();
        if (inSightRange) Chase();
        
    }

    void Idle()
    {

    }

    void Chase()
    {
        navAgent.SetDestination(player.transform.position);

        attackTimer += Time.deltaTime;

        if(attackTimer > attackRate)
        {
            attackTimer = 0;
            if (distanceFromPlayer < attackRange) Attack();
        }
    }

    void Attack()
    {
        player.GetComponent<Health>().Damage(attackDamage, Health.DamageType.Melee);
    }

}
