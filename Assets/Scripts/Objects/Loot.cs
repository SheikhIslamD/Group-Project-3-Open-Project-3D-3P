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
    bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCooking>() == null || collected) return;
        other.GetComponent<PlayerCooking>().AddIngredient((int)itemType);
        collected = true;
        Destroy(gameObject);
    }

}
