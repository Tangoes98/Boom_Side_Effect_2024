using UnityEngine;

public class Minion : MonoBehaviour {
    // Editor里只需要填写code，其他property会通过code自动填充
    [SerializeField]
    protected string code;


    [SerializeField]
    protected MinionBase info;
    [SerializeField]
    float range;
    [SerializeField]
    float viewRange;
    [SerializeField]
    bool isMelee;
    [SerializeField]
    float damage;
    [SerializeField]
    DamageType damageType;
    [SerializeField]
    float health;
    [SerializeField]
    float speed;
    [SerializeField]
    float attackSpeed;


    void Awake() {
        // load info
        info = ArchitectConfig.GetMinionBase(code);
        InitializeFromInfo();
    }

    void InitializeFromInfo() {
        range = info.Range;
        viewRange = info.ViewRange;
        isMelee = info.IsMelee;
        damage = info.Damage;
        damageType = info.DamageType;
        health = info.Health;
        speed = info.Speed;
        attackSpeed = info.AttackSpeed;
    }



}