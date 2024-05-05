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
    BuildingArtAssets _artAsset;
    public DefenseTowerAttackState(DefenseTower manager)
    {
        this.manager = manager;
        this.status = (DefenseTowerStatus)manager.Status();
        this.targets = manager.targets;
    }

    public void onEnter()
    {
        _artAsset = manager.GetComponentInChildren<BuildingArtAssets>();
        if (!_artAsset) return;
        _artAsset.AttackVFX.SetActive(true);

        manager.Attack();
    }

    public void onExit()
    {
        _artAsset.AttackVFX.SetActive(false);
    }

    public void onUpdate()
    {
        if (!_artAsset.IsAOEAttack)
        {
            if (manager.targets != null && manager.targets.Length > 0 && manager.targets[0] != null)
            {
                _artAsset.AttackVFX.transform.LookAt(manager.targets[0].transform.position);
            }
        }

        //if(!manager.checkTarget()) manager.TransitionState(DefenseTowerStateType.IDLE);
    }
    public string Type() => DefenseTowerStateType.ATTACK.ToString();
}
