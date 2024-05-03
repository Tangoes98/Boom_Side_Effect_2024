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
        timer = status.fireInterval;
        
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        timer -= Time.deltaTime;
        // refresh nearest enemies
        Minion[] targets = manager.GetOppenentInRange(status.range);
        manager.targets = targets;

        attackTarget = manager.targets==null? null : manager.targets[0];
        if(attackTarget!=null) {
            if(Vector3.Distance(attackTarget.transform.position, manager.transform.position) < status.range)
                // in range, no need to move
                manager.agent.speed = 0;
            else {
                manager.agent.speed = manager.status.speed;
                manager.transform.LookAt(attackTarget.transform.position);
                manager.agent.SetDestination(attackTarget.transform.position);
            }
        }

        if (timer <= 0)
        {
            // must wait for interval.
            if (attackTarget == null ||
                Vector3.Distance(attackTarget.transform.position, manager.transform.position) >= status.range)
            {
                manager.TransitionState(MinionStateType.IDLE);
                return;
            }
            manager.TransitionState(MinionStateType.ATTACK);
        }
    }
}
