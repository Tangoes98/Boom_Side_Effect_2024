using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Barrack : Architect
{
    [SerializeField]
    public BarrackStatus status;
    [SerializeField]
    protected BarrackBase baseInfo;

    protected override ArchitectBase GetInfo() => baseInfo;

    protected override void UpgradeTo(int level) {
        if(level==this.level) {
            return;
        }
        this.level = level;
        BarrackProperty prop = (BarrackProperty)GetBaseProperty(level);
        // refresh all properties
        status.maxLinkNum = prop.maxLinkNum;
        status.linkRange = prop.linkRange;
        status.range = prop.range;
        status.maxMinionNum = prop.maxMinionNum;
        status.coolDown = prop.coolDown;

    }

}
