using UnityEngine;

public class Minion : MonoBehaviour {

    [SerializeField]
    public MinionStatus status; //建筑status里的minionModifier会修改status
    [SerializeField]
    public MinionBase baseInfo;

    public string Code => baseInfo.code;


    void Awake() {
        // load baseInfo
        Reload();
    }

    void Reload() {
        status.range = baseInfo.range;
        status.viewRange = baseInfo.viewRange;
        status.isMelee = baseInfo.isMelee;
        status.damage = baseInfo.damage;
        status.damageType = baseInfo.damageType;
        status.health = baseInfo.health;
        status.speed = baseInfo.speed;
        status.attackSpeed = baseInfo.attackSpeed;
    }



}