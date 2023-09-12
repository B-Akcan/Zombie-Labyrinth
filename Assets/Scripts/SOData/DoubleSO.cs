using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DoubleSO : ScriptableObject
{
    [SerializeField] double _value;

    public double Value
    {
        get { return _value; }
        set { _value = value; }
    }
}
