using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Yunhao_Fight
{
    public class MinionUI : MonoBehaviour
    {
        #region =============== Variables =======================

        [Header("UI")]
        [SerializeField] Transform _UI;
        [SerializeField] Slider _healthBar;

        #endregion
        #region =================== Public ============================


        #endregion
        #region ================ MonoBehaviour =======================

        private void Update()
        {
            UpdateUI();
        }
        #endregion
        #region =============== Methods =======================

        void UpdateUI()
        {
            if( _UI != null)
            {
                _UI.forward = transform.position - Camera.main.transform.position;
            }
        }

        void InitializeHealth(float health)
        {
            _healthBar.value = _healthBar.maxValue = health;
        }


        #endregion

    }
}
