using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yunhao_Fight;

public class DefenseTowerAttackState : IState
{
    DefenseTower manager;
    DefenseTowerStatus status;
    Enemy[] targets;
    public DefenseTowerAttackState(DefenseTower manager)
    {
        this.manager = manager;
        this.status = (DefenseTowerStatus)manager.Status();
        this.targets = manager.targets;
    }

    public void onEnter()
    {
        manager.Attack();
    }

    public void onExit()
    {

    }

    public void onUpdate()
    {
        if(!manager.checkTarget()) manager.TransitionState(DefenseTowerStateType.IDLE);
    }

}
