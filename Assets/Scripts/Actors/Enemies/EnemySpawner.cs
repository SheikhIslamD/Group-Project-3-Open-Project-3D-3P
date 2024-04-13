using UnityEngine;

public class EnemySpawner : EnemyBase
{
    //Config
    [SerializeField] private float spawnRange = 10;
    [SerializeField] private float spawnRate = 5f;
    [SerializeField] private int maxEnemies = 12;
    //[SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private bool canSpawn = true;

    //Components
    private ObjectPool[] objectPools;

    //Data
    private float spawnTimer;

    protected override void Awake()
    {
        base.Awake();

        objectPools = GetComponents<ObjectPool>();
        /*
        objectPools = new ObjectPool[enemyPrefabs.Length];
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject baseObj = Instantiate(enemyPrefabs[i]);
            baseObj.transform.parent = transform;
            baseObj.SetActive(false);
            objectPools[i] = ObjectPool.Create(gameObject, baseObj, 2, deleteObjectsOnDestroy: false);
        }
         */
        if(objectPools.Length<1) gameObject.SetActive(false);
    }

    private void Update()
    {
        if (distanceFromPlayer > spawnRange) return;

        if (spawnTimer < spawnRate) spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            spawnTimer = 0;
            if (TotalEnemyCount() < maxEnemies) Spawn();
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

    private void Spawn()
    {
        int rand = Random.Range(0, objectPools.Length);
        PoolableObject enemyToSpawn = objectPools[rand].Pump();
        enemyToSpawn.transform.position = transform.position - Vector3.forward;
    }

    private int TotalEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < objectPools.Length; i++)
        {
            count += objectPools[i].ActiveObjects();
        }
        return count;
    }

}
