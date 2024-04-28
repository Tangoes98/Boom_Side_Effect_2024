using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttackState : IState
{
    Minion manager;
    MinionStatus status;

    Minion attackTarget;
    public MinionAttackState(Minion manager)
    {
        this.manager = manager;
        this.status = manager.status;
    }

    float timer;

    public void onEnter()
    {
        attackTarget = manager.targets[0];
        timer = status.fireTime;
    }
    public void onExit()
    {
         manager.Attack(attackTarget);
    }
    public void onUpdate()
    {
        manager.transform.LookAt(attackTarget.transform.position);

        timer -= status.fireTime;
        
        //如果失去了攻击目标，回到待机
        if (manager.targets == null ||
            Vector3.Distance(attackTarget.transform.position, manager.transform.position) >= status.range)
        {
            manager.TransitionState(MinionStateType.IDLE);
            return;
        }

        if (timer <= 0)
        {
            manager.Attack(attackTarget);
            manager.TransitionState(MinionStateType.INTERVAL);
        }
    }
}
