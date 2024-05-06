using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Yunhao_Fight;

public class Minion : MonoBehaviour,IHealthBar
{

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

    [field:SerializeField] public Minion[] targets { get; set; }
    public Vector3 moveDestination { get; set; }
    public NavMeshAgent agent { get; set; }

    protected IState currentState;
    protected Dictionary<MinionStateType, IState> states = new Dictionary<MinionStateType, IState>();

    //[SerializeField] TextMeshProUGUI stateLabel;
    [SerializeField] string _stateLabel;
    public AnimationController animationController;

    private float _poisonUpdateTimer = 1;

    public AoeAttack aoeAttack {private set; get; }

    public MainBase mainBase {get;set;}

    private bool _rbResetting = false;
    private float _stuckTimer = 2;
    private int _stuckCount = 3;
    private Vector3 _lastPos = Vector3.zero;
    
    private void Awake() {
        aoeAttack = GetComponent<AoeAttack>();
    }

    public void Initialize(Barrack parent)
    {
        _parent = parent;
        level = parent.level;
        float modifier = GetMinionDamageAndHealthModifier(parent.Unstability);
        MinionProperty prop = GetBaseProperty(level);

        status.maxHealth = modifier * prop.health;
        status.health = status.maxHealth;
        status.damage = modifier * prop.damage;
        
        status.minRange = baseInfo.minRange;
        status.range = prop.range;
        status.viewRange = math.max(prop.range, prop.viewRange);
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

        status.effectBase = new Effect();
        status.takeDamageModifer = 1f;

        moveDestination = baseInfo.minionType == MinionType.ENEMY ? LevelManager.BaseDestination().position : _parent.minionDestination;

        status.speed = baseInfo.speed;//也许要改

        //*Animation Controller Setups
        animationController = GetComponentInChildren<AnimationController>();
        animationController.InitializeAnimationClipPairs();

        createAgent();

        states.Add(MinionStateType.IDLE, new MinionIdleState(this));
        states.Add(MinionStateType.INTERVAL, new MinionIntervalState(this));
        states.Add(MinionStateType.ATTACK, new MinionAttackState(this));
        states.Add(MinionStateType.VIEW, new MinionViewState(this));
        states.Add(MinionStateType.DIZZY, new MinionDizzyState(this));
        states.Add(MinionStateType.DYING, new MinionDyingState(this));

        TransitionState(MinionStateType.IDLE);

    }

    void Update()
    {
        UpdateEffect();
        currentState.onUpdate();
        if(!_rbResetting) {
            CheckIfStuck();
        }
    }

    protected MinionProperty GetBaseProperty(int level) => baseInfo.levelRelatedProperties.Where(p => p.level == level).First();

    protected float GetMinionDamageAndHealthModifier(int unstability) => ArchiLinkManager.Instance.GetModifier(unstability, out modifierType);

    public void SelfDestroy()
    {
        _parent.DestroyMinion(this);
    }

    public Minion[] GetOppenentInRange(float range, float minRange, out MainBase baseInRange)
    {
        LayerMask OppenetLayer = (baseInfo.minionType == MinionType.FRIEND) ? LevelManager.EnemyLayer() : LevelManager.FriendLayer();
        Collider[] attackTargets = Physics.OverlapSphere(this.transform.position, range, OppenetLayer);
        baseInRange = baseInfo.minionType == MinionType.FRIEND? null : 
                        attackTargets.Select(collider => collider.gameObject.GetComponent<MainBase>())
                            .Where(b => b != null)
                            .FirstOrDefault();
        
        Minion[] minions = attackTargets.Select(collider => collider.gameObject.GetComponent<Minion>())
                               .Where(enemy => enemy != null)
                               .Where(m => minRange==0 || Vector3.Distance(this.transform.position, m.transform.position)> minRange)
                               .OrderBy(m => Vector3.Distance(this.transform.position, m.transform.position))
                               .ToArray();
        
        if(baseInfo.minionType == MinionType.ENEMY && baseInRange!=null && 
                (minions.Length==0 || 
                    Vector3.Distance(this.transform.position, minions[0].transform.position) > Vector3.Distance(this.transform.position, baseInRange.transform.position))) {
            return null;
        }

        if (minions.Length > 0)
        {
            return minions;
        }
        else return null;
    }
    public void TransitionState(MinionStateType type)
    {
        if(currentState!=null && (currentState.Type() == MinionStateType.DYING.ToString() || currentState.Type() == type.ToString())) {
            return;
        }
        
        if (currentState != null)
        {
            currentState.onExit();
        }
        currentState = states[type];
        currentState.onEnter();

        //stateLabel.text = type.ToString();
        _stateLabel = type.ToString();
    }

    public virtual void Attack(Minion target)
    {
        target.TakeEffect(status.specialEffect, status.specialEffectModifier,status.specialEffectLastTime);
        target.TakeEffect(status.secondSpEffect, status.secondSpEffectModifier, status.secondSpEffectLastTime);
        if(target.status.health<=status.damage*status.takeDamageModifer && aoeAttack!=null && aoeAttack.type==AoeType.CIRCLE_CENTER_ENEMY_DIE) {
            aoeAttack.TriggerAOE(target.transform.position,status.takeDamageModifer); 
        }
        target.TakeDamage(status.damage);
        
    }

    public void TakeDamage(float damage)
    {
        if(_stateLabel== MinionStateType.DYING.ToString()) {
            return;
        }
        damage *= status.takeDamageModifer;
        
        //GetComponentInChildren<Slider>().value = status.health;
        if (status.health - damage <= 0)
        {
            TransitionState(MinionStateType.DYING);
        }
        status.health = Mathf.Clamp(status.health - damage, 0, status.maxHealth);
    }
    public void TakeEffect(SpecialEffect type, float modifier, float lastTime)
    {
        if (type == SpecialEffect.NONE) return;
        status.effectBase.AddEffect(type, modifier,lastTime);
    }
    void UpdateEffect()
    {
        List<SpecialEffect> cancelledEffects = status.effectBase.UpdateEffect(Time.deltaTime);
        ResetEffect(cancelledEffects);

        foreach (var gotEffect in status.gotEffects)
        {       
            SpecialEffect specialEffect = gotEffect.Key;
            EffectStruct effect = gotEffect.Value;

            switch (specialEffect)
            {
                case SpecialEffect.WEAK:
                    status.takeDamageModifer = 1 + effect.modifier;
                    break;
                case SpecialEffect.POSION:
                    _poisonUpdateTimer-=Time.deltaTime;
                    if(_poisonUpdateTimer<=0) {
                        SendMessage("TakeDamage", effect.modifier * status.maxHealth);
                        _poisonUpdateTimer = 1;
                    }
                    break;
                case SpecialEffect.SLOW:
                    status.speed = Info().speed + effect.modifier;
                    if(agent.speed>0) agent.speed = status.speed;
                    break;
                case SpecialEffect.DIZZY:
                    TransitionState(MinionStateType.DIZZY);
                    break;

            }
        }
    }

    void ResetEffect(List<SpecialEffect> cancelledEffects) {
        foreach (var e in cancelledEffects) {
            if(e == SpecialEffect.SLOW) {
                status.speed = Info().speed;
                if(agent.speed>0) agent.speed = status.speed;
            } else if(e==SpecialEffect.WEAK) {
                status.takeDamageModifer = 1f;
            } else if(e==SpecialEffect.POSION) {
                _poisonUpdateTimer = 1f;
            }
        }
    }
    void createAgent()
    {
        this.transform.position = _parent.GetSpawnPosition();
        agent = GetComponent<NavMeshAgent>();
        // agent = gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;

        //* Editing Minion Agent Info
        // agent.stoppingDistance = 3f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, status.viewRange);
    }

    public float GetHealth() => status.health;

    public float GetMaxHealth() => status.maxHealth;

    private void CheckIfStuck() {
        if(!agent.isOnNavMesh) {
            NavMeshHit hit;
            if(NavMesh.SamplePosition(transform.position, out hit, status.range, NavMesh.AllAreas)) {
                transform.position = hit.position;
            } else {
                LevelEditor.Instance.EnemyDie(this);
            }

            return;
        }

        if(baseInfo.minionType == MinionType.ENEMY && 
            currentState.Type()==MinionStateType.IDLE.ToString() &&
            Vector3.Distance(transform.position, _lastPos) <0.1f) {
            _stuckTimer-=Time.deltaTime;
            
        }  else {
            _stuckTimer=2;
            _stuckCount=3;
        }
        _lastPos = transform.position;
            
        if(_stuckTimer<=0) {
            _rbResetting = true;
            _stuckTimer = 2;
            if(_stuckCount<0) {
                LevelEditor.Instance.EnemyDie(this);
                return;
            }
            StartCoroutine(ResetRigidBodyConstraints());
            _stuckCount--;
        }     

    }
    private IEnumerator ResetRigidBodyConstraints() {
        var rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        yield return new WaitForNextFrameUnit();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rbResetting = false;
    }
}
