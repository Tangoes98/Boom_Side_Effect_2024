using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Yunhao_Fight;

public class Minion : MonoBehaviour {

    public string code;

    public int level;
    public ModifierType modifierType; // BUFF/DEBUFF/NONE, 可能会根据此变量，改变小兵外观（大小）

    [SerializeField]
    public MinionStatus status; // 只用这里的数据
    [SerializeField]
    private MinionBase baseInfo; // 不要直接调用！初始数据

    public MinionBase Info() => baseInfo;

    private Barrack _parent;
    public Barrack Barrack() => _parent;

    public Minion[] targets{get; set;}
    public Vector3 moveDestination { get; set; }
    public NavMeshAgent agent { get; set; }

    protected IState currentState;
    protected Dictionary<MinionStateType, IState> states = new Dictionary<MinionStateType, IState>();

    [SerializeField] TextMeshProUGUI stateLabel;

    public void Initialize(Barrack parent) {
        _parent = parent;
        level = parent.level;
        float modifier = GetMinionDamageAndHealthModifier(parent.Unstability);
        MinionProperty prop = GetBaseProperty(level);

        status.maxHealth = modifier * prop.health;
        status.health = status.maxHealth;
        status.damage = modifier * prop.damage;

        status.range = prop.range;
        status.viewRange = prop.viewRange;
        status.attackMode = baseInfo.attackMode;
        status.lockMode = baseInfo.lockMode;
        status.fireInterval = baseInfo.fireInterval;
        status.fireTime = baseInfo.fireTime;
        

        status.specialEffect = baseInfo.specialEffect;
        status.specialEffectModifier = prop.specialEffectModifier;
        status.specialEffectLastTime = prop.specialEffectLastTime;

        status.secondSpEffect = baseInfo.secondSpecialEffect;
        status.secondSpEffectModifier = prop.secondSpEffectModifier;
        status.secondSpEffectLastTime = prop.secondSpEffectLastTime;

        status.gotEffects = new Dictionary<SpecialEffect, Effect>();
        status.takeDamageModifer = 1f;

        moveDestination = baseInfo.minionType == MinionType.ENEMY? LevelManager.BaseDestination().position:_parent.minionDestination;

        status.speed = baseInfo.speed;//也许要改

        createAgent();

        states.Add(MinionStateType.IDLE, new MinionIdleState(this));
        states.Add(MinionStateType.INTERVAL, new MinionIntervalState(this));
        states.Add(MinionStateType.ATTACK, new MinionAttackState(this));
        states.Add(MinionStateType.VIEW, new MinionViewState(this));
        states.Add(MinionStateType.DYING, new MinionDyingState(this));

        TransitionState(MinionStateType.IDLE);
        
        SendMessage("InitializeHealth", status.maxHealth);
    }

    void Update()
    {
        UpdateEffect();
        currentState.onUpdate();
    }

    protected MinionProperty GetBaseProperty(int level)  => baseInfo.levelRelatedProperties.Where(p=>p.level==level).First();

    protected float GetMinionDamageAndHealthModifier(int unstability) => ArchiLinkManager.Instance.GetModifier(unstability, out modifierType);

    public void SelfDestroy() {
        _parent.DestroyMinion(this);
    }

    public Minion[] GetOppenentInRange(float range)
    {
        LayerMask OppenetLayer = (baseInfo.minionType == MinionType.FRIEND) ? LevelManager.EnemyLayer() : LevelManager.FriendLayer();
        Collider[] attackTargets = Physics.OverlapSphere(this.transform.position, range, OppenetLayer);
        Minion[] minions = attackTargets.Select(collider => collider.gameObject.GetComponent<Minion>())
                               .Where(enemy => enemy != null)
                               .ToArray();
        if (attackTargets.Length > 0)
        {
            return minions;
        }
        else return null;
    }
    public void TransitionState(MinionStateType type)
    {
        if (currentState != null)
        {
            currentState.onExit();
        }
        currentState = states[type];
        currentState.onEnter();

        stateLabel.text=type.ToString();
    }
    
    public virtual void Attack(Minion target)
    {
        target.TakeEffect(status.specialEffect,level);
        target.TakeEffect(status.secondSpEffect, level);
        target.TakeDamage(status.damage);
    }

    void TakeDamage(float damage)
    {
        damage *= status.takeDamageModifer;
        status.health = Mathf.Clamp(status.health - damage, 0, status.maxHealth);
        GetComponentInChildren<Slider>().value = status.health;
        if (status.health <= 0)
        {
            TransitionState(MinionStateType.DYING);
        }
    }
    void TakeEffect(SpecialEffect type, int level)
    {
        if (type == SpecialEffect.NONE) return;
        if (!status.gotEffects.TryGetValue(type, out Effect existingeffect))
        {
            status.gotEffects.Add(type, new Effect());
        }
        status.gotEffects[type].AddEffect(type, level);
    }
    void UpdateEffect()
    {
        foreach(var gotEffect in status.gotEffects)
        {
            SpecialEffect specialEffect = gotEffect.Key;
            Effect effect = gotEffect.Value;

            effect.UpdateEffect(Time.deltaTime);
            switch (specialEffect)
            {
                case SpecialEffect.WEAK:
                    status.takeDamageModifer = 1 + effect.effect.modifier;
                    break;
                case SpecialEffect.POSION:
                    SendMessage("TakeDamage",effect.effect.modifier*status.maxHealth);
                    break;
                case SpecialEffect.SLOW:
                    agent.speed=status.speed=agent.speed* effect.effect.modifier;
                    break;
                case SpecialEffect.DIZZY:
                    TransitionState(MinionStateType.DIZZY);
                    break;
                    
            }
        }
    }
    void createAgent()
    {
        this.transform.position=_parent.GetSpawnPosition();

        agent = gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, status.viewRange);
    }


}
