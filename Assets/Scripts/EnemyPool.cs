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
                                                   new Vector3(23,0,23),
                                                   new Vector3(23, 0, 0),
                                                   new Vector3(-23, 0, 0),
                                                   new Vector3(0, 0, 23),
                                                   new Vector3(0, 0, 0)};
    Vector3 spawnPoint;
    double enemySpawnTimer;
    double timePassed; // Since last spawn
    int i;
    double difficulty;
    [SerializeField] IntSO difficultySO;

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
        
        switch (difficultySO.Value)
        {
            case 0: difficulty = difficultyEasy; break;
            case 1: difficulty = difficultyMedium; break;
            case 2: difficulty = difficultyHard; break;
        }

        timePassed = 0f;
        i = 0;
    }

    void Update()
    {
        if (!PlayerStats.SharedInstance.PlayerIsDead() && !PlayerController.SharedInstance.isGameStopped())
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

            spawnPoint = spawnPoints[Random.Range(0, 8)];
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
