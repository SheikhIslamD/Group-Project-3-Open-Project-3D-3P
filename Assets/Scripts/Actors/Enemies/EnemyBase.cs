using UnityEngine;

[RequireComponent(typeof(AudioCaller))]
public class EnemyBase : MonoBehaviour
{

    //Connections
    protected PlayerMove player;
    protected new AudioCaller audio;

    //Data
    protected float distanceFromPlayer
    {
        get
        {
            if (player == null)
            {
                gameObject.SetActive(false);
                return 0f;
            }
            return Vector3.Distance(transform.position, player.position);
        }
    }

    protected virtual void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        audio = GetComponent<AudioCaller>();
    }

    protected virtual void OnHealthDeplete()
    {
        GetComponent<LootBag>()?.DropLoot();
    }
}
