using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Architect : MonoBehaviour
{
    [SerializeField]
    protected int level;

    protected GameObject baseArchitect; // 合体前的基础建筑


    public string Code => GetInfo().code;
    public bool IsUpgradable => GetInfo().GetProperties().Select(p=>p.level).Max()>level;
    
    protected abstract ArchitectBase GetInfo();
    protected ArchitectModiferBase GetBaseModifer(int sourceArchLinkNum) 
                                        => GetBaseProperty(level).GetModifers().Where(p=>p.sourceLinkNum==sourceArchLinkNum).First();

    protected ArchitectProperty GetBaseProperty(int level)  => GetInfo().GetProperties().Where(p=>p.level==level).First();


    protected virtual void Awake() {
        SelfExam();
    }

    public void Upgrade() => UpgradeTo(level+1);

    protected abstract void UpgradeTo(int level);

    public void UnMutate() {
        // need to upgrade base architect to be same as the mutant one first
        baseArchitect.GetComponent<Architect>().UpgradeTo(level);
        baseArchitect.SetActive(true);
        Destroy(this.gameObject);
    }

    public void Mutate(Architect fromArchitect) {
        ArchiLinkManager.Instance.Mutate(fromArchitect, this);
    }


    public void Destroy() {
        if(baseArchitect!=null) {
            ArchiLinkManager.Instance.Destroy(baseArchitect.GetComponent<Architect>()); // destroy links
            Destroy(baseArchitect);
        }
        ArchiLinkManager.Instance.Destroy(this); // / destroy links
        Destroy(this.gameObject);
    }

    private void SelfExam() {

        foreach( ArchitectProperty p in GetInfo().GetProperties()) {
             HashSet<int> levels = new();
            if(p.GetModifers()==null || p.GetModifers().Length==0) {
                if(GetInfo().isMutant)
                    throw new System.Exception("Mutant architect must have modifer in them");
            } else {
                if(!GetInfo().isMutant)
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
