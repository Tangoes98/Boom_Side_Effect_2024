
using System;
using UnityEngine;

[Serializable]
public class Wave {
     [Header("第几秒召唤多少敌人")]
    public SpawnEnemy[] timeline; 
    [Space(10)]
    [Header("离下一关间隔")]
    public float interval;
}