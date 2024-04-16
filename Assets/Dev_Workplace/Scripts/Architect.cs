using System;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField,Range(0,4)]
    public int activeOutputLinkNum;

    [SerializeField,Range(0,4)]
    public int sourceArchitectLinkNum;

    public GameObject BaseArchitect {get;set;} // 合体前的基础建筑


    public string Code => Info().code;

    public bool IsUpgradable => Info().GetProperties().Select(p=>p.level).Max()>level;

    
    public abstract ArchitectBase Info();

    protected ArchitectProperty GetBaseProperty(int level)  => Info().GetProperties().Where(p=>p.level==level).First();


    protected virtual void Awake() {
        SelfExam();
        try {
            level = 1; // initialize status to level 1 !!!!!!
            existingLinkNum=0;
            sourceArchitectLinkNum=0;
            activeOutputLinkNum=0;
            Reload(); 
        } catch(Exception e) {
            Debug.LogError("建筑初始化失败，请检查BaseInfo>Properties中level=1的元素");
            Debug.LogError(e);
        }
        
    }

    public void Upgrade() => UpgradeTo(level+1);

    public void Reload() => UpgradeTo(level);

    public abstract void UpgradeTo(int level);

    protected abstract void ApplyModifer();

    public abstract ArchitectStatus Status();


    public void Destroy() {
        if(BaseArchitect!=null) {
            ArchiLinkManager.Instance.Unregister(BaseArchitect.GetComponent<Architect>()); // destroy links
            Destroy(BaseArchitect);
        }
        ArchiLinkManager.Instance.Unregister(this); // / destroy links
        Destroy(this.gameObject);
    }

    private void SelfExam() {
        if(disableSelfExam) return;


        foreach( ArchitectProperty p in Info().GetProperties()) {
             HashSet<int> levels = new();
            if(p.GetModifers()==null || p.GetModifers().Length==0) {
                if(Info().isMutant)
                    throw new System.Exception("Mutant architect must have modifer in them");
            } else {
                if(!Info().isMutant)
                    throw new System.Exception("Only mutant architect can have modifer in them");
                else {
                    HashSet<int> keys = new();
                    foreach(var m in p.GetModifers()) {
                        if(m.probability <0 || m.probability>1)
                            throw new System.ArgumentOutOfRangeException("probability has to be in 0-1");
                        if(keys.Contains(m.sourceLinkNum)) {
                            throw new System.ArgumentOutOfRangeException("Duplicate Key " +m.sourceLinkNum+ " found in modifiers");
                        } else {
                            keys.Add(m.sourceLinkNum);
                        }
                    }
                    
                    
                }
                    
            }
            if(levels.Contains(p.level)) {
                throw new System.ArgumentOutOfRangeException("Duplicate Level " +p.level+ " found in properties");
            } else {
                levels.Add(p.level);
            }
                
        }
        
    }

}
