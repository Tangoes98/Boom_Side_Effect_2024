using System;
using UnityEngine;

[Serializable]
public abstract class ArchitectProperty {
    
    [Range(1,3)]
    public int level;
    [Header("升级费用，1级即为建造费用。只有基础建筑要填")]
    public int cost;

    public float range;
    
}
