using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerTwo : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyOne;
    [SerializeField]
    private GameObject enemyTwo;
    [SerializeField]
    private GameObject enemyThree;

    [SerializeField]
    private float enemyOneInterval = 3f;
    [SerializeField]
    private float enemyTwoInterval = 5f;
    [SerializeField]
    private float enemyThreeInterval = 7f;

    void Start()
    {
        StartCoroutine(spawnEnemy(enemyOneInterval, enemyOne));
        StartCoroutine(spawnEnemy(enemyTwoInterval, enemyTwo));
        StartCoroutine(spawnEnemy(enemyThreeInterval, enemyTwo));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), 0, Random.Range(-5f, 5f)), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }

    void OnHealthChange(Health.Interaction args)
    {
        if (args.depletes)
        {
            GetComponent<LootBag>().DropLoot();
            Destroy(gameObject);
        }

    }
}
