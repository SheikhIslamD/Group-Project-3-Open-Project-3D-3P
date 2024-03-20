using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public GameObject lootModel;
    public string lootName;
    [Range(0, 100)]
    public float dropChance;

    public Loot(string lootName, float dropChance)
    {
        this.lootName = lootName;
        this.dropChance = Mathf.Clamp(dropChance, 0, 100);
    }
}
