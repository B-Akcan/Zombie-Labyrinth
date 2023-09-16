using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;

public class BloodSprayPool : MonoBehaviour
{
    public static BloodSprayPool SharedInstance;
    [SerializeField] GameObject prefab;
    Queue<GameObject> effects;
    WaitForSeconds bloodSprayLifetime;
    IEnumerator coroutine;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        bloodSprayLifetime = new WaitForSeconds(bloodSprayDelay);

        effects = new Queue<GameObject>();
        GameObject tmp;
        for(int i = 0; i < bsp_amountToPool; i++)
        {
            tmp = Instantiate(prefab);
            tmp.transform.parent = transform;
            tmp.gameObject.SetActive(false);
            effects.Enqueue(tmp);
        }
    }

    public GameObject GetEffect()
    {
        return effects.Dequeue();
    }

    public void InstantiateEffect(GameObject effect, RaycastHit hit)
    {
        effect.transform.parent = hit.transform;
        effect.transform.position = hit.point;
        effect.transform.LookAt(PlayerController.SharedInstance.GetCamera());

        effect.SetActive(true);

        coroutine = DeactivateEffect(effect);
        StartCoroutine(coroutine);
    }

    public IEnumerator DeactivateEffect(GameObject effect)
    {
        yield return bloodSprayLifetime;

        effect.gameObject.SetActive(false);
        effect.transform.parent = transform;
        effects.Enqueue(effect);
    }
}
