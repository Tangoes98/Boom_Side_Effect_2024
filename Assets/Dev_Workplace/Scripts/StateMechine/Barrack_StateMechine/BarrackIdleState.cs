using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrackIdleState : IState
{
    Barrack manager;
    BarrackStatus status;
    public BarrackIdleState(Barrack manager)
    {
        this.manager = manager;
        this.status = (BarrackStatus)manager.Status();
    }

    float time;

    public void onEnter()
    {
        time = status.manufactureInterval;
    }

    public void onExit()
    {
        time = status.manufactureInterval;
    }

    public void onUpdate()
    {
        time -= Time.deltaTime;
        if (time < 0 && status.currentMinions.Count < status.maxMinionNum)
        {
            manager.TransitionState(BarrackStateType.SPAWN);
        }
    }

}
