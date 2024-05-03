using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class MinionDizzyState : IState
{
    Minion manager;
    MinionStatus status;

    public MinionDizzyState(Minion manager)
    {
        this.manager = manager;
        this.status = manager.status;
    }

    public void onEnter()
    {
        manager.agent.speed = 0;
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        if (status.gotEffects[SpecialEffect.DIZZY].lastTime < 0.1f) manager.TransitionState(MinionStateType.IDLE);
    }

}
