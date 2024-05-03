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
    bool _attacked;

    public void onEnter()
    {
        //*Set Animation State
        manager.animationController.SwitchAnimState("Attack");

        attackTarget = manager.targets[0];
        timer = status.fireTime;
        _attacked = false;
        timerInterval0_1 = 0.1f;
        manager.agent.SetDestination(attackTarget.transform.position);
        manager.agent.speed = 0;
        //manager.agent.velocity = Vector3.zero;
    }
    public void onExit()
    {
        timer = status.fireTime;
        timerInterval0_1 = 0.1f;
    }
    public void onUpdate()
    {
        timer -= Time.deltaTime;
        timerInterval0_1 -= Time.deltaTime;

        // if (Vector3.Distance(attackTarget.transform.position, manager.transform.position) < manager.agent.stoppingDistance)
        //     manager.agent.speed = 0;

        if (attackTarget == null 
        //|| Vector3.Distance(attackTarget.transform.position, manager.transform.position) > status.range
        )
        {
            manager.TransitionState(MinionStateType.INTERVAL);
            return;
        }

        manager.transform.LookAt(attackTarget.transform.position);

        if (status.attackMode == AttackMode.SINGLE_HIT && !_attacked)
        {
            if(manager.aoeAttack!=null && manager.aoeAttack.type==AoeType.CIRCLE_CENTER_SELF_DIE) {

                manager.aoeAttack.TriggerAOE(manager.transform.position); 
                manager.TransitionState(MinionStateType.DYING);
                return;

            }
            if(manager.aoeAttack!=null && manager.aoeAttack.type==AoeType.CIRCLE_CENTER_ENEMY) {
                manager.aoeAttack.TriggerAOE(attackTarget.transform.position); 
            }

            if(manager.aoeAttack!=null && (manager.aoeAttack.type==AoeType.CIRCLE_CENTER_SELF || 
                                            manager.aoeAttack.type==AoeType.ARC || 
                                            manager.aoeAttack.type==AoeType.LINE)) {
                manager.aoeAttack.TriggerAOE(manager.transform.position); 
                return;
            }
            manager.Attack(attackTarget);
            _attacked = true;
        }

        if (status.attackMode == AttackMode.CONTINUOUS && timerInterval0_1 <= 0) {
            manager.Attack(attackTarget);
            timerInterval0_1 =0.1f;
            
        }

        if (timer <= 0)
        {
            manager.TransitionState(MinionStateType.INTERVAL);
        }

        
    }
}
