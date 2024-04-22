using System;

[Serializable]
public class MinionBase {
    
    public string name;

    public float health;

    public float speed;
    
    public DamageType baseType;
    public DamageType addonType;

    public AttackMode attackMode;

    public LockMode lockMode;

    public float fireInterval;

    public float fireTime; // each fire take time

    public SpecialEffect specialEffect;

    public MinionProperty[] levelRelatedProperties;
}