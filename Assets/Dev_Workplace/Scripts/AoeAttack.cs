using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AoeAttack : MonoBehaviour {
    [SerializeField] private bool unitRangeAsAoeRange = false;
    public float aoeRange;

    public AoeType type;

    private float _damage;

    [SerializeField] private float _mainBaseDmg; 
    private List<EffectStruct> _effects = new();

    private Minion mParent;
    private DefenseTower tParent;

    private bool _isMainBase;

    private Vector3 _center=Vector3.zero;

    private void Start() {
        mParent = GetComponent<Minion>();
        tParent = GetComponent<DefenseTower>();
        var mainBase = GetComponent<MainBase>();
        if(mParent != null) {
            if(unitRangeAsAoeRange) {
                aoeRange = mParent.status.range;
            }
            _damage = mParent.status.damage;
            if(mParent.status.specialEffect!=SpecialEffect.NONE) {
                _effects.Add(new EffectStruct(mParent.status.specialEffect, 
                                                mParent.status.specialEffectModifier, 
                                                mParent.status.specialEffectLastTime));
            }
            if(mParent.status.secondSpEffect!=SpecialEffect.NONE) {
                _effects.Add(new EffectStruct(mParent.status.secondSpEffect, 
                                                mParent.status.secondSpEffectModifier, 
                                                mParent.status.specialEffectLastTime));
            } 
        } else if(tParent != null) {
             if(unitRangeAsAoeRange) {
                aoeRange = tParent.Status().range;
            }
            var status = (DefenseTowerStatus) tParent.Status();
            _damage = status.damage;
            if(status.specialEffect!=SpecialEffect.NONE) {
                _effects.Add(new EffectStruct(status.specialEffect, 
                                                status.specialEffectModifier, 
                                                status.specialEffectLastTime));
            }
            if(status.secondSpEffect!=SpecialEffect.NONE) {
                _effects.Add(new EffectStruct(status.secondSpEffect, 
                                                status.secondSpEffectModifier, 
                                                status.specialEffectLastTime));
            } 
        } else {
            // main base
            _damage = _mainBaseDmg;
            _isMainBase = true;
            _center = this.transform.position;
        }
    }

    private void Update() {
        if(type==AoeType.CIRCLE_CENTER_SELF || type==AoeType.CIRCLE_CENTER_SELF_DIE || type==AoeType.LINE || type==AoeType.ARC) {
            _center = transform.position;
        }
        if(_isMainBase) {
            TriggerAOE(_center,1);
        }
    }

    public void TriggerAOE(Vector3 center, float takeDamageModifer) {
        Minion[] enemies; 
        Gizmos.color = Color.yellow;
        if(type == AoeType.LINE) {
            enemies = GetEnemyInRange(center,10, ParentFaceDirection());
        } else if(type == AoeType.ARC) {
            enemies = GetEnemyInRange(center, 120, ParentFaceDirection());
        } else {
            enemies = GetEnemyInRange(center,360, Vector3.zero);
        }
        _center = center;
        foreach(var enemy in enemies) {
            if(_damage>0) enemy.TakeDamage(_damage*takeDamageModifer);
            if(_effects!=null && _effects.Count>0) {
                foreach(var effect in _effects) {
                    enemy.TakeEffect(effect.type,effect.modifier,effect.lastTime);
                }
                
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if( _center == Vector3.zero) return;
        if(type == AoeType.LINE) {
            Gizmos.DrawLine(_center, _center + aoeRange * ParentFaceDirection());
        } else if(type == AoeType.ARC) {
            Gizmos.DrawLine(_center, _center + aoeRange * (Quaternion.Euler(0, -60, 0) * ParentFaceDirection()));
            Gizmos.DrawLine(_center, _center + aoeRange * (Quaternion.Euler(0, 60, 0) * ParentFaceDirection()));
        } else {
            Gizmos.DrawWireSphere(_center, aoeRange);
        }
    }

    private Vector3 ParentFaceDirection() {
        if(mParent!=null) return mParent.transform.forward;
        if(tParent!=null) return tParent.transform.forward;
        return Vector3.zero;
    } 

    private Minion[] GetEnemyInRange(Vector3 center, int angle, Vector3 faceDirection)
    {
        LayerMask OppenetLayer = Yunhao_Fight.LevelManager.EnemyLayer();
        Collider[] attackTargets = Physics.OverlapSphere(center, aoeRange, OppenetLayer);
        Minion[] minions = attackTargets.Select(collider => collider.gameObject.GetComponent<Minion>())
                               .Where(enemy => enemy != null)
                               .Where(enemy => Vector3.Distance(enemy.transform.position, center)>0.001f) // not self
                               .Where(enemy => angle==360 || Vector3.Angle(enemy.transform.position-center, faceDirection) <= angle )
                               .ToArray();
        return minions;
    }
}

public enum AoeType {
    CIRCLE_CENTER_SELF,
    CIRCLE_CENTER_ENEMY,
    CIRCLE_CENTER_SELF_DIE,
    CIRCLE_CENTER_ENEMY_DIE,
    LINE,
    ARC
}