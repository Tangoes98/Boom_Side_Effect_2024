using System;
using UnityEngine;

[Serializable]
public class MinionStatus {    
    public float health;
    public float maxHealth;
    public float speed; 

    public float range;
    
    public float viewRange; // 索敌范围，doc没写，感觉需要，先放这

    public AttackMode attackMode;

    public LockMode lockMode;

    public float fireInterval;

    public float fireTime; // 每发攻击的时间
    
    public float damage;

    public SpecialEffect specialEffect;

    [Header("额外5%写1.05，减0.5写-0.5")]
    public float specialEffectModifier;

    public float specialEffectLastTime;
    
}