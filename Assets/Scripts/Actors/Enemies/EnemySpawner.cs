using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : EnemyBase
{
    //Config
    [SerializeField] private float spawnRange = 10;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private int maxEnemies = 12;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private bool canSpawn = true;

    //Components
    ObjectPool[] objectPools;

    //Data
    float spawnTimer;

    protected override void Awake()
    {
        base.Awake();
        
        objectPools = new ObjectPool[enemyPrefabs.Length];
        for (int i = 0; i < objectPools.Length; i++) objectPools[i] = ObjectPool.Create(gameObject, enemyPrefabs[i], 1, deleteObjectsOnDestroy: false);

    }

    private void Update()
    {
        if (distanceFromPlayer > spawnRange) return;
        
        if(spawnTimer < spawnRate) spawnTimer = Time.deltaTime;
        if(spawnTimer >= spawnRate)
        {
            spawnTimer = 0;
            if (TotalEnemyCount() < maxEnemies)
                Spawn();
        }

    }

    /*

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (canSpawn)
        {
            yield return wait;
            int rand = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyToSpawn = enemyPrefabs[rand];

            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
    }
     */
    
    void Spawn()
    {
        int rand = Random.Range(0, enemyPrefabs.Length);
        var enemyToSpawn = objectPools[rand].Pump();
        enemyToSpawn.transform.position = transform.position - Vector3.forward;
    }

    int TotalEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < objectPools.Length; i++)
        {
            count += objectPools[i].ActiveObjects();
        }
        return count;
    }

}
