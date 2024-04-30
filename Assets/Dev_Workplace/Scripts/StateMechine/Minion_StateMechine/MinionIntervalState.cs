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
        attackTarget = manager.targets[0];
        timer = status.fireInterval;
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        timer -= Time.deltaTime;

        //如果失去了攻击目标，回到待机
        if (attackTarget == null ||
            Vector3.Distance(attackTarget.transform.position, manager.transform.position) >= status.range)
        {
            manager.TransitionState(MinionStateType.IDLE);
            return;
        }

        if (timer <= 0)
        {
            manager.TransitionState(MinionStateType.ATTACK);
        }
    }
}
