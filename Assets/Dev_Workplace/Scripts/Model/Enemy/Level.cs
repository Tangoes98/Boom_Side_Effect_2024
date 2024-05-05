using System;
using UnityEngine;

[Serializable]
public class Level {
    [Header("填入炸开新路口的index:2,3,4")]
    public int[] boomEntrances;
    public Wave[] waves;

}