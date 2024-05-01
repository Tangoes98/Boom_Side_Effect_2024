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


        //manager.targets = null; // ���ܲ���Ҫ
        manager.agent.speed = status.speed;

        //�ص�Ĭ��λ��
        manager.agent.SetDestination(manager.moveDestination);
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        //������˽��빥����Χ���л�������״̬
        Minion[] targets = manager.GetOppenentInRange(status.range);
        if (targets != null)
        {
            manager.targets = targets;
            manager.TransitionState(MinionStateType.ATTACK);

            return;
        }

        //������˽������з�Χ���л�������״̬
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
