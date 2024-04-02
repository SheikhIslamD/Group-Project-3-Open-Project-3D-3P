using System.Collections;
using UnityEngine;


public class EnemyBase : MonoBehaviour
{

    //Connections
    protected new Transform transform;
    protected Transform player;
    protected AudioCaller audioC;

    //Data
    protected float distanceFromPlayer => Vector3.Distance(transform.position, player.position);

    protected virtual void Start()
    {
        transform = GetComponent<Transform>();
        player = GameObject.Find("Player").transform;
        audioC = GetComponent<AudioCaller>();
    }

    protected virtual void OnDeplete()
    {
        audioC.PlaySound("Death");
        GetComponent<LootBag>()?.DropLoot(transform.position);
        PoolableObject pooled = GetComponent<PoolableObject>();
        if (pooled != null) pooled.Disable();
        else Destroy(gameObject);
    }
}
