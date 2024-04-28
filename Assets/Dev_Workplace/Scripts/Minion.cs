using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
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
    public Transform moveDestination;
    public NavMeshAgent agent;

    IState currentState;
    Dictionary<MinionStateType, IState> states = new Dictionary<MinionStateType, IState>();

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
        moveDestination = baseInfo.minionType == MinionType.ENEMY? LevelManager.BaseDestination():_parent.minionDestination;

        agent = this.GetComponent<NavMeshAgent>();

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
        currentState.onUpdate();
    }

    private MinionProperty GetBaseProperty(int level)  => baseInfo.levelRelatedProperties.Where(p=>p.level==level).First();

    private float GetMinionDamageAndHealthModifier(int unstability) => ArchiLinkManager.Instance.GetModifier(unstability, out modifierType);

    public void SelfDestroy() {
        _parent.DestroyMinion(this);
    }

    public Minion[] GetOppenentInRange(float range)
    {
        Collider[] attackTargets = Physics.OverlapSphere(this.transform.position, range, LevelManager.EnemyLayer());
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
    }
    
    public virtual void Attack(Minion target)
    {
        //targets[0].gameObject.SendMessage("TakeDamage", GetDamage(out modifiertype));//攻击最近的目标
    }

    void InitializeHealth(float health)
    {
        status.health = health;
    }
    void TakeDamage(float damage)
    {
        status.health = Mathf.Clamp(status.health - damage, 0, status.maxHealth);
        if (status.health <= 0)
        {
            TransitionState(MinionStateType.DYING);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, status.viewRange);
    }


}
