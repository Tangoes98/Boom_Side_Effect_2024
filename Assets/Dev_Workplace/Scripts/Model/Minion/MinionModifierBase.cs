using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MinionModifierBase {
    // add to base value 
    [Inspectable,Range(0,1)]
    public float probability;
    [Inspectable]
    public float range;
    [Inspectable]
    public float viewRange;
    [Inspectable]
    public float damage;
    [Inspectable]
    public DamageType damageType; // if SAME_AS_BEFORE, don't change type
    [Inspectable]
    public float health;
    [Inspectable]
    public float speed;
    [Inspectable]
    public float attackSpeed;
}