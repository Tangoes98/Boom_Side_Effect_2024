using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionIntervalState : IState
{
    Minion manager;
    MinionStatus status;
    public MinionIntervalState(Minion manager)
    {
        this.manager = manager;
        this.status = manager.status;
    }
    float timer;
    Minion attackTarget;
    public void onEnter()
    {
        //*Set Animation State
        manager.animationController.SwitchAnimState("Idle");
        // refresh nearest enemies
        Minion[] targets = manager.GetOppenentInRange(status.range);
        manager.targets = targets;

        attackTarget = manager.targets==null? null : manager.targets[0];
        timer = status.fireInterval;
        if(attackTarget!=null && 
            Vector3.Distance(attackTarget.transform.position, manager.transform.position) < status.range) {
            // in range, no need to move
             manager.agent.speed = 0;
        }
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        timer -= Time.deltaTime;

        //���ʧȥ�˹���Ŀ�꣬�ص�����
        if (attackTarget == null ||
            Vector3.Distance(attackTarget.transform.position, manager.transform.position) >= status.range)
        {
            manager.TransitionState(MinionStateType.IDLE);
            return;
        }

        if (timer <= 0)
        {
            manager.TransitionState(MinionStateType.ATTACK);
        }
    }
}
