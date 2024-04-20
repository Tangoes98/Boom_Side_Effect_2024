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
        [SerializeField] Transform _enemyTarget;
        [SerializeField] LayerMask _enemyLayer;
        #endregion
        #region =================== Public ============================
        public static Transform EnemyTarget() => Instance._enemyTarget;
        public static LayerMask EnemyLayer() => Instance._enemyLayer;
        #endregion
        #region ================ MonoBehaviour =======================
        //private void Start()
        //{
        //    GetAllUnitIntoUnitList();

        //}
        //private void Update()
        //{
        //    CheckUnitListEmptiness();
        //}
        #endregion
        #region =============== Methods =======================

        //void GetAllUnitIntoUnitList()
        //{
        //    T_Unit[] units = _unitSpawner.GetComponentsInChildren<T_Unit>();
        //    foreach (var unit in units)
        //    {
        //        if (unit.G_IsEnemyUnit()) _enemyList.Add(unit);
        //        else _friendList.Add(unit);
        //    }
        //}
        //void CheckUnitListEmptiness()
        //{
        //    if (IsUnitListEmpty(_enemyList) || IsUnitListEmpty(_friendList))
        //    {
        //        // Game Over
        //        Event_GameOver?.Invoke();
        //    }
        //    if (IsUnitListEmpty(_enemyList))
        //    {
        //        // Player wins

        //    }
        //    if (IsUnitListEmpty(_friendList))
        //    {
        //        // Enemy wins

        //    }


        //}


        //bool IsUnitListEmpty(List<T_Unit> untiList)
        //{
        //    if (untiList.Count < 1) return true;
        //    else return false;
        //}















        #endregion

    }
}
