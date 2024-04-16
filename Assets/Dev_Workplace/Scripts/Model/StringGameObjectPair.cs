
using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class StringGameObjectPair {
    [Inspectable]
    public string code;
    [Inspectable]
    public GameObject gameObject;
}