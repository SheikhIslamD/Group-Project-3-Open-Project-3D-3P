using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    [SerializeField] List<LootEntry> lootList = new List<LootEntry>();
    [SerializeField] int maxDrops;
    [SerializeField] bool dropMultiple;

    [System.Serializable]
    public struct LootEntry
    {
        public Loot loot;
        [Range(0, 100)]
        public float dropChance;
    }

    Loot GetDroppedItem()
    {
        //Randomly chooses one item out of all items whose drop chance is below a single Random 0-100 roll.

        float randomNumber = Random.Range(0f, 100f);
        List<Loot> possibleItems = new List<Loot>();
        foreach (LootEntry item in lootList)
        {
            if(randomNumber <= item.dropChance) possibleItems.Add(item.loot);
        }
        if(possibleItems.Count > 0)
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        Debug.Log("No loot dropped");
        return null;
    }    
    List<Loot> GetDroppedItems()
    {
        //Runs a Random 0-100 roll for every possible item, removes possible items until maxItems is reached, and drops them all.

        List<Loot> possibleItems = new List<Loot>();
        foreach (LootEntry item in lootList)
        {
            float randomNumber = Random.Range(0f, 100f);
            if (randomNumber <= item.dropChance) possibleItems.Add(item.loot);
        }
        if(possibleItems.Count > 0)
        {
            while (possibleItems.Count > maxDrops) possibleItems.RemoveAt(Random.Range(0, possibleItems.Count));

            return possibleItems;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    public void DropLoot(Vector3 spawnPosition)
    {
        if (!dropMultiple)
        {
            List<Loot> droppedItems = GetDroppedItems();
            if (droppedItems.Count > 0)
                foreach (Loot item in droppedItems)
                    InstantiateLoot(item, spawnPosition);

        }
        else InstantiateLoot(GetDroppedItem(), spawnPosition);
    }

    public void InstantiateLoot(Loot loot, Vector3 spawnPosition)
    {
        if(loot == null) return;
        GameObject lootGameObject = Instantiate(loot.gameObject, spawnPosition, Quaternion.identity);
    }
}
