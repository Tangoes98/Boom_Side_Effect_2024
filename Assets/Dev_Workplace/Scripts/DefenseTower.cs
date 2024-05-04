using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Yunhao_Fight;

public class DefenseTower : Architect
{
    [SerializeField]
    protected DefenseTowerStatus status; // 只用这里的数据
    [SerializeField]
    protected DefenseTowerBase baseInfo; // 不要直接调用！初始数据

    public override ArchitectBase Info() => baseInfo;
    public override ArchitectStatus Status() => status;

    protected Dictionary<DefenseTowerStateType, IState> states = new Dictionary<DefenseTowerStateType, IState>();
    public Enemy[] targets;

    [SerializeField] string stateLabel;

    public AoeAttack aoeAttack {private set; get; }

    public override void UpgradeTo(int level) {
        //if(level==this.level) {
        //    return;
        //}
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

        if(aoeAttack!=null && level!=1) {
            aoeAttack.Load();
        }

    }


    protected override void Awake() {
        base.Awake(); // keep this!
        Reload();

        states.Add(DefenseTowerStateType.IDLE, new DefenseTowerIdleState(this));
        states.Add(DefenseTowerStateType.INTERVAL, new DefenseTowerIntervalState(this));
        states.Add(DefenseTowerStateType.ATTACK, new DefenseTowerAttackState(this));

        TransitionState(DefenseTowerStateType.IDLE);

        aoeAttack = GetComponent<AoeAttack>();
    }

    void Update()
    {
        currentState.onUpdate();
    }

    public float GetDamage(out ModifierType modifierType) { // BUFF/DEBUFF
        float modifier = ArchiLinkManager.Instance.GetModifier(Unstability, out modifierType);
        //Debug.Log(modifier + " " + status.damage);
        return status.damage * modifier;
    }

    public void TransitionState(DefenseTowerStateType type)
    {
        if (currentState != null)
        {
            currentState.onExit();
        }
        currentState = states[type];
        currentState.onEnter();

        stateLabel = type.ToString();
    }

    public Enemy[] GetEnemyInRange()
    {
        Collider[] attackTargets = Physics.OverlapSphere(this.transform.position, status.range, LevelManager.EnemyLayer());
        Enemy[] enemies = attackTargets.Select(collider => collider.gameObject.GetComponent<Enemy>())
                               .Where(enemy => enemy != null)
                               .OrderBy(m => Vector3.Distance(this.transform.position, m.transform.position))
                               .ToArray();
        if (attackTargets.Length > 0)
        {
            //Debug.Log("Get a new enemy: " + enemy.name);
            return enemies;
        }
        else return null;
    }

    public void Attack()
    {
        switch (status.attackMode)
        {
            case AttackMode.SINGLE_HIT:
                StartCoroutine(SingleAttack());
                break;
            case AttackMode.CONTINUOUS:
                StartCoroutine(ContinuousAttack());
                break;
        }
    }
    IEnumerator SingleAttack()
    {
        yield return new WaitForSeconds(status.fireTime);
        
        DealDamage();
        TransitionState(DefenseTowerStateType.INTERVAL);
    }
    IEnumerator ContinuousAttack()
    {
        float timer = status.fireTime;
        while (timer > 0 && checkTarget())
        {
            DealDamage();
            timer -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        TransitionState(DefenseTowerStateType.IDLE);
    }
    //应该拓展DealDamage和checkTarget就能处理成不同的建筑功能
    protected virtual void DealDamage()
    {
        //暂时为单体。
        var damage =GetDamage(out modifiertype);
        if(aoeAttack!=null && aoeAttack.type==AoeType.CIRCLE_CENTER_SELF) {
            aoeAttack.TriggerAOE(this.transform.position,1); 
            return;
        }

        var target = targets[0].GetComponent<Minion>();
        target.TakeDamage(damage);
        target.TakeEffect(status.specialEffect, status.specialEffectModifier,status.specialEffectLastTime);
        target.TakeEffect(status.secondSpEffect, status.secondSpEffectModifier, status.secondSpEffectLastTime);
        if(aoeAttack!=null && aoeAttack.type==AoeType.CIRCLE_CENTER_ENEMY) {
            aoeAttack.TriggerAOE(target.transform.position,1); 
        }
        
    }

    public virtual bool checkTarget()//检测攻击对象是否存在
    {
        targets = GetEnemyInRange();
        bool isExist = targets!=null && targets[0] != null && Vector3.Distance(targets[0].transform.position, this.transform.position) < status.range;
        return isExist;

    }

}

