using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Yunhao_Fight
{
    public class MinionBase : MonoBehaviour
    {
        //enum damageType;
        #region =============== Instance =======================
        public static MinionBase Instance;
            //private void Awake()
            //{
            //    if (Instance != null)
            //    {
            //        Debug.LogError("Multiple instances occured");
            //        Destroy(Instance);
            //    }
            //    Instance = this;

            //}
            #endregion

        #region =============== Variables =======================
        [SerializeField] string code;

        //[Header("DEBUG_VIEW")]

        [Header("====Field_value_required=====")]
        [SerializeField] bool _isEnemy;
        
        [Header("health")]
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
        #endregion
        #region =============== Methods =======================

















        #endregion

    }
}