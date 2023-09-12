using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlightenment : MonoBehaviour
{
    public static Enlightenment SharedInstance;
    Light lightComp;
    [SerializeField] DoubleSO brightnessSO;

    void Awake()
    {
        SharedInstance = this;

        lightComp = GetComponent<Light>();
    }

    void Start()
    {
        lightComp.intensity = (float) brightnessSO.Value;
    }
}
