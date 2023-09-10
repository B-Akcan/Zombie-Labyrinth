using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool SharedInstance;
    List<Enemy> enemies;
    [SerializeField] Enemy enemyPrefab;
    List<Vector3> spawnPoints = new List<Vector3> {new Vector3(23,0,-23),
                                                   new Vector3(-23,0,-23),
                                                   new Vector3(-23,0,23),
                                                   new Vector3(23,0,23)};
    Vector3 spawnPoint;
    double enemySpawnTimer;
    double timePassed; // Since last spawn
    int i;
    float z; // z coordinate of player
    double difficulty;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        enemies = new List<Enemy>();
        Enemy tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(enemyPrefab);
            tmp.transform.parent = transform;
            tmp.gameObject.SetActive(false);
            enemies.Add(tmp);
        }

        enemySpawnTimer = initialEnemySpawnTimer;
        difficulty = difficultyNormal;

        timePassed = 0f;
        i = 0;
    }

    void Update()
    {
        if (!PlayerStats.SharedInstance.PlayerIsDead())
            Spawn();
    }

    void Spawn()
    {
        if (enemies[i].gameObject.activeInHierarchy)
        {
            if (i+1 == amountToPool)
                i = 0;
            else
                i++;
        }
        
        if (Time.time >= timePassed)
        {
            timePassed = Time.time + enemySpawnTimer;

            z = PlayerStats.SharedInstance.gameObject.transform.position.z;
            if (z <= 0)
                spawnPoint = spawnPoints[Random.Range(0, 2)];
            else
                spawnPoint = spawnPoints[Random.Range(2, 4)];

            enemies[i].transform.position = spawnPoint;
            enemies[i].gameObject.SetActive(true);
        }
    }

    public Enemy GetEnemy(GameObject obj)
    {
        for (int i=0; i<amountToPool; i++)
        {
            if (enemies[i].gameObject == obj)
            {
                return enemies[i];
            }
        }

        return null;
    }

    public void AdjustEnemySpawnTimer()
    {
        if (PlayerStats.SharedInstance.GetScore() % scoreMultiplier == 0)
            enemySpawnTimer *= difficulty;
    }
}
