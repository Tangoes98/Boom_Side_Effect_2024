using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public partial class Barrack : Architect
{
    [SerializeField]
    protected BarrackStatus status;
    [SerializeField]
    protected BarrackBase baseInfo;

    public override ArchitectBase Info() => baseInfo;
    public override ArchitectStatus Status() => status;

    public override void UpgradeTo(int level) {
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
        status.minionCode = prop.minionCode;
        
        ApplyModifer();
    }

    protected override void ApplyModifer() {
        if(sourceArchitectLinkNum==0) {
            return;
        }
        try {
            BarrackModifierBase modifier = (BarrackModifierBase) GetBaseProperty(level).GetModifers().Where(p=>p.sourceLinkNum==sourceArchitectLinkNum).First();
            status.maxLinkNum += modifier.maxLinkNum;
            status.range += modifier.range;
            status.maxMinionNum += modifier.maxMinionNum;
            status.coolDown += modifier.coolDown;
            status.minionModifier = modifier.minionModifier;
        } catch(Exception e) {
            Debug.LogError("Fail to apply modifer for num " + sourceArchitectLinkNum);
            Debug.LogError(e.Message);
        }
    }

    protected override void Awake() {
        base.Awake(); // keep this!
    }

}
