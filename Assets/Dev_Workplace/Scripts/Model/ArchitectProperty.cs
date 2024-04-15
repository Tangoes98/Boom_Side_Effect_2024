using System.Collections.Generic;
using System.Linq;

public class ArchitectProperty {
    public int MaxLinkNum {get;}
    public float Range {get;}
    public ArchitectBase.ArchitectType Type {get;}

    private Dictionary<int,ArchitectModiferBase> _modifers; //parent architect link num : new mutant architect

    // defense tower only

    public float Damage {get;}
    public DamageType DamageType {get;}
    public float AttackSpeed {get;}

    // barrack only

    public MinionBase MinionBase {get;}
    public int MaxMinionNum {get;}
    public float CoolDown {get;}


    public ArchitectProperty(int maxLinkNum, float range, Dictionary<int,ArchitectModiferBase> modifers, 
        float damage, DamageType damageType, float attackSpeed) 
    {
        // constructor for defense towner
        Type = ArchitectBase.ArchitectType.DEFENCE_TOWER;
        MaxLinkNum = maxLinkNum;
        Range = range;

        if(modifers.Values.Any(m=>m.Type != Type)) {
            throw new System.Exception("property type doesn't match modifier type");
        }

        _modifers = modifers;
        Damage = damage;
        DamageType = damageType;
        AttackSpeed = attackSpeed;
    }

    public ArchitectProperty(int maxLinkNum, float range, Dictionary<int,ArchitectModiferBase> modifers,
        MinionBase minionBase, int maxMinionNum, float cooldown) 
    {
        // constructor for barrack
         Type = ArchitectBase.ArchitectType.BARRACK;
        MaxLinkNum = maxLinkNum;
        Range = range;

        if(modifers.Values.Any(m=>m.Type != Type)) {
            throw new System.Exception("property type doesn't match modifier type");
        }
        
        _modifers = modifers;
        MinionBase = minionBase;
        MaxMinionNum = maxMinionNum;
        CoolDown = cooldown;
    }

    public ArchitectModiferBase Modifer(int sourceArchLinkNum) => _modifers == null? null : _modifers[sourceArchLinkNum];
}
