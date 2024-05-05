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
        //manager.animationController.SwitchAnimState("Idle");   
        timer = status.fireInterval;
        
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        timer -= Time.deltaTime;
        // refresh nearest enemies
        MainBase mainBase;
        Minion[] targets = manager.GetOppenentInRange(status.range,status.minRange, out mainBase);
        manager.targets = targets;
        manager.mainBase = targets==null && mainBase !=null? mainBase : null;

        if(manager.code=="E04") {
            // dog go straight to main base
            if(mainBase==null) {
                manager.agent.SetDestination(manager.moveDestination);
                manager.animationController.SwitchAnimState("Move");
                manager.agent.speed = manager.status.speed;
            } else {
                manager.transform.LookAt(mainBase.transform.position);
                manager.agent.speed = 0;
                manager.animationController.SwitchAnimState("Idle"); 
            }

            if (timer <= 0)
            {
                // must wait for interval.
                if (mainBase==null)
                {
                    manager.TransitionState(MinionStateType.IDLE);
                }
                manager.TransitionState(MinionStateType.ATTACK);
            }
            return;
        } 

        attackTarget = manager.targets==null? null : manager.targets[0];
        bool outOfBarrackRange = false, veryOutOfRange = false;
        if(manager.Info().minionType == MinionType.FRIEND) {
            var barrack = manager.Barrack();
            outOfBarrackRange = Vector3.Distance(barrack.transform.position, manager.transform.position) >= manager.Barrack().Status().range;
            veryOutOfRange = Vector3.Distance(barrack.transform.position, manager.transform.position) >= manager.Barrack().Status().range + 2;
        }
                    
        if(attackTarget!=null) {
            if(Vector3.Distance(attackTarget.transform.position, manager.transform.position) < status.range || 
                (outOfBarrackRange && !veryOutOfRange)) {
                // in range, no need to move
                // move out of range, stop
                manager.animationController.SwitchAnimState("Idle"); 
                manager.agent.speed = 0;
            }
            else {
                manager.agent.speed = manager.status.speed;
                manager.animationController.SwitchAnimState("Move");
                //manager.transform.LookAt(attackTarget.transform.position);
                manager.agent.SetDestination(veryOutOfRange? manager.moveDestination : attackTarget.transform.position);
            }
        } else {
            manager.animationController.SwitchAnimState("Idle"); 
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
    public string Type() => MinionStateType.INTERVAL.ToString();
}
