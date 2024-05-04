using UnityEngine;

public class MinionDyingState : IState
{
    Minion manager;
    MinionStatus status;
    float _deathAnimationTimer;
    public MinionDyingState(Minion manager)
    {
        this.manager = manager;
        this.status = manager.status;
    }
    public void onEnter()
    {
        manager.animationController.SwitchAnimState("Dead");
        _deathAnimationTimer = 3f;

    }
    public void onExit()
    {

    }
    public void onUpdate()
    {
        _deathAnimationTimer -= Time.deltaTime;
        
        if (_deathAnimationTimer < 0)
        {
            _deathAnimationTimer = 0;
            if (manager.Info().minionType == MinionType.FRIEND) manager.Barrack().DestroyMinion(manager);
            else LevelEditor.Instance.EnemyDie(manager);
        }
    }
}
