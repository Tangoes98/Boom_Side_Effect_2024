public class ArchitectModiferBase {
    // add to base value 
    public ArchitectBase.ArchitectType Type {get;}
    public float Probability {get;}
    public int MaxLinkNum {get;}
    public float Range {get;}

    // defense tower only
    public float Damage {get;}
    public DamageType DamageType {get;} // change damage type
    public float AttackSpeed  {get;}


    // barrack only
    public int MaxMinionNum  {get;}
    public float CoolDown {get;}
	public MinionModifierBase MinionModifier {get;} 


    public ArchitectModiferBase(float probability, int maxLinkNum, float range, float damage, DamageType damageType, float attackSpeed) {
        // constructor for defense towner
        if(probability<0 || probability>1) {
            throw new System.ArgumentOutOfRangeException("probability has to be in 0-1");
        }
        Type = ArchitectBase.ArchitectType.DEFENCE_TOWER;
        Probability = probability;
        MaxLinkNum = maxLinkNum;
        Range = range;
        Damage = damage;
        DamageType = damageType;
        AttackSpeed = attackSpeed;
    }

    public ArchitectModiferBase(float probability, int maxLinkNum, float range, int maxMinionNum,float coolDown, MinionModifierBase minionModifierBase) {
        // constructor for barrack
        if(probability<0 || probability>1) {
            throw new System.ArgumentOutOfRangeException("probability has to be in 0-1");
        }
        Type = ArchitectBase.ArchitectType.BARRACK;
        Probability = probability;
        MaxLinkNum = maxLinkNum;
        Range = range;
        MaxMinionNum = maxMinionNum;
        CoolDown = coolDown;
        MinionModifier = minionModifierBase;
    }

}