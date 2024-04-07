using UnityEngine;

[RequireComponent(typeof(AudioCaller))]
public class EnemyBase : MonoBehaviour
{

    //Connections
    protected new Transform transform;
    protected Transform player;
    protected new AudioCaller audio;

    //Data
    protected float distanceFromPlayer => Vector3.Distance(transform.position, player.position);

    protected virtual void Start()
    {
        transform = GetComponent<Transform>();
        player = GameObject.Find("Player").transform;
        audio = GetComponent<AudioCaller>();
    }

    protected virtual void OnHealthDeplete()
    {
        GetComponent<LootBag>()?.DropLoot(transform.position);
    }
}
