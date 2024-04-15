using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : Architect
{
    protected float damage;
    protected DamageType damageType;
    protected float attackSpeed;

    public override void UpgradeTo(int level) {
        if(level==this.level) {
            return;
        }
        
        ArchitectProperty prop = info.GetProperty(level);
        // refresh all properties
        maxLinkNum = prop.MaxLinkNum;
        range = prop.Range;
        damage = prop.Damage;
        damageType = prop.DamageType;
        attackSpeed = prop.AttackSpeed;

    }

}
