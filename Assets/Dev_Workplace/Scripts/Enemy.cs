using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using UnityEngine.AI;
using Yunhao_Fight;

public class Enemy : Minion
{
    private void Start()
    {
        status.maxHealth = GetBaseProperty(1).health;
        status.health = status.maxHealth;

        status.attackMode = Info().attackMode;
        status.lockMode = Info().lockMode;
        status.fireInterval = Info().fireInterval;
        status.fireTime = Info().fireTime;
        status.speed = Info().speed;

        status.specialEffect = Info().specialEffect;


        status.secondSpEffect = Info().secondSpecialEffect;

        moveDestination = LevelManager.BaseDestination().position;
        status.effectBase = new Effect();
        status.takeDamageModifer = 1f;

        agent = this.GetComponent<NavMeshAgent>();

        //*Animation Controller Setups
        animationController = GetComponentInChildren<AnimationController>();
        animationController.InitializeAnimationClipPairs();

        states.Add(MinionStateType.IDLE, new MinionIdleState(this));
        states.Add(MinionStateType.INTERVAL, new MinionIntervalState(this));
        states.Add(MinionStateType.ATTACK, new MinionAttackState(this));
        states.Add(MinionStateType.VIEW, new MinionViewState(this));
        states.Add(MinionStateType.DIZZY, new MinionDizzyState(this));
        states.Add(MinionStateType.DYING, new MinionDyingState(this));
        TransitionState(MinionStateType.IDLE);

        //SendMessage("InitializeHealth", status.maxHealth);
    }
}
