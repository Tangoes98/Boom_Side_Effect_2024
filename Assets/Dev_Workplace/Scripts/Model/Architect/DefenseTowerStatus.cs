using System;

[Serializable]
public class DefenseTowerStatus : ArchitectStatus {

    public float damage; 
    public AttackMode attackMode;

    public LockMode lockMode;

    public float fireInterval;

    public float fireTime; // each fire take time

    public float aoeRange;

    public SpecialEffect specialEffect;
    public float specialEffectModifier;
    public float specialEffectLastTime;
}