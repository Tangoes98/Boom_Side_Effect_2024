using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Enemy[] targets;

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
        status.specialEffectModifier = prop.specialEffectModifier;
        status.specialEffectLastTime = prop.specialEffectLastTime;

        status.attackMode = baseInfo.attackMode;
        status.lockMode = baseInfo.lockMode;
        status.fireInterval  = baseInfo.fireInterval;
        status.fireTime = baseInfo.fireTime;
        status.aoeRange = baseInfo.aoeRange;
        status.specialEffect = baseInfo.specialEffect;

    }


    protected override void Awake() {
        base.Awake(); // keep this!

        states.Add(DefenseTowerStateType.IDLE, new DefenseTowerIdleState(this));
        states.Add(DefenseTowerStateType.INTERVAL, new DefenseTowerIdleState(this));
        states.Add(DefenseTowerStateType.ATTACK, new DefenseTowerIdleState(this));
        TransitionState(DefenseTowerStateType.IDLE);
    }

    void Update()
    {
        currentState.onUpdate();
    }

    public float GetDamage(out ModifierType modifierType) { // BUFF/DEBUFF
        float modifier = ArchiLinkManager.Instance.GetModifier(Unstability, out modifierType);
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
    }

    public Enemy[] GetEnemyInRange()
    {
        Collider[] attackTargets = Physics.OverlapSphere(this.transform.position, status.range, LevelManager.EnemyLayer());
        Enemy[] enemies = attackTargets.Select(collider => collider.gameObject.GetComponent<Enemy>())
                               .Where(enemy => enemy != null)
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
    }
    IEnumerator ContinuousAttack()
    {
        float timer = status.fireTime;
        while (timer > 0)
        {
            timer -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        DealDamage();
    }
    //应该拓展DealDamage和checkTarget就能处理成不同的建筑功能
    protected virtual void DealDamage()
    {
        //暂时为单体。
        targets[0].gameObject.SendMessage("TakeDamage", GetDamage(out modifiertype));//攻击最近的目标
    }

    public virtual bool checkTarget()//检测攻击对象是否存在
    {
        //暂时为单体。
        bool isExist = targets[0] != null && Vector3.Distance(targets[0].transform.position, transform.position) < status.range;
        return isExist;

    }

}

