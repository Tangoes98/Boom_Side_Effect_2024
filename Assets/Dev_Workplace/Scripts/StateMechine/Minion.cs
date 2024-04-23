using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace Yunhao_Fight
{
    public class Minion : MinionBase
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
            _idleDestination = this.transform;
        }

        protected override void View(MinionBase oppenent)
        {
            base.View(oppenent);

            //我方离开兵营范围
            Barracks barracks = GetComponentInParent<Barracks>();
            if (Vector3.Distance(barracks.transform.position, this.transform.position) >= barracks.AttackRange())
            {
                //停止但持续索敌直到敌方（可能）进入兵营范围
                _agent.speed = 0;
                if (oppenent!=null && Vector3.Distance(barracks.transform.position, oppenent.transform.position) <= barracks.AttackRange())
                {
                    _agent.speed = _moveSpeed;
                }
            }
        }

        #endregion


    }
}