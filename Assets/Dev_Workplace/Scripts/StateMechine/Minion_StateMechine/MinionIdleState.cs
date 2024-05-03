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

        //manager.targets = null; // ���ܲ���Ҫ
        manager.agent.speed = status.speed;
        manager.targets = null;

        //�ص�Ĭ��λ��
        manager.agent.SetDestination(manager.moveDestination);
        manager.animationController.SwitchAnimState("Move");
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        //������˽��빥����Χ���л�������״̬
        Minion[] targets = manager.GetOppenentInRange(status.range,status.minRange);
        if (targets != null)
        {
            manager.targets = targets;
            manager.TransitionState(MinionStateType.ATTACK);

            return;
        }

        //������˽������з�Χ���л�������״̬
        targets = manager.GetOppenentInRange(status.viewRange,0);
        if (targets != null)
        {
            manager.targets = targets;
            manager.TransitionState(MinionStateType.VIEW);

            return;
        }

        //*Stop move animtion check
        if (Vector3.Distance(manager.moveDestination, manager.transform.position) > manager.agent.stoppingDistance) 
            manager.animationController.SwitchAnimState("Move");
        else manager.animationController.SwitchAnimState("Idle");

    }
}
