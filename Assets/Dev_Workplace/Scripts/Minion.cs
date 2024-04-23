using System.Linq;
using UnityEngine;

public class Minion : MonoBehaviour {

    public int level;
    public ModifierType modifierType; // BUFF/DEBUFF/NONE, 可能会根据此变量，改变小兵外观（大小）

    [SerializeField]
    public MinionStatus status; // 只用这里的数据
    [SerializeField]
    private MinionBase baseInfo; // 不要直接调用！初始数据


    private Barrack _parent;

    public void Initialize(Barrack parent) {
        _parent = parent;
        level = parent.level;
        float modifier = GetMinionDamageAndHealthModifier(parent.Unstability);
        MinionProperty prop = GetBaseProperty(level);

        status.maxHealth = modifier * baseInfo.health;
        status.health = status.maxHealth;
        status.damage = modifier * prop.damage;

        status.range = prop.range;
        status.viewRange = prop.viewRange;
        status.specialEffectModifier = prop.specialEffectModifier;
        status.specialEffectLastTime = prop.specialEffectLastTime;
        status.attackMode = baseInfo.attackMode;
        status.lockMode = baseInfo.lockMode;
        status.fireInterval = baseInfo.fireInterval;
        status.fireTime = baseInfo.fireTime;
        status.specialEffect = baseInfo.specialEffect;

    }

    private MinionProperty GetBaseProperty(int level)  => baseInfo.levelRelatedProperties.Where(p=>p.level==level).First();

    private float GetMinionDamageAndHealthModifier(int unstability) => ArchiLinkManager.Instance.GetModifier(unstability, out modifierType);

    public void SelfDestroy() {
        _parent.DestroyMinion(this);
    }

}