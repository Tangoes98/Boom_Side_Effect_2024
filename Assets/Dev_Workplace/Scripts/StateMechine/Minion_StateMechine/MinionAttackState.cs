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
    Vector3 _freezePos;

    public void onEnter()
    {
        //*Set Animation State
        manager.animationController.SwitchAnimState("Attack");

        
        timer = status.fireTime;
        _attacked = false;
        timerInterval0_1 = 0.1f;

        if(manager.mainBase==null) {
            if(manager.targets==null) {
                manager.TransitionState(MinionStateType.IDLE);
                return;
            }
            attackTarget = manager.targets[0];
            manager.agent.SetDestination(attackTarget.transform.position);
        } else {
            manager.agent.SetDestination(manager.mainBase.transform.position);
        }

        manager.agent.speed = 0;

        _freezePos = manager.transform.position;
        manager.agent.velocity = Vector3.zero;
    }

    public void onExit()
    {
        timer = status.fireTime;
        timerInterval0_1 = 0.1f;
    }
    public void onUpdate()
    {   
        manager.transform.position = _freezePos;
        timer -= Time.deltaTime;
        timerInterval0_1 -= Time.deltaTime;

        if(manager.mainBase!=null) {
            manager.transform.LookAt(manager.mainBase.transform.position);
            if (status.attackMode == AttackMode.SINGLE_HIT && !_attacked) {
                manager.mainBase.TakeDamage(manager.status.damage);
                _attacked = true;
            }
            if (timer <= 0)
            {
                manager.TransitionState(MinionStateType.INTERVAL);
            }
            return;
        }

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

                manager.aoeAttack.TriggerAOE(manager.transform.position,status.takeDamageModifer); 
                manager.TransitionState(MinionStateType.DYING);
                return;

            }
            if(manager.aoeAttack!=null && manager.aoeAttack.type==AoeType.CIRCLE_CENTER_ENEMY) {
                manager.aoeAttack.TriggerAOE(attackTarget.transform.position,status.takeDamageModifer); 
            }

            if(manager.aoeAttack!=null && (manager.aoeAttack.type==AoeType.CIRCLE_CENTER_SELF || 
                                            manager.aoeAttack.type==AoeType.ARC || 
                                            manager.aoeAttack.type==AoeType.LINE)) {
                manager.aoeAttack.TriggerAOE(manager.transform.position,status.takeDamageModifer); 
            }
            if(manager.aoeAttack==null || manager.aoeAttack.type!=AoeType.CIRCLE_CENTER_SELF)manager.Attack(attackTarget);
            _attacked = true;
        }

        if (status.attackMode == AttackMode.CONTINUOUS && timerInterval0_1 <= 0) {
            if(manager.aoeAttack!=null && manager.aoeAttack.type==AoeType.CIRCLE_CENTER_ENEMY) {
                manager.aoeAttack.TriggerAOE(attackTarget.transform.position,status.takeDamageModifer); 
            }

            if(manager.aoeAttack!=null && (manager.aoeAttack.type==AoeType.CIRCLE_CENTER_SELF || 
                                            manager.aoeAttack.type==AoeType.ARC || 
                                            manager.aoeAttack.type==AoeType.LINE)) {
                manager.aoeAttack.TriggerAOE(manager.transform.position,status.takeDamageModifer); 
            }
            manager.Attack(attackTarget);
            timerInterval0_1 =0.1f;
            
        }

        if (timer <= 0)
        {
            manager.TransitionState(MinionStateType.INTERVAL);
        }

        
    }
}
