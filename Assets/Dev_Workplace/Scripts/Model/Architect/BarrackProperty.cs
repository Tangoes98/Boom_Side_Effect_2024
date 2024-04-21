using System;
using UnityEngine;

[Serializable]
public class BarrackProperty : ArchitectProperty {
    [Range(1,4)]
    public int maxMinionNum;

}
