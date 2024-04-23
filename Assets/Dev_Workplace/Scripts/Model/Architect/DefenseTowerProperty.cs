using System;
using UnityEngine;

[Serializable]
public class DefenseTowerProperty : ArchitectProperty {

    public float damage;
    [Header("额外5%写1.05，减0.5写-0.5")]
    public float specialEffectModifier;

    public float specialEffectLastTime;

    public float secondSpEffectModifier;

    public float secondSpEffectLastTime;
}
