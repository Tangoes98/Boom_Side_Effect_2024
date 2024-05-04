using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class MinionDizzyState : IState
{
    Minion manager;
    MinionStatus status;

    Vector3 _freezePos;

    public MinionDizzyState(Minion manager)
    {
        this.manager = manager;
        this.status = manager.status;
    }

    public void onEnter()
    {
        manager.agent.speed = 0;
        _freezePos = manager.transform.position;
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        manager.transform.position = _freezePos;
        if (!status.gotEffects.ContainsKey(SpecialEffect.DIZZY)) manager.TransitionState(MinionStateType.IDLE);
    }

}
