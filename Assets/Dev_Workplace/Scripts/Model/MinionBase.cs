public class MinionBase {
    public string Code {get;}
    public string Name {get;}
    public float Range {get;}
    public float ViewRange {get;}
    public float Damage {get;}
    public bool IsMelee {get;}
    public DamageType DamageType {get;}
    public float Health {get;}
    public float Speed {get;}
    public float AttackSpeed {get;}

     public MinionBase(string code, string name, float range, float viewRange, float damage, 
        bool isMelee, DamageType damageType, float health, float speed, float attackSpeed)
    {
        Code=code;
        Name=name;
        Range = range;
        ViewRange = viewRange;
        Damage = damage;
        IsMelee = isMelee;
        DamageType = damageType;
        Health = health;
        Speed = speed;
        AttackSpeed = attackSpeed;
    }
}