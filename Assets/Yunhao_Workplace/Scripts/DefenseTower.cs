using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Yunhao_Fight
{
    public class DefenseTower : MonoBehaviour
    {
        #region =============== Variables =======================

        [SerializeField] LineRenderer _laser;
        Enemy _target;
        [SerializeField] bool _isWorking = true;

        [Header("Attack")]
        [SerializeField] float _attackRange;
        [SerializeField] float _damage;
        [SerializeField] float _attackTimer;

        #endregion
        #region =================== Public ============================

        #endregion
        #region ================ MonoBehaviour =======================
        private void Start()
        {
            _laser.SetPosition(0, _laser.transform.position);
            StartCoroutine(Attack());
            
        }
        private void Update()
        {
            _laser.positionCount = 2;
            if (_target != null)//is attacking
            {
                _laser.SetPosition(1, _target.transform.position);
            }
            else
            {
                _laser.SetPosition(1, _laser.transform.position);
            }

        }
        #endregion
        #region =============== Methods =======================
        Enemy GetEnemyInRange()
        {
            Collider[] attackTargets = Physics.OverlapSphere(this.transform.position, this._attackRange, LevelManager.EnemyLayer());
            if (attackTargets.Length>0 && attackTargets[0].gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                Debug.Log("Get a new enemy: " + enemy.name);
                return enemy;
            }
            else return null;

        }
        IEnumerator Attack()
        {
            yield return null;
            while (_isWorking)
            {
                if (_target == null)
                {
                    _target = GetEnemyInRange();
                    yield return null;
                }
                else
                {
                    while(_target != null && Vector3.Distance(_target.transform.position, this.transform.position) < _attackRange)
                    {
                        _target.SendMessage("TakeDamage", _damage);
                        yield return new WaitForSeconds(_attackTimer);
                    }
                    _target = GetEnemyInRange();
                }
                yield return null;
            }
        }


        #endregion
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
