using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;

public class BulletImpactPool : MonoBehaviour
{
    public static BulletImpactPool SharedInstance;
    [SerializeField] GameObject prefab;
    Queue<GameObject> effects;
    WaitForSeconds bulletImpactLifetime;
    IEnumerator coroutine;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        bulletImpactLifetime = new WaitForSeconds(bulletImpactDelay);

        effects = new Queue<GameObject>();
        GameObject tmp;
        for(int i = 0; i < bip_amountToPool; i++)
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
        effect.transform.position = hit.point;

        if (hit.transform.gameObject.tag.Equals(ENVIRONMENT))
            effect.transform.LookAt(PlayerController.SharedInstance.GetCamera());

        else if (hit.transform.gameObject.tag.Equals(BOTTOM))
            effect.transform.rotation = Quaternion.Euler(-90, 0, 0);

        effect.SetActive(true);

        coroutine = DeactivateEffect(effect);
        StartCoroutine(coroutine);
    }

    public IEnumerator DeactivateEffect(GameObject effect)
    {
        yield return bulletImpactLifetime;

        effect.gameObject.SetActive(false);
        effects.Enqueue(effect);
    }
}
