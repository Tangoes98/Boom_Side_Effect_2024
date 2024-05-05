using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefenseTowerIntervalState : IState
{
    DefenseTower manager;
    DefenseTowerStatus status;
    public DefenseTowerIntervalState(DefenseTower manager)
    {
        this.manager = manager;
        this.status = (DefenseTowerStatus)manager.Status();
    }

    float timer;
    public void onEnter()
    {
        timer = status.fireInterval;
    }

    public void onExit()
    {
        timer = status.fireInterval;
    }

    public void onUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (!manager.checkTarget()) {
                manager.TransitionState(DefenseTowerStateType.IDLE);
                return;
            }
            manager.TransitionState(DefenseTowerStateType.ATTACK);
        }
    }
    public string Type() => DefenseTowerStateType.INTERVAL.ToString();

}
