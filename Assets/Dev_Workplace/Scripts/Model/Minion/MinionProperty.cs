using System;
using UnityEngine;

[Serializable]
public class MinionProperty {

    [Range(1,3)]
    public int level=1;
    
    public float range;
    
    public float viewRange; // 索敌范围，doc没写，感觉需要，先放这
    
    public float damage;

    [Header("额外5%写1.05，减0.5写-0.5")]
    public float specialEffectModifier;

    public float specialEffectLastTime;
}