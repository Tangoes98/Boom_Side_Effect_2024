using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionViewState : IState
{
    Minion manager;
    MinionStatus status;

    Minion viewTarget;
    public MinionViewState(Minion manager)
    {
        this.manager = manager;
        this.status = manager.status;
    }
    public void onEnter()
    {
        viewTarget = manager.targets==null? null : manager.targets[0];

        if (viewTarget==null) manager.animationController.SwitchAnimState("Idle");
        else manager.animationController.SwitchAnimState("Move");
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        // refresh nearest enemy
        MainBase mainBase;
        Minion[] targets = manager.GetOppenentInRange(status.range,status.minRange, out mainBase);
        manager.mainBase = targets==null && mainBase !=null? mainBase : null;
        if (targets != null)
        {
            manager.targets = targets;
            manager.TransitionState(MinionStateType.ATTACK);
            return; 
        } else if(mainBase!=null) {
            manager.TransitionState(MinionStateType.ATTACK); 
            return;         
        }
        manager.targets = manager.GetOppenentInRange(status.viewRange,0, out mainBase);
        viewTarget = manager.targets==null? null : manager.targets[0];
        // no enemy in range
        if (manager.targets == null ||
            Vector3.Distance(viewTarget.transform.position, manager.transform.position) >= status.viewRange)
        {
            manager.TransitionState(MinionStateType.IDLE);
            return;
        }
        
        
        // limit moving range
        if (manager.Info().minionType == MinionType.FRIEND)
        {
            Barrack barrack = manager.Barrack();
            var disToBarrack = Vector3.Distance(barrack.transform.position, manager.transform.position);
            if ( disToBarrack >= barrack.Status().range)
            {
                            
                if (disToBarrack >= barrack.Status().range + 2)
                {
                    manager.agent.SetDestination(manager.moveDestination);
                    manager.agent.speed = status.speed;
                    manager.animationController.SwitchAnimState("Move");
                } else if(viewTarget != null && Vector3.Distance(barrack.transform.position, viewTarget.transform.position) <= barrack.Status().range) {
                    manager.agent.SetDestination(viewTarget.transform.position);
                    manager.agent.speed = status.speed;
                    manager.animationController.SwitchAnimState("Move");
                }else {
                    manager.animationController.SwitchAnimState("Idle");
                    manager.agent.speed = 0;

                }
                return;
            }
        }
        manager.agent.speed = status.speed;
        manager.agent.SetDestination(viewTarget.transform.position);

        manager.animationController.SwitchAnimState("Move");
    }

    public string Type() => MinionStateType.VIEW.ToString();
}
