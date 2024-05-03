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
        //������˽��빥����Χ���л�������״̬
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

        //�з���ʧ��з��뿪���з�Χ���л��ش���
        if (manager.targets == null ||
            Vector3.Distance(viewTarget.transform.position, manager.transform.position) >= status.viewRange)
        {
            manager.TransitionState(MinionStateType.IDLE);
            return;
        }
        viewTarget = manager.targets[0];
        
        //���Ʒ�Χ
        if (manager.Info().minionType == MinionType.FRIEND)
        {
            Barrack barrack = manager.Barrack();
            if (Vector3.Distance(barrack.transform.position, manager.transform.position) >= barrack.Status().range)
            {
                //ֹͣ����������ֱ���з������ܣ������Ӫ��Χ                
                if (viewTarget != null && Vector3.Distance(barrack.transform.position, viewTarget.transform.position) <= barrack.Status().range)
                {
                    manager.agent.SetDestination(viewTarget.transform.position);
                    manager.agent.speed = status.speed;

                    manager.animationController.SwitchAnimState("Move");
                } else {
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
}
