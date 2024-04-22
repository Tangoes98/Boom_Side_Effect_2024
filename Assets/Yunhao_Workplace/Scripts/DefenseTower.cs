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
        [SerializeField] float _attackTimer;//�������
        [SerializeField] float _attackDuration;//��������ʱ��

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
            //���ι�������
            //��ʼ���Ŷ���(���������ʵ�ָ��ӵĻ�������ִ��һ��corotine)
            yield return new WaitForSeconds(_attackDuration);
            _target.SendMessage("TakeDamage", _damage);


            /*�����������ͣ�û���Թ���
            //��ʼ���Ŷ���(���������ʵ�ָ��ӵĻ�������ִ��һ��corotine)
            float timer = _attackDuration;
            while(timer > 0)
            {
                timer -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            */


            yield return new WaitForSeconds(_attackTimer);
            _attacking = null;
            //_attackTarget = GetOpponentInRange(_attackRange);//�������ͨ��������ټ�����䣨û���Թ���
            //���и���Χ���е���Ҫ����������Χ�ڵĵ��ˣ����к͹����߼���Ҫ�޸ģ�����������о͸ĳɹ��������б�ĵ��ˣ����Ƿ�Χ����ʱ�б�ֻ��һ��
            //�������ͺ��������Ϳ������������ӿڰ���Ҫ���ò��ԣ�
        }
        #endregion
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
