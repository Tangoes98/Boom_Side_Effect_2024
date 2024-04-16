using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefenseTower : Architect
{
    [SerializeField]
    protected DefenseTowerStatus status;
    [SerializeField]
    protected DefenseTowerBase baseInfo;

    public override ArchitectBase Info() => baseInfo;
    public override ArchitectStatus Status() => status;

    public override void UpgradeTo(int level) {
        if(level==this.level) {
            return;
        }
        this.level = level;
        DefenseTowerProperty prop = (DefenseTowerProperty) GetBaseProperty(level);
        // refresh all properties
        status.maxLinkNum = prop.maxLinkNum;
        status.linkRange = prop.linkRange;
        status.range = prop.range;
        status.damage = prop.damage;
        status.damageType = prop.damageType;
        status.attackSpeed = prop.attackSpeed;

        ApplyModifer();

    }

    protected override void ApplyModifer() {
        if(sourceArchitectLinkNum==0) {
            return;
        }
        try {
            DefenseTowerModifierBase modifier = (DefenseTowerModifierBase) GetBaseProperty(level).GetModifers().Where(p=>p.sourceLinkNum==sourceArchitectLinkNum).First();
            status.maxLinkNum += modifier.maxLinkNum;
            status.range += modifier.range;
            status.damage += modifier.damage;
            if(modifier.damageType!=DamageType.SAME_AS_BEFORE) 
                status.damageType = modifier.damageType;
            status.attackSpeed += modifier.attackSpeed;
        } catch(Exception e) {
            Debug.LogError("Fail to apply modifer for num " + sourceArchitectLinkNum);
            Debug.LogError(e.Message);
        }
    }

    protected override void Awake() {
        base.Awake(); // keep this!
    }

}
