using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionIdleState : IState
{
    Minion manager;
    MinionStatus status;
    public MinionIdleState(Minion manager)
    {
        this.manager = manager;
        this.status = manager.status;
    }
    public void onEnter()
    {
        //*Set Animation State
        manager.animationController.SwitchAnimState("Move");


        //manager.targets = null; // 可能不需要
        manager.agent.speed = status.speed;

        //回到默认位置
        manager.agent.SetDestination(manager.moveDestination);
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        //如果敌人进入攻击范围，切换到攻击状态
        Minion[] targets = manager.GetOppenentInRange(status.range);
        if (targets != null)
        {
            manager.targets = targets;
            manager.TransitionState(MinionStateType.ATTACK);

            return;
        }

        //如果敌人进入索敌范围，切换到索敌状态
        targets = manager.GetOppenentInRange(status.viewRange);
        if (targets != null)
        {
            manager.targets = targets;
            manager.TransitionState(MinionStateType.VIEW);
        }

        //*Stop move animtion check
        if (Vector3.Distance(manager.moveDestination, manager.transform.position) > manager.agent.stoppingDistance) return;
        else manager.animationController.SwitchAnimState("Idle");

    }
}
