using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool SharedInstance;
    public Queue<GameObject> enemies;
    [SerializeField] GameObject enemyPrefab;
    public int amountToPool=100;
    GameObject enemy;
    List<Vector3> spawnPoints = new List<Vector3> {new Vector3(23,0,-23),
                                                   new Vector3(-23,0,-23),
                                                   new Vector3(-23,0,23),
                                                   new Vector3(23,0,23)};
    Vector3 spawnPoint;
    float enemySpawnTimer;
    float timePassed; // Since last spawn

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        enemies = new Queue<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(enemyPrefab);
            tmp.transform.parent = transform;
            tmp.SetActive(false);
            enemies.Enqueue(tmp);
        }

        timePassed = 0f;
        enemySpawnTimer = 5f;
    }

    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        if (Time.time >= timePassed)
        {
            enemy = enemies.Dequeue();
            spawnPoint = spawnPoints[Random.Range(0, 4)];
            enemy.transform.position = spawnPoint;
            enemy.SetActive(true);

            timePassed = Time.time + enemySpawnTimer;
        }
    }

    public void Die(GameObject enemy)
    {
        enemy.SetActive(false);
        enemies.Enqueue(enemy);
    }
}
