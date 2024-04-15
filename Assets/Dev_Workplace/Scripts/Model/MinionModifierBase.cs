public class MinionModifierBase {
    // add to base value 
    public float Probability {get;}
    public float Range {get;}
    public float ViewRange {get;}
    public float Damage {get;}
    public DamageType DamageType {get;}
    public float Health {get;}
    public float Speed {get;}
    public float AttackSpeed {get;}


    public MinionModifierBase(float probability, float range, float viewRange, float damage, 
        DamageType damageType, float health, float speed, float attackSpeed) {
        if(probability<0 || probability>1) {
            throw new System.ArgumentOutOfRangeException("probability has to be in 0-1");
        }
        Probability = probability;
        Range = range;
        ViewRange = viewRange;
        Damage = damage;
        DamageType = damageType;
        Health = health;
        Speed = speed;
        AttackSpeed = attackSpeed;
    }
}