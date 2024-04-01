using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : EnemyBase
{
    //Config
    public LayerMask whatIsGround;
    public float timeBetweenAttacks;
    public float attackRange;
    
    //Connections
    public NavMeshAgent navAgent;
    ObjectPool pool;

    //Data
    float timeLeftBeforeAttack;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        pool = GetComponent<ObjectPool>();
        timeLeftBeforeAttack = timeBetweenAttacks;
    }
    
    private void Update()
    {
        if (distanceFromPlayer < attackRange) AttackPlayer();
        else ChasePlayer();
    }

    private void ChasePlayer()
    {
        navAgent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        navAgent.SetDestination(transform.position);
        transform.LookAt(player);

        if(timeLeftBeforeAttack > 0)
        {
            timeLeftBeforeAttack -= Time.deltaTime;
        }
        else
        {
            audioC.PlaySound("Attack");
            PoolableObject bullet = pool.Pump();
            bullet.Prepare_Basic(transform.position, Vector3.zero, transform.forward * 32f + transform.up * 2f);

            /*
            sphere.gameObject.SetActive(true);
            sphere.transform.position = transform.position;
            sphere.transform.eulerAngles = Vector3.zero;
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
             */

            timeLeftBeforeAttack = timeBetweenAttacks;
        }

    }
}

