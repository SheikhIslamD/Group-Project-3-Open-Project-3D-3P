using System.Collections;
using UnityEngine;


public class BottomlessPit : MonoBehaviour
{
    PlayerMove player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    private void OnTriggerExit(Collider other) => player.StorePosition();
    private void OnTriggerEnter(Collider other) => player.StorePosition();
}
