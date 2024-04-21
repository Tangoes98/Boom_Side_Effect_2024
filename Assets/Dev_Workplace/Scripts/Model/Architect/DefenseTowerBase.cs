using System;

[Serializable]
public class DefenseTowerBase : ArchitectBase {
    public static readonly ArchitectType type = ArchitectType.DEFENCE_TOWER;

    public override ArchitectProperty[] GetProperties() => levelRelatedProperties;

    public DamageType baseType;
    public DamageType addonType;

    public AttackMode attackMode;

    public LockMode lockMode;

    public float fireInterval;

    public float fireTime; // each fire take time

    public float aoeRange;

    public SpecialEffect specialEffect;

    public DefenseTowerProperty[] levelRelatedProperties;
}

public enum DamageType {
    NONE,FIRE,OIL,POISON,RADIATION
}

public enum AttackMode {
    SINGLE_HIT, // 单次攻击
    CONTINUOUS // 持续攻击
}

public enum LockMode {
    NORMAL, // 普通锁定
    TRACK // 追踪锁定
}

public enum SpecialEffect {
    NONE,
    WEAK, //脆弱
    SLOW, //减速
    POSION, //中毒
    DIZZY //眩晕
}