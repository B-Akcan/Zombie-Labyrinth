using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPool : MonoBehaviour
{
    public static LootPool SharedInstance;
    [SerializeField] GameObject healthPrefab;
    [SerializeField] GameObject damagePrefab;
    [SerializeField] GameObject speedPrefab;
    List<GameObject> loots;
    int dropChance;
    Vector3 positionAdjuster = new Vector3(0, 1, 0);
    AudioSource audioSource;
    
    void Awake()
    {
        SharedInstance = this;

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        loots = new List<GameObject>();
        GameObject tmp;

        tmp = Instantiate(healthPrefab);
        tmp.transform.parent = transform;
        tmp.gameObject.SetActive(false);
        loots.Add(tmp);

        tmp = Instantiate(damagePrefab);
        tmp.transform.parent = transform;
        tmp.gameObject.SetActive(false);
        loots.Add(tmp);

        tmp = Instantiate(speedPrefab);
        tmp.transform.parent = transform;
        tmp.gameObject.SetActive(false);
        loots.Add(tmp);
    }

    public void DropLoot(Vector3 position)
    {
        dropChance = Random.Range(0,100);

        if (dropChance < 10 && !loots[0].activeInHierarchy) // drop health loot
        {
            loots[0].transform.position = position + positionAdjuster;
            loots[0].SetActive(true);
        }

        else if (dropChance >= 10 && dropChance < 20 && !loots[1].activeInHierarchy) // drop damage loot
        {
            loots[1].transform.position = position + positionAdjuster;
            loots[1].SetActive(true);
        }

        else if (dropChance >= 20 && dropChance < 30 && !loots[2].activeInHierarchy) // drop speed loot, else drop nothing
        {
            loots[2].transform.position = position + positionAdjuster;
            loots[2].SetActive(true);
        }
    }

    public void PlaySoundEffect()
    {
        audioSource.Play();
    }
}
