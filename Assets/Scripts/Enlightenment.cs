using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlightenment : MonoBehaviour
{
    Light lightComp;
    [SerializeField] DoubleSO brightnessSO;

    void Awake()
    {
        lightComp = GetComponent<Light>();
    }

    void Start()
    {
        lightComp.intensity = (float) brightnessSO.Value;
    }
}
