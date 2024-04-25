using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    [SerializeField] private List<LootEntry> lootList = new();
    [SerializeField] private int maxDrops;
    [SerializeField] private bool dropMultiple;
    [SerializeField] private bool autoDropOnDisable;

    [System.Serializable]
    public struct LootEntry
    {
        public Loot loot;
        [Range(0, 100)]
        public float dropChance;
    }

    private Loot GetDroppedItem()
    {
        //Randomly chooses one item out of all items whose drop chance is below a single Random 0-100 roll.

        float randomNumber = Random.Range(0f, 100f);
        List<Loot> possibleItems = new();
        foreach (LootEntry item in lootList)
        {
            if (randomNumber <= item.dropChance) possibleItems.Add(item.loot);
        }
        if (possibleItems.Count > 0)
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    private List<Loot> GetDroppedItems()
    {
        //Runs a Random 0-100 roll for every possible item, removes possible items until maxItems is reached, and drops them all.

        List<Loot> possibleItems = new();
        foreach (LootEntry item in lootList)
        {
            float randomNumber = Random.Range(0f, 100f);
            if (randomNumber <= item.dropChance) possibleItems.Add(item.loot);
        }
        if (possibleItems.Count > 0)
        {
            while (possibleItems.Count > maxDrops) possibleItems.RemoveAt(Random.Range(0, possibleItems.Count));

            return possibleItems;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    public void DropLoot()
    {
        if (dropMultiple)
        {
            List<Loot> droppedItems = GetDroppedItems();
            if (droppedItems.Count > 0)
                foreach (Loot item in droppedItems)
                    InstantiateLoot(item, transform.position);

        }
        else InstantiateLoot(GetDroppedItem(), transform.position);
    }

    public void InstantiateLoot(Loot loot, Vector3 spawnPosition)
    {
        if (!loot) return;
        GameObject lootGameObject = Instantiate(
            loot.gameObject,
            spawnPosition + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)),
            Quaternion.identity);
    }

    private void OnDisable()
    {
        if(autoDropOnDisable)DropLoot();
    }
}
