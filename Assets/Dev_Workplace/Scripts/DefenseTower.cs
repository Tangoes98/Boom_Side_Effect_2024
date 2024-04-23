using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : Architect
{
    [SerializeField]
    protected DefenseTowerStatus status; // 只用这里的数据
    [SerializeField]
    protected DefenseTowerBase baseInfo; // 不要直接调用！初始数据

    public override ArchitectBase Info() => baseInfo;
    public override ArchitectStatus Status() => status;

    public override void UpgradeTo(int level) {
        if(level==this.level) {
            return;
        }
        this.level = level;
        DefenseTowerProperty prop = (DefenseTowerProperty) GetBaseProperty(level);
        // refresh all properties
        //status.maxLinkNum = prop.maxLinkNum;
        status.range = prop.range;
        status.damage = prop.damage;

        status.attackMode = baseInfo.attackMode;
        status.lockMode = baseInfo.lockMode;
        status.fireInterval  = baseInfo.fireInterval;
        status.fireTime = baseInfo.fireTime;
        status.aoeRange = baseInfo.aoeRange;

        status.specialEffect = baseInfo.specialEffect;
        status.specialEffectModifier = prop.specialEffectModifier;
        status.specialEffectLastTime = prop.specialEffectLastTime;

        status.secondSpEffect = baseInfo.secondSpEffect;
        status.secondSpEffectModifier = prop.secondSpEffectModifier;
        status.secondSpEffectLastTime = prop.secondSpEffectLastTime;

    }


    protected override void Awake() {
        base.Awake(); // keep this!
    }

    private float GetDamage(out ModifierType modifierType) { // BUFF/DEBUFF
        float modifier = ArchiLinkManager.Instance.GetModifier(Unstability, out modifierType);
        return status.damage * modifier;
    }

}
