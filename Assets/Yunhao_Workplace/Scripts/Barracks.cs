using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Yunhao_Fight
{
    public class Barracks : MonoBehaviour
    {
        #region =============== Variables =======================
        [SerializeField] float _attackRange;

        #endregion
        #region =================== Public ============================
        public float AttackRange() => _attackRange;
        #endregion
        #region ================ MonoBehaviour =======================

        #endregion
        #region =============== Methods =======================

        #endregion
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
