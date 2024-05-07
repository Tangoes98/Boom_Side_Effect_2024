using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public abstract class Architect : MonoBehaviour
{
    [Header("DEV TOOL")]
    [SerializeField]
    protected bool disableSelfExam;
    [Space(20)]

    
    [SerializeField,Range(1,3)]
    public int level;

    [SerializeField,Range(0,4)]
    public int existingLinkNum;

    [Space(20)]
    [Header("不稳定度取以下中更高值")]

    [SerializeField,Range(0,4)]
    public int activeOutputLinkNum;

    [SerializeField,Range(0,4)]
    public int sourceArchitectLinkNum;

    [SerializeField] public GameObject buff,debuff;

    public GameObject BaseArchitect {get;set;} // 合体前的基础建筑


    public string Code => Info().code;

    public int Unstability => math.max(activeOutputLinkNum, sourceArchitectLinkNum);

    public abstract ArchitectBase Info();

    public abstract ArchitectStatus Status();

    protected ArchitectProperty GetBaseProperty(int level)  => Info().GetProperties().Where(p=>p.level==level).First();

    protected ModifierType modifiertype;

    protected IState currentState;

    private bool _isBuffPlaying;
   
    protected virtual void Awake() {
        SelfExam();
        try {
            level = 1; // initialize status to level 1 !!!!!!
            existingLinkNum=0;
            sourceArchitectLinkNum=0;
            activeOutputLinkNum=0;
            if(buff!=null) buff.SetActive(false);
            if(debuff!=null) debuff.SetActive(false);
            _isBuffPlaying = false;
            Reload(); 
        } catch(Exception e) {
            Debug.LogError("建筑初始化失败，请检查BaseInfo>Properties中level=1的元素");
            Debug.LogError(e);
        }
        
    }

    public bool IsUpgradable() => Info().GetProperties().Select(p=>p.level).Max()>level;

    public int GetBuildCost() => Info().isMutant? BaseArchitect.GetComponent<Architect>().GetBaseProperty(1).cost : GetBaseProperty(1).cost;

    public int GetUpgradeCost() => Info().isMutant? //先检查 IsUpgradable()
                BaseArchitect.GetComponent<Architect>().GetBaseProperty(level + 1).cost : 
                GetBaseProperty(level + 1).cost; 

    public void Upgrade() => UpgradeTo(level+1);

    public void Reload() => UpgradeTo(level);

    public abstract void UpgradeTo(int level);

    public bool IsPreview;


    public void SelfDestroy() {
        if(BaseArchitect!=null) {
            ArchiLinkManager.Instance.Unregister(BaseArchitect.GetComponent<Architect>()); // destroy links
            Destroy(BaseArchitect);
        }
        ArchiLinkManager.Instance.Unregister(this); // / destroy links
        Destroy(this.gameObject);
    }

    private void SelfExam() {
        if(disableSelfExam) return;

        HashSet<int> levels = new();
        foreach( ArchitectProperty p in Info().GetProperties()) {
            if(levels.Contains(p.level)) {
                throw new System.ArgumentOutOfRangeException("Duplicate Level " +p.level+ " found in properties");
            } else {
                levels.Add(p.level);
            }
                
        }
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Status().range);
    }

    public IEnumerator PlayBuff(bool isBuff) {
        if(!_isBuffPlaying) {
            _isBuffPlaying = true;
            if(isBuff) {
                buff.SetActive(true);
            } else {
                debuff.SetActive(true);
            }
            yield return new WaitForSeconds(1);
            buff.SetActive(false);
            debuff.SetActive(false);
            _isBuffPlaying = false;
        }
    }

}
