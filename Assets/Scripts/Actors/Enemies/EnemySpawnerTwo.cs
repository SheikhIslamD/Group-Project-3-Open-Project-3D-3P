using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerTwo : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyOnePrefab;
    [SerializeField]
    private GameObject enemyTwoPrefab;

    [SerializeField]
    private float enemyOneInterval = 3.5f;
    [SerializeField]
    private float enemyTwoInterval = 10f;

    void Start()
    {
        StartCoroutine(spawnEnemy(enemyOneInterval, enemyOnePrefab));
        StartCoroutine(spawnEnemy(enemyTwoInterval, enemyTwoPrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), 0, Random.Range(-5f, 5f)), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }

    void OnDeplete()
    {
        GetComponent<LootBag>().DropLoot(transform.position);
        Destroy(gameObject);
    }
}
