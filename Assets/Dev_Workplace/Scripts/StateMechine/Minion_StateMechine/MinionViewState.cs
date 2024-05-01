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
        //*Set Animation State
        manager.animationController.SwitchAnimState("Move");

        viewTarget = manager.targets[0];
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

        //�з���ʧ��з��뿪���з�Χ���л��ش���
        if (manager.targets == null ||
            Vector3.Distance(viewTarget.transform.position, manager.transform.position) >= status.viewRange)
        {
            manager.TransitionState(MinionStateType.IDLE);
            return;
        }
        manager.agent.SetDestination(viewTarget.transform.position);

        //���Ʒ�Χ
        if (manager.Info().minionType == MinionType.FRIEND)
        {
            Barrack barrack = manager.Barrack();
            if (Vector3.Distance(barrack.transform.position, manager.transform.position) >= barrack.Status().range)
            {
                //ֹͣ����������ֱ���з������ܣ������Ӫ��Χ
                manager.agent.speed = 0;
                if (viewTarget != null && Vector3.Distance(barrack.transform.position, viewTarget.transform.position) <= barrack.Status().range)
                {
                    manager.agent.speed = status.speed;
                }
            }
        }
    }
}
