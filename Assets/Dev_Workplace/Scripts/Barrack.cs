using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.AI;
using Unity.Burst.CompilerServices;
using UnityEngine.SocialPlatforms;
using Yunhao_Fight;
using System.Drawing;

public partial class Barrack : Architect
{
    [SerializeField]
    protected BarrackStatus status; // 只用这里的数据
    [SerializeField]
    protected BarrackBase baseInfo; // 不要直接调用！初始数据

    public override ArchitectBase Info() => baseInfo;
    public override ArchitectStatus Status() => status;

    Dictionary<BarrackStateType, IState> states = new Dictionary<BarrackStateType, IState>();

    public Vector3 minionDestination
    {
        get
        {
            return GetIdlePosition();
        }
        set => minionDestination = value;
    }

    public override void UpgradeTo(int level) {
        //if (level == this.level)
        //{
        //    return;
        //}如果Reload()函数的意义是重载的话，这里可能要增加条件，不然无法执行
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

        Reload();

        states.Add(BarrackStateType.IDLE, new BarrackIdleState(this));
        states.Add(BarrackStateType.SPAWN, new BarrackSpawnState(this));
        TransitionState(BarrackStateType.IDLE);
    }
    private void Update()
    {
        currentState.onUpdate();
    }


    public void ManufactureMinion() {
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(transform.position, out hit, status.range, NavMesh.AllAreas)) return;
        GameObject minionObj = Instantiate(baseInfo.minionPrefab,this.transform); // 改
        
        Minion minion = minionObj.GetComponent<Minion>();
        minion.Initialize(this);
        status.currentMinions.Add(minion);
    }

    public void DestroyMinion(Minion minion) {
        status.currentMinions.Remove(minion);
        Destroy(minion.gameObject);//改
    }

    public void TransitionState(BarrackStateType type)
    {
        if (currentState != null)
        {
            currentState.onExit();
        }
        currentState = states[type];
        currentState.onEnter();

        //Debug.Log(gameObject.name+" change to "+type.ToString());
    }
    public Vector3 GetSpawnPosition()
    {
        Vector3 sp=transform.position;//改
        NavMeshHit hit;
        Vector3 point = NavMesh.SamplePosition(sp, out hit, status.range, NavMesh.AllAreas) ? hit.position : Vector3.zero;
        return point;
    }
    public Vector3 GetIdlePosition()//需要加算一个偏移量，保持一定阵型
    {
        Collider[] points=Physics.OverlapSphere(this.transform.position, status.range, LevelManager.SpawnPointLayer());
        if (points.Length > 0)
        {
            return new List<Collider>(points)
                .OrderBy(p => Vector3.Distance(this.transform.position, p.transform.position))
                .First().transform.position;
        }
        else
        {
            Debug.LogError("there's no spawn point in range");
            return Vector3.zero;
        }
    }
}
