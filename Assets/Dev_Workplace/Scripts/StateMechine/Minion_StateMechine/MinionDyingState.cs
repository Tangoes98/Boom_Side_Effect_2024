using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDyingState : IState
{
    Minion manager;
    MinionStatus status;
    public MinionDyingState(Minion manager)
    {
        this.manager = manager;
        this.status = manager.status;
    }
    public void onEnter()
    {
        manager.Barrack().DestroyMinion(manager);
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
    }
}
