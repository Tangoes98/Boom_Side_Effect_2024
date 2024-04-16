using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Link.LinkStatus;

public class ArchiLinkManager : MonoBehaviour
{   
    private static ArchiLinkManager _instance;
    public static ArchiLinkManager Instance {
        get {
            if(_instance==null) {
                throw new Exception("ArchiLinkManager not in scene");
            }  
            return _instance; 
        } 
    }

    [SerializeField]
    private GameObject _linePrefab;

    [SerializeField]
    private StringGameObjectPair[] _basicArchitectPrefabs,_mutantArchitectPrefabs;

    [SerializeField]
    private ArchitectMutation[] _mutations;

    public Dictionary<string,GameObject> AllBasicArchitectPrefabs, AllMutantArchitectPrefabs;


    private GameObject _lineToMouse;
    private bool _linkFromClosestArchToPointer;

    private Dictionary<Architect,Vector3> _architects = new();
    private List<Link> _links = new();

    private void Awake() {
        if(_instance == null) {
            _instance = this;
        } else if(_instance != this) {
            Destroy(this);
        }
        AllBasicArchitectPrefabs = _basicArchitectPrefabs.ToDictionary(p=>p.code,p=>p.gameObject);
        AllMutantArchitectPrefabs = _mutantArchitectPrefabs.ToDictionary(p=>p.code,p=>p.gameObject);
    }
    
    public Architect Build(Vector3 position, string code) // 在对应位置instantiate建筑
    {   
        bool isMutant = !AllBasicArchitectPrefabs.ContainsKey(code);

        GameObject archObj = Instantiate(isMutant? AllMutantArchitectPrefabs[code] : AllBasicArchitectPrefabs[code], 
                                        position,Quaternion.identity);
        Architect arch = archObj.GetComponent<Architect>();
        _architects.Add(arch, position);
        if(!isMutant) {
            // auto link cloest link if basic, then mutate
            var tuple = FindClosestArchArray(arch);
            if(tuple==null) {
                return arch;
            }
            var cloestArch = tuple.Item1;
            
            if(Mutate(cloestArch, arch)) {
                GameObject line = BuildLink(cloestArch, arch);
                line.SetActive(false);
                // new link
                cloestArch.existingLinkNum ++;
                arch.existingLinkNum ++;

                
            }
        } 
        return arch;
    }
    

    public void Unregister(Architect architect) {
        _architects.Remove(architect);
        _links.RemoveAll(l=>l.ArchitectA==architect || l.ArchitectB==architect);
    }

    private bool Mutate(Architect fromArchitect,Architect toArchitect) { // disable基础建筑，存起来，在原地instantiate变种建筑
        try {
            Vector3 pos = _architects[toArchitect];
            string mutantCode = GetMutantResult(fromArchitect.Code, toArchitect.Code);
            Architect arch = Build(pos,mutantCode);
            arch.BaseArchitect = toArchitect.gameObject; // save for later

            toArchitect.gameObject.SetActive(false);

            fromArchitect.activeOutputLinkNum++;
            toArchitect.sourceArchitectLinkNum = fromArchitect.activeOutputLinkNum;

            return true;
        } catch(InvalidOperationException ioe) {
            Debug.LogError("mutation not found for " + fromArchitect.Code + ", " + toArchitect.Code);
        }
        return false;
    }

    private string GetMutantResult(string fromArchitectCode, string toArchitectCode) 
                            => _mutations.Where(m => m.baseArchitectCode == toArchitectCode && m.buffArchitectCode==fromArchitectCode)
                                .Select(m=>m.mutantArchitectCode).First();

    private void UnMutate(Architect architect) {
        // need to upgrade base architect to be same as the mutant one first
        architect.BaseArchitect.GetComponent<Architect>().UpgradeTo(architect.level);
        architect.BaseArchitect.SetActive(true);
        Destroy(architect.gameObject);
    }

    public void HighAllLinks(bool isShow) {
        foreach (var link in _links) {
            link.LineAB.SetActive(isShow);
        }
    }

    public void GetInAndOutAndPauseLinks(Architect architect, out List<Link> incomingLinks,
                                        out List<Link> outgoingLinks, out List<Link> pausedLinks) // 用于显示建筑已有link
    {
        incomingLinks = new();
        outgoingLinks = new();
        pausedLinks = new();

        foreach(Link l in _links.Where(l=>l.ArchitectA==architect || l.ArchitectB==architect)) {
            if(l.Status==PAUSE) {
                pausedLinks.Add(l);
                continue;
            }
            if((architect==l.ArchitectB && l.Status==A_TO_B) ||
                (architect==l.ArchitectA && l.Status==B_TO_A)) {
                incomingLinks.Add(l);
                continue;
            }
            outgoingLinks.Add(l);
        }
    }

    public void UpdateLink(Link link, Link.LinkStatus newStatus) {
        if(link.Status == newStatus) return;
        // check 2 architect

        Architect oldOuting;
        Architect oldAbsorbing; 
        Architect newOuting;
        Architect newAbsorbing; 

        if(link.Status==PAUSE) {
            // null -> outing
            // null -> absorbing
            if(newStatus == A_TO_B) {
                newOuting = link.ArchitectA;
                newAbsorbing = link.ArchitectB;
            } else {
                newOuting = link.ArchitectB;
                newAbsorbing = link.ArchitectA;
            }

            //Remo
        }
        //Architect oldOuting = ; 
        
        
        // outing -> null
        // abosrbing -> null
        // outing -> absorbing
        // absorbing -> outing 
        
        // update parent connection number
        // mute/unmute

        link.Status = newStatus;
    }

    public List<Architect> GetSurroundingArchitects(Architect architect) // 显示可link的其他建筑，策划说先不允许主动连线/断线
    {
        return null;
    }

    public GameObject BuildLink(Architect fromArch, Architect toArch) { // 策划说不做
        Vector3[] waypoints = new Vector3[]{_architects[fromArch],_architects[toArch]};
        GameObject line = Instantiate(_linePrefab), 
            lineReverse = Instantiate(_linePrefab);
        
        line.name =  fromArch.Info().name + "-" + toArch.Info().name;
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.positionCount = waypoints.Length;
        lineRenderer.SetPositions(waypoints);

        lineReverse.name =  toArch.Info().name + "-" + fromArch.Info().name;
        LineRenderer lineRenderer2 = lineReverse.GetComponent<LineRenderer>();
        lineRenderer2.positionCount = waypoints.Length;
        lineRenderer2.SetPositions(waypoints.Reverse().ToArray());

        _links.Add(new(fromArch, toArch, line, lineReverse));
        return line;
    }

    public void LinkFromClosestArchToPointer(bool on) => _linkFromClosestArchToPointer= on;// 从最近的建筑到鼠标
         

    private void Update() {
        UpdateLineFromClosestArchToPointer();
    }

    private void UpdateLineFromClosestArchToPointer() {
        if(!_linkFromClosestArchToPointer) {
            if(_lineToMouse!=null) {
                Destroy(_lineToMouse);
                _lineToMouse = null;
            } 
            return;
        }
        var tuple = FindClosestArchArray(null);
        if(tuple==null) {
            return;
        }
        if(_lineToMouse==null) {
            _lineToMouse = Instantiate(_linePrefab);
            _lineToMouse.name =  "line_from_closest_arch_to_pointer";
        }
       
        LineRenderer lineRenderer = _lineToMouse.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        
        lineRenderer.SetPositions(tuple.Item2);
    }

    private Tuple<Architect,Vector3[]> FindClosestArchArray(Architect skipArch) {
        // 鼠标坐标，后续改
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 closestPos = Vector3.zero;
        Architect closestArch = null;
        float dis = float.PositiveInfinity;
        foreach(var e in _architects) {
            var arch = e.Key;
            var pos = e.Value;

            if(skipArch==arch) {
                continue;
            }

            if(arch.existingLinkNum>=arch.Status().maxLinkNum) { 
                Debug.Log(arch.name + " all outlets occupied. Skip.");
                continue; // skip arch with all outlets occupied
            }
            float newDis = Vector3.Distance(mousePos, pos);

            if(dis>newDis) {
                dis= newDis;
                closestPos = pos;
                closestArch = arch;
            }
        }
        return (closestArch==null)? null : new Tuple<Architect, Vector3[]> (closestArch, new Vector3[]{mousePos,closestPos});
    }
}
