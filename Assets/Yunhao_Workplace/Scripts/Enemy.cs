using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Yunhao_Fight
{
    public class Enemy : MonoBehaviour
    {
        //enum damageType;

        #region =============== Variables =======================
        [SerializeField] string code;

        //[Header("DEBUG_VIEW")]

        [Header("Health")]
        [SerializeField] float _maxHealth;
        float _health;

        [Header("Movement")]
        [SerializeField] float _moveSpeed;

        [Header("Attack")]
        [SerializeField] float _attackRange;
        [SerializeField] float _viewRange;
        [SerializeField] float _damage;
        [SerializeField] float _attackSpeed;
        float _attackTimer;
        bool _isMelee;

        NavMeshAgent _agent;

        #endregion
        #region =================== Public ============================








        #endregion
        #region ================ MonoBehaviour =======================
        private void Start()
        {
            SendMessage("InitializeHealth", _maxHealth);

            _agent = GetComponent<NavMeshAgent>();
            addTarget(LevelManager.EnemyTarget());


        }
        //private void Update()
        //{
        //    CheckUnitListEmptiness();
        //}
        #endregion
        #region =============== Methods =======================

        void addTarget(Transform target)
        {
            _agent.SetDestination(target.position);
        }
        void InitializeHealth(float health)
        {
            Debug.Log("Set Health");
            _health =health;
        }
        void TakeDamage(float damage)
        {
            _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
            if(_health <= 0)
            {
                Destroy(gameObject);//Dead
            }
        }


        #endregion

    }
}