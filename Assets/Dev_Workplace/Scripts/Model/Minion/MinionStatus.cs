using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MinionStatus {
    [Header("初始化/升级时'Status'会被'Base Info'覆盖")]
    [Space(10)]
    
    [Inspectable]
    public float range;
    [Inspectable]
    public float viewRange;
    [Inspectable]
    public bool isMelee;
    [Inspectable]
    public float damage;
    [Inspectable]
    public DamageType damageType;
    [Inspectable]
    public float health;
    [Inspectable]
    public float speed;
    [Inspectable]
    public float attackSpeed;
    
}