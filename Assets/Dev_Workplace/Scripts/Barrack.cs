using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public partial class Barrack : Architect
{
    [SerializeField]
    protected BarrackStatus status; // 只用这里的数据
    [SerializeField]
    protected BarrackBase baseInfo; // 不要直接调用！初始数据

    public override ArchitectBase Info() => baseInfo;
    public override ArchitectStatus Status() => status;

    public override void UpgradeTo(int level) {
        if(level==this.level) {
            return;
        }
        this.level = level;
        BarrackProperty prop = (BarrackProperty)GetBaseProperty(level);
        // refresh all properties
        //status.maxLinkNum = prop.maxLinkNum;
        status.range = prop.range;
        status.maxMinionNum = prop.maxMinionNum;

        status.manufactureInterval = baseInfo.manufactureInterval;
        status.manufactureTime = baseInfo.manufactureTime;
        status.minionPrefab = baseInfo.minionPrefab;
    }

    protected override void Awake() {
        base.Awake(); // keep this!
    }

    
    public void ManufactureMinion() {
        GameObject minionObj = Instantiate(baseInfo.minionPrefab); // 改
        
        Minion minion = minionObj.GetComponent<Minion>();
        minion.Initialize(this);
        status.currentMinions.Add(minion);
    }

    public void DestroyMinion(Minion minion) {
        status.currentMinions.Remove(minion);
    }
}
