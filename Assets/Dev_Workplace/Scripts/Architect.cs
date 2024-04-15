using UnityEngine;

public abstract class Architect : MonoBehaviour
{
    [SerializeField]
    protected Vector3 position;
    [SerializeField]
    protected int level;
    [SerializeField]
    protected ArchitectBase info;
    [SerializeField]
    protected GameObject baseArchitect;

    
    [SerializeField]
    protected int maxLinkNum;
    [SerializeField]
    protected int existingLinkNum;
    [SerializeField]
    protected int activeOutputLinkNum;
    [SerializeField]
    protected float range;


    public string Code => info.Code;
    public bool IsUpgradable => info.IsUpgradable(level);

    public void Upgrade() => UpgradeTo(level++);

    public abstract void UpgradeTo(int level);

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

}
