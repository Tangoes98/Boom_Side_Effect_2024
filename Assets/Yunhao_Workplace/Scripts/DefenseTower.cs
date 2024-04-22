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
        Enemy _targets;
        [SerializeField] bool _isWorking = true;

        [Header("Attack")]
        [SerializeField] float _attackRange;
        [SerializeField] float _damage;
        [SerializeField] float _attackTimer;//攻击间隔
        [SerializeField] float _attackDuration;//攻击持续时间

        Coroutine _attacking;

        #endregion
        #region =================== Public ============================

        #endregion
        #region ================ MonoBehaviour =======================
        private void Start()
        {
            _laser.SetPosition(0, _laser.transform.position);
            StartCoroutine(Combat());
            
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
                //Debug.Log("Get a new enemy: " + enemy.name);
                return enemy;
            }
            else return null;

        }
        IEnumerator Combat()
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
                        if (_attacking == null) _attacking = StartCoroutine(Attack());
                        yield return null;
                    }
                    _target = GetEnemyInRange();
                }
                yield return null;
            }
        }
        IEnumerator Attack()
        {
            //单次攻击类型
            //开始播放动画(如果动画是实现复杂的话可以再执行一个corotine)
            yield return new WaitForSeconds(_attackDuration);
            _target.SendMessage("TakeDamage", _damage);


            /*持续攻击类型（没测试过）
            //开始播放动画(如果动画是实现复杂的话可以再执行一个corotine)
            float timer = _attackDuration;
            while(timer > 0)
            {
                timer -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            */


            yield return new WaitForSeconds(_attackTimer);
            _attacking = null;
            //_attackTarget = GetOpponentInRange(_attackRange);//如果是普通索敌这边再加上这句（没测试过）
            //还有个范围索敌的需要攻击整个范围内的敌人，索敌和攻击逻辑都要修改，或者这边索敌就改成攻击整个列表的敌人，但非范围索敌时列表只有一个
            //攻击类型和索敌类型考虑做成两个接口按需要调用策略？
        }
        #endregion
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
