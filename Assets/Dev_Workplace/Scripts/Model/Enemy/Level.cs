using System;
using UnityEngine;

[Serializable]
public class Level {
    [Header("填入炸开新路口的trigger，空则不炸")]
    public string boomTrigger="";
    public Wave[] waves;

}