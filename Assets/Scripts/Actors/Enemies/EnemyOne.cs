using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : MonoBehaviour
{
    AudioCaller audioC;
    NavMeshAgent enemy;
    GameObject player;

    void Start()
    {
        enemy=GetComponent<NavMeshAgent>();
        player=GameObject.FindWithTag("Player");
        audioC=GetComponent<AudioCaller>();
    }
    void Update()
    {
        enemy.SetDestination(player.transform.position);
    }
    
    void OnDeplete()
    {
        audioC.PlaySound("Death");
        GetComponent<LootBag>().DropLoot(transform.position);
        Destroy(gameObject);
    }
}
