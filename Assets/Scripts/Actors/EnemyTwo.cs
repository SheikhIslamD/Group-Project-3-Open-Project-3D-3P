using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTwo : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float attackRange;
    public bool playerInAttackRange;
    ObjectPool pool;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        pool = GetComponent<ObjectPool>();
    }
    
    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInAttackRange) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();
    }

    private void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        enemy.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            PoolableObject sphere = pool.Pump();
            sphere.gameObject.SetActive(true);
            sphere.transform.position = transform.position;
            sphere.transform.eulerAngles = Vector3.zero;
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void OnDeplete()
    {
        GetComponent<LootBag>().DropLoot(transform.position);
        Destroy(gameObject);
    }
}

