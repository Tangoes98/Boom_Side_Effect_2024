using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttackState : IState
{
    Minion manager;
    MinionStatus status;

    Minion attackTarget;
    public MinionAttackState(Minion manager)
    {
        this.manager = manager;
        this.status = manager.status;
    }

    float timer;
    float timerInterval0_1;

    public void onEnter()
    {
        //*Set Animation State
        manager.animationController.SwitchAnimState("Attack");

        attackTarget = manager.targets[0];
        timer = status.fireTime;
        timerInterval0_1 = 1;
        manager.agent.SetDestination(attackTarget.transform.position);
        manager.agent.speed = 0;
        //manager.agent.velocity = Vector3.zero;
    }
    public void onExit()
    {
        timer = status.fireTime;
        timerInterval0_1 = 1;
    }
    public void onUpdate()
    {
        timer -= Time.deltaTime;
        timerInterval0_1 -= Time.deltaTime;

        // if (Vector3.Distance(attackTarget.transform.position, manager.transform.position) < manager.agent.stoppingDistance)
        //     manager.agent.speed = 0;

        //锟斤拷锟绞ワ拷斯锟斤拷锟侥匡拷辏拷氐锟斤拷锟斤拷锟�
        if (attackTarget == null 
        //|| Vector3.Distance(attackTarget.transform.position, manager.transform.position) > status.range
        )
        {
            manager.TransitionState(MinionStateType.IDLE);
            return;
        }

        manager.transform.LookAt(attackTarget.transform.position);

        if (status.attackMode == AttackMode.SINGLE_HIT)
        {
            if (timerInterval0_1 <= 0)
            {
                manager.Attack(attackTarget);
                timerInterval0_1 = 1;
            }
        }
        if (timer <= 0)
        {
            if (status.attackMode == AttackMode.CONTINUOUS) manager.Attack(attackTarget);
            manager.TransitionState(MinionStateType.INTERVAL);
        }
    }
}
