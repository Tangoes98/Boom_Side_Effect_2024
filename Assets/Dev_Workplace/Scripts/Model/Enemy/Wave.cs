
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Wave {
    [Header("第几秒召唤多少敌人")]
    public SpawnEnemy[] timeline; 
    [Space(10)]
    [Header("离下一wave间隔")]
    public float interval;
    public float totalFightTime => timeline.Last().timePoint;
}