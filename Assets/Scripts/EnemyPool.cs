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
                                                   new Vector3(0, 0, 0),
                                                   new Vector3(0, 0, -23)};
    Vector3 spawnPoint;
    double enemySpawnTimer;
    double timePassed; // Since last spawn
    int i;
    [SerializeField] IntSO difficultySO;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        enemies = new List<Enemy>();
        Enemy tmp;
        for(int i = 0; i < ep_amountToPool; i++)
        {
            tmp = Instantiate(enemyPrefab);
            tmp.transform.parent = transform;
            tmp.gameObject.SetActive(false);
            enemies.Add(tmp);
        }

        i = 0;
        timePassed = initialSpawnTime;
        AssignSpawnTimer();
    }

    void Update()
    {
        if (!PlayerStats.SharedInstance.PlayerIsDead() && !PlayerController.SharedInstance.isGameStopped() && IsThereAnyEnemyToSpawn())
            Spawn();
    }

    void Spawn()
    {
        if (enemies[i].gameObject.activeInHierarchy)
        {
            if (i+1 == ep_amountToPool)
                i = 0;
            else
                i++;
        }
        
        if (Time.timeSinceLevelLoadAsDouble >= timePassed)
        {
            timePassed = Time.timeSinceLevelLoadAsDouble + enemySpawnTimer;

            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            enemies[i].transform.position = spawnPoint;
            enemies[i].gameObject.SetActive(true);
        }
    }

    public Enemy GetEnemy(GameObject obj)
    {
        for (int i=0; i<ep_amountToPool; i++)
        {
            if (enemies[i].gameObject == obj)
            {
                return enemies[i];
            }
        }

        return null;
    }

    void AssignSpawnTimer()
    {
        switch (difficultySO.Value)
        {
            case 0: enemySpawnTimer = spawnTimerEasy; break;
            case 1: enemySpawnTimer = spawnTimerMedium; break;
            case 2: enemySpawnTimer = spawnTimerHard; break;
        }
    }

    bool IsThereAnyEnemyToSpawn()
    {
        for (int i=0; i<ep_amountToPool; i++)
        {
            if (!enemies[i].gameObject.activeInHierarchy)
                return true;
        }

        return false;
    }
}
