using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yunhao_Fight;

namespace Yunhao_Fight
{
    public class LevelManager : MonoBehaviour
    {
        #region =============== Instance =======================
        static LevelManager Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple instances occured");
                Destroy(Instance);
            }
            Instance = this;
        }
        #endregion
        #region =============== Variables =======================
        [SerializeField] LayerMask _friendLayer;
        [SerializeField] LayerMask _enemyLayer;
        [SerializeField] Transform _baseDestination;
        #endregion
        #region =================== Public ============================
        public static LayerMask FriendLayer() => Instance._friendLayer;
        public static LayerMask EnemyLayer() => Instance._enemyLayer;
        public static Transform BaseDestination() => Instance._baseDestination;
        #endregion
        #region ================ MonoBehaviour =======================

        #endregion
        #region =============== Methods =======================


        #endregion

    }
}
