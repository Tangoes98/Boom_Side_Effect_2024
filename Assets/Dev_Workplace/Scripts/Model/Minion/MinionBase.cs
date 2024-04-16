using System;
using Unity.VisualScripting;

[Serializable]
public class MinionBase {
    [Inspectable]
    public string code;
    [Inspectable]
    public string name;
    [Inspectable]
    public float range;
    [Inspectable]
    public float viewRange;
    [Inspectable]
    public float damage;
    [Inspectable]
    public bool isMelee;
    [Inspectable]
    public DamageType damageType;
    [Inspectable]
    public float health;
    [Inspectable]
    public float speed;
    [Inspectable]
    public float attackSpeed;
}