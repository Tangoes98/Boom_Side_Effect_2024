using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : Architect
{
    [SerializeField]
    public DefenseTowerStatus status;
    [SerializeField]
    protected DefenseTowerBase baseInfo;

    protected override ArchitectBase GetInfo() => baseInfo;

    protected override void UpgradeTo(int level) {
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

    }

}
