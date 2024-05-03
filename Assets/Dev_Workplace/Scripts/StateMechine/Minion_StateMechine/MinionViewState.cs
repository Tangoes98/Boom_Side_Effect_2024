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
        viewTarget = manager.targets[0];

        if (!viewTarget) manager.animationController.SwitchAnimState("Idle");
        else manager.animationController.SwitchAnimState("Move");
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        //如果敌人进入攻击范围，切换到攻击状态
        Minion[] targets = manager.GetOppenentInRange(status.range,status.minRange);
        if (targets != null)
        {
            manager.targets = targets;
            manager.TransitionState(MinionStateType.ATTACK);

            return;
        }
        manager.targets = manager.GetOppenentInRange(status.viewRange,0);

        //敌方消失或敌方离开索敌范围，切换回待机
        if (manager.targets == null ||
            Vector3.Distance(viewTarget.transform.position, manager.transform.position) >= status.viewRange)
        {
            manager.TransitionState(MinionStateType.IDLE);
            return;
        }
        viewTarget = manager.targets[0];
        manager.agent.speed = status.speed;
        manager.agent.SetDestination(viewTarget.transform.position);

        //限制范围
        if (manager.Info().minionType == MinionType.FRIEND)
        {
            Barrack barrack = manager.Barrack();
            if (Vector3.Distance(barrack.transform.position, manager.transform.position) >= barrack.Status().range)
            {
                //停止但持续索敌直到敌方（可能）进入兵营范围                
                if (viewTarget != null && Vector3.Distance(barrack.transform.position, viewTarget.transform.position) <= barrack.Status().range)
                {
                    manager.agent.speed = status.speed;
                    manager.animationController.SwitchAnimState("Move");
                } else {
                    manager.animationController.SwitchAnimState("Idle");
                    manager.agent.speed = 0;
                }
                return;
            }
        }
        manager.animationController.SwitchAnimState("Move");
    }
}
