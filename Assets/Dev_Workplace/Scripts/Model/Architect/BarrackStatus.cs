using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BarrackStatus : ArchitectStatus {
    public List<Minion> currentMinions; 
    [Range(1,4)]
    public int maxMinionNum;
    public float manufactureInterval;

    public float manufactureTime;
    public GameObject minionPrefab;

}