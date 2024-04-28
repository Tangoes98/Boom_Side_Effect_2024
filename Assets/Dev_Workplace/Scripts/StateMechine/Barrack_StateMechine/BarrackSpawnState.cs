using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrackSpawnState : IState
{
    Barrack manager;
    BarrackStatus status;
    public BarrackSpawnState(Barrack manager)
    {
        this.manager = manager;
        this.status = (BarrackStatus)manager.Status();
    }

    float time;

    public void onEnter()
    {
        time = status.manufactureTime;
    }

    public void onExit()
    {
        manager.ManufactureMinion()
    }

    public void onUpdate()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            manager.TransitionState(BarrackStateType.IDLE);
        }
    }

}
