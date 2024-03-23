using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Loot : MonoBehaviour
{
    public enum ItemType
    {
        Fish,
        Rice,
        Seaweed
    }
    public ItemType itemType;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerControls>() != null) { }
    }

}
