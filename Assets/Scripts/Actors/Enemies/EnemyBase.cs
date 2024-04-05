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

    protected virtual void OnHealthChange(Health.Interaction args)
    {

        if (args.depletes)
        {
            audio.PlaySound("Death");
            GetComponent<LootBag>()?.DropLoot(transform.position);
            PoolableObject pooled = GetComponent<PoolableObject>();
            if (pooled != null)
            {
                pooled.Disable();
                gameObject.SetActive(false);
            }
            else Destroy(gameObject);
        }
    }
}
