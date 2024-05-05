using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class MinionDizzyState : IState
{
    Minion manager;

    Vector3 _freezePos;

    public MinionDizzyState(Minion manager)
    {
        this.manager = manager;
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
        if (!manager.status.gotEffects.ContainsKey(SpecialEffect.DIZZY)) manager.TransitionState(MinionStateType.IDLE);
    }
    public string Type() => MinionStateType.DIZZY.ToString();
}
