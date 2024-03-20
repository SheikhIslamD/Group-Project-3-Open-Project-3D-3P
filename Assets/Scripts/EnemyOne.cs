using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : MonoBehaviour
{
    NavMeshAgent enemy;
    GameObject player;

    void Start()
    {
        enemy=GetComponent<NavMeshAgent>();
        player=GameObject.FindWithTag("Player");
    }
    void Update()
    {
        enemy.SetDestination(player.transform.position);
    }
    
    void OnHealthDeplete()
    {
        Destroy(gameObject);
    }
}
