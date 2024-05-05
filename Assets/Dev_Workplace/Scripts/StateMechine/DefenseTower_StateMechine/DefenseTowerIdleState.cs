using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yunhao_Fight;

public class DefenseTowerIdleState : IState
{
    DefenseTower manager;
    ArchitectStatus status;
    public DefenseTowerIdleState(DefenseTower manager)
    {
        this.manager = manager;
        this.status = (DefenseTowerStatus)manager.Status();
    }
    public void onEnter()
    {

    }
    public void onExit()
    {
        
    }
    public void onUpdate()
    {
        Enemy[] targets = manager.GetEnemyInRange();
        if (targets!= null)
        {
            manager.targets=targets;
            manager.TransitionState(DefenseTowerStateType.ATTACK);
        }
    }
    public string Type() => DefenseTowerStateType.IDLE.ToString();
}
