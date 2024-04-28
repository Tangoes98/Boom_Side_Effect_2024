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
    public void onEnter()
    {
        timer = status.fireInterval;
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            manager.TransitionState(MinionStateType.ATTACK);
        }
    }
}
