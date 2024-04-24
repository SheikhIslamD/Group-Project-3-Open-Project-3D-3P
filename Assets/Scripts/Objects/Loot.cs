using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Loot : MonoBehaviour
{

    public enum ItemType
    {
        Rice,
        Fish,
        Seaweed
    }
    public ItemType itemType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCooking>() == null) return;
        GetComponent<AudioCaller>().PlaySound("Pickup");
        other.GetComponent<PlayerCooking>().AddIngredient((int)itemType);
        Destroy(gameObject);
    }

}
