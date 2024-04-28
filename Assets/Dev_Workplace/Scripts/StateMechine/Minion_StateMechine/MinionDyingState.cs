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
        if(manager.Info().minionType==MinionType.FRIEND) manager.Barrack().DestroyMinion(manager);
        else LevelEditor.Instance.EnemyDie(manager);
    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
    }
}
