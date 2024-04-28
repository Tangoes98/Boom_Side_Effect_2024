using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Yunhao_Fight
{
    public class Enemy : MinionBase
    {
        //enum damageType;

        #region =============== Variables =======================

        #endregion
        #region =================== Public ============================








        #endregion
        #region ================ MonoBehaviour =======================


        #endregion
        #region =============== Methods =======================
        protected override void setIdleDestination()
        {
            _idleDestination = LevelManager.BaseDestination();
        }
        


        #endregion

    }
}