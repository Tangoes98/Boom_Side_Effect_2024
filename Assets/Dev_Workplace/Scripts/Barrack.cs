using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Architect
{
    protected List<Minion> curMinions; 
    protected int maxMinionNum;
    protected float coolDown;

    public override void UpgradeTo(int level) {
        if(level==this.level) {
            return;
        }
        ArchitectProperty prop = info.GetProperty(level);
        // refresh all properties
        maxLinkNum = prop.MaxLinkNum;
        range = prop.Range;
        maxMinionNum = prop.MaxMinionNum;
        coolDown = prop.CoolDown;

    }

}
