using System;

[Serializable]
public class MinionBase {
    
    public string name;

    public float speed;
    
    public DamageType baseType;
    public DamageType addonType;

    public AttackMode attackMode;

    public LockMode lockMode;

    public MinionType minionType;

    public float fireInterval;

    public float fireTime; // each fire take time

    public SpecialEffect specialEffect;

    public SpecialEffect secondSpecialEffect;

    public MinionProperty[] levelRelatedProperties;
}
public enum MinionStateType
{
    IDLE,//´ý»ú×´Ì¬
    VIEW,//Ë÷µÐ×·×Ù
    INTERVAL,//¼ä¸ô×´Ì¬
    ATTACK,//¹¥»÷×´Ì¬
    DYING,//±ôËÀ
    DIZZY//Ñ£ÔÎ
}

public enum MinionType
{
    ENEMY,//µÐ¾ü 
    FRIEND//ÓÑ¾ü
}