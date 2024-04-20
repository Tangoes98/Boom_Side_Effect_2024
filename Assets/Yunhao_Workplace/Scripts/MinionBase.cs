using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace Yunhao_Fight
{
    public class MinionBase : MonoBehaviour
    {
        enum MinionType
        {
            Enemy,Freind
        }
        enum MinionState
        {
            Idle, View, Combat, Dying //for Enemy, Idle×´Ì¬¼´³ÖÐøÑØÂ·¾¶ÒÆ¶¯
        }
        //enum damageType;

        #region =============== Variables =======================
        [SerializeField] MinionType _minionType;
        [SerializeField] string _code;
        LayerMask _oppenentLayer;

        [SerializeField] MinionState _minionState;

        [Header("Health")]
        [SerializeField] float _maxHealth;
        float _health;

        [Header("Movement")]   
        [SerializeField] protected float _moveSpeed;
        protected Transform _idleDestination;

        [Header("Attack")]
        [SerializeField] float _attackRange;
        [SerializeField] float _viewRange;
        [SerializeField] float _damage;
        //[SerializeField] float _attackSpeed;
        [SerializeField] float _attackTimer;
        MinionBase _viewTarget;
        MinionBase _attackTarget;
        Coroutine _attacking;

        protected NavMeshAgent _agent;

        #endregion
        #region =================== Public ============================








        #endregion
        #region ================ MonoBehaviour =======================
        private void Start()
        {
            SendMessage("InitializeHealth", _maxHealth);

            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _moveSpeed;

            _oppenentLayer = (_minionType == MinionType.Enemy) ? LevelManager.FriendLayer() : LevelManager.EnemyLayer();
            setIdleDestination();

            SwitchMinionState(MinionState.Idle);
            StartCoroutine(Combat());
        }
        private void Update()
        {

            switch (_minionState)
            {
                case MinionState.Idle:
                    _viewTarget = GetOpponentInRange(_viewRange);
                    if (_viewTarget != null) SwitchMinionState(MinionState.View);

                    break;
                case MinionState.View:
                    View(_viewTarget);

                    break;
                case MinionState.Combat:
                    
                    break;
                case MinionState.Dying:

                    break;
            }
        }
        #endregion
        #region =============== Methods =======================
        protected virtual void setIdleDestination() { }
        MinionBase GetOpponentInRange(float range)
        {
            Collider[] attackTargets = Physics.OverlapSphere(this.transform.position, range, _oppenentLayer);
            if (attackTargets.Length > 0 && attackTargets[0].gameObject.TryGetComponent<MinionBase>(out MinionBase oppenent))
            {
                return oppenent;
            }
            else return null;
        }
        protected virtual void View(MinionBase oppenent)
        {
            //Debug.Log("view " + oppenent.name);
            if(oppenent == null)
            {
                SwitchMinionState(MinionState.Idle);
                return;
            }

            _agent.SetDestination(oppenent.transform.position);
            
            //µÐ·½Àë¿ªË÷µÐ·¶Î§ÄÚ
            if (Vector3.Distance(oppenent.transform.position, this.transform.position) >=_viewRange)
            {
                SwitchMinionState(MinionState.Idle);
            }
        }
        IEnumerator Combat()
        {
            yield return null;

            while (_minionState!=MinionState.Dying)
            {
                if (_attackTarget == null)
                {
                    _attackTarget = GetOpponentInRange(_attackRange);
                    yield return null;
                }
                else
                {
                    SwitchMinionState(MinionState.Combat);
                    while (_attackTarget != null && Vector3.Distance(_attackTarget.transform.position, this.transform.position) < _attackRange)
                    {
                        transform.LookAt(_attackTarget.transform.position);
                        if (_attacking == null) _attacking = StartCoroutine(Attack());
                        yield return null;
                    }
                    SwitchMinionState(MinionState.Idle);
                    _attackTarget = GetOpponentInRange(_attackRange);
                }
                yield return null;
            }
        }

        IEnumerator Attack()
        {
            _attackTarget.SendMessage("TakeDamage", _damage);
            yield return new WaitForSeconds(_attackTimer);
            _attacking = null;
        }
        


        void SwitchMinionState(MinionState state)
        {
            _minionState = state;
            switch (state)
            {
                case MinionState.Idle:
                    _viewTarget = _attackTarget = null;
                    _agent.SetDestination(_idleDestination.position);
                    _agent.speed = _moveSpeed;
                    break;
                case MinionState.View:
                    _agent.speed = _moveSpeed;
                    break;
                case MinionState.Combat:
                    _agent.speed = 0;
                    break;
            }
            
        }

        void InitializeHealth(float health)
        {
            _health = health;
        }
        void TakeDamage(float damage)
        {
            _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
            if (_health <= 0)
            {
                Destroy(gameObject);//Dead
            }
        }

        #endregion
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _viewRange);
        }


    }
}