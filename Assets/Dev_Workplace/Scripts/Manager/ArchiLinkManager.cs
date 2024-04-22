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
    [Header("DEV TOOL")]
    [SerializeField]
    protected bool disableSelfExam;
    [Space(20)]
    [SerializeField] private GameObject _directionalLinePrefab;
    [SerializeField] private GameObject _pauseLinePrefab;

    [SerializeField]
    private StringGameObjectPair[] _basicArchitectPrefabs,_mutantArchitectPrefabs;

    [SerializeField]
    private ArchitectMutation[] _mutations;
    [SerializeField]
    private ArchitectModifer[] _modifiers;

    public Dictionary<string,GameObject> AllBasicArchitectPrefabs, AllMutantArchitectPrefabs;


    private Dictionary<int,Dictionary<int,ArchitectModiferCase>> _modifierDict = new();
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

        SelfExam();
        
        AllBasicArchitectPrefabs = _basicArchitectPrefabs.ToDictionary(p=>p.code,p=>p.gameObject);
        AllMutantArchitectPrefabs = _mutantArchitectPrefabs.ToDictionary(p=>p.code,p=>p.gameObject);

        _modifierDict = _modifiers.ToDictionary(m => m.unstability,
                                        m=> {
                                                Dictionary<int,ArchitectModiferCase> dict = new();
                                                int total = 0;
                                                foreach(var c in m.modifierCases.Where(c=>c.type!=ModifierType.NONE)) {
                                                    total+=c.probability;
                                                    dict.Add(total,c);
                                                }
                                                return dict;
                                            });
    }
    
    public Architect Build(Vector3 position, string code) // 在对应位置instantiate建筑
    {   
        Debug.Log("build");
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

            var lines = BuildLink(cloestArch, arch);
            lines.Item1.SetActive(false);
            lines.Item2.SetActive(false);
        } 
        return arch;
    }
    

    public void Unregister(Architect architect) {
        _architects.Remove(architect);
        foreach(var link in _links.Where(l=> l.IsActive && (l.ArchitectA==architect || l.ArchitectB==architect)).ToList()) {
            DestroyLink(link);
        }
    }

    private bool Mutate(Architect fromArchitect,Architect toArchitect) { // disable基础建筑，存起来，在原地instantiate变种建筑
        try {
            Vector3 pos = _architects[toArchitect];
            string mutantCode = GetMutantResult(fromArchitect.Info().isMutant? 
                                    fromArchitect.BaseArchitect.GetComponent<Architect>().Code 
                                    : fromArchitect.Code, toArchitect.Code);
            Architect newArch = Build(pos,mutantCode);
            newArch.BaseArchitect = toArchitect.gameObject; // save for later
            
            // copy status
            Architect baseArch = toArchitect;
            newArch.activeOutputLinkNum = baseArch.activeOutputLinkNum;
            newArch.existingLinkNum = baseArch.existingLinkNum;

            fromArchitect.activeOutputLinkNum++;
            UpdateSurroundingSourceLinkNum(fromArchitect,newArch);
            newArch.sourceArchitectLinkNum = fromArchitect.activeOutputLinkNum;

            // need to upgrade architect to be same as the base one first
            newArch.UpgradeTo(toArchitect.level);

            toArchitect.gameObject.SetActive(false);

            foreach(var l in _links.Where(l=> l.ArchitectA==baseArch || l.ArchitectB==baseArch )) {
                if(l.ArchitectA==baseArch) l.ArchitectA=newArch;
                if(l.ArchitectB==baseArch) l.ArchitectB=newArch;
            }

            return true;
        } catch(InvalidOperationException ioe) {
            Debug.LogError("mutation not found for " + fromArchitect.Code + ", " + toArchitect.Code);
        }
        return false;
    }

    private string GetMutantResult(string fromArchitectCode, string toArchitectCode) 
                            => _mutations.Where(m => m.baseArchitectCode == toArchitectCode && m.buffArchitectCode==fromArchitectCode)
                                .Select(m=>m.mutantArchitectCode).First();

    private Architect UnMutate(Architect architect) {
        if(architect.BaseArchitect==null) {
            // base architect, no mutation
            return architect;
        }
        
        Architect baseArch = architect.BaseArchitect.GetComponent<Architect>();
        // copy status
        baseArch.activeOutputLinkNum = architect.activeOutputLinkNum;
        baseArch.existingLinkNum = architect.existingLinkNum;
        baseArch.sourceArchitectLinkNum = 0;

        // need to upgrade base architect to be same as the mutant one first
        baseArch.UpgradeTo(architect.level);

        architect.BaseArchitect.SetActive(true);

        foreach(var l in _links.Where(l=> l.ArchitectA==architect || l.ArchitectB==architect )) {
            if(l.ArchitectA==architect) l.ArchitectA=baseArch;
            if(l.ArchitectB==architect) l.ArchitectB=baseArch;
        }

        Destroy(architect.gameObject);
        return baseArch;
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
        if(link.Status==PAUSE) {
            // 启动
            link.Status = newStatus;
            
            // pause all other incoming links
            if(link.To.Info().isMutant) {
                foreach(var l in _links.Where(l=> l!=link && l.To==link.To)) {
                    UpdateLink(l,PAUSE); // this will change arch to basic arch
                }
            }
            
            // mutate again
            Mutate(link.From,link.To);

        } else if(newStatus == PAUSE) {
            // 暂停
            link.From.activeOutputLinkNum--;
            UpdateSurroundingSourceLinkNum(link.From,link.To);
            UnMutate(link.To);

            link.Status = newStatus;

        } else {
             // 反向
            link.From.activeOutputLinkNum--;
            UpdateSurroundingSourceLinkNum(link.From,link.To);
            UnMutate(link.From);
            UnMutate(link.To);

            // mutate again
            Mutate(link.To,link.From);

            link.Status = newStatus;
        }
        
       
        
    }

    private void UpdateSurroundingSourceLinkNum(Architect architect, Architect ignore) {
        // architect is the outputter
        foreach(var arch in _links.Where(l => l.From==architect && l.To!=ignore).Select(l=>l.To)) {
            arch.sourceArchitectLinkNum = architect.activeOutputLinkNum;
        }
    }

    public List<Architect> GetSurroundingArchitects(Architect architect) // 显示可link的其他建筑，策划说先不允许主动连线/断线
    {
        return null;
    }

    public Tuple<GameObject,GameObject> BuildLink(Architect fromArch, Architect toArch) { // 策划说不做主动连线
        Vector3[] waypoints = GenerateCurveLine(_architects[fromArch],_architects[toArch], 10); // height可改
        GameObject line = Instantiate(_directionalLinePrefab), 
            lineReverse = Instantiate(_directionalLinePrefab),
            linePause = Instantiate(_pauseLinePrefab);
        
        line.name =  fromArch.Info().name + "-" + toArch.Info().name;
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.positionCount = waypoints.Length;
        lineRenderer.SetPositions(waypoints);

        lineReverse.name =  toArch.Info().name + "-" + fromArch.Info().name;
        LineRenderer lineRenderer2 = lineReverse.GetComponent<LineRenderer>();
        lineRenderer2.positionCount = waypoints.Length;
        lineRenderer2.SetPositions(waypoints.Reverse().ToArray());

        linePause.name = "pause";
        LineRenderer lineRenderer3 = line.GetComponent<LineRenderer>();
        lineRenderer3.positionCount = waypoints.Length;
        lineRenderer3.SetPositions(waypoints);

        Link link = new(fromArch, toArch, line, lineReverse,linePause);
        UpdateLink(link, A_TO_B);
        _links.Add(link);
        
        return new(line,lineReverse);
    }

    private Vector3[] GenerateCurveLine(Vector3 start, Vector3 end, int height) {
        // y轴是纵轴
        var pointList = new List<Vector3>();
        var curvePoint = new Vector3((start.x + end.x) / 2,(start.y + end.y) / 2 + height,(start.z + end.z) / 2 );
        int vertexCount = 50;
        for(float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount) {
            var tanLineVertex1 = Vector3.Lerp(start, curvePoint, ratio);
            var tanLineVertex2 = Vector3.Lerp(curvePoint, end, ratio);
            var bezierPoint = Vector3.Lerp(tanLineVertex1,tanLineVertex2,ratio);
            pointList.Add(bezierPoint);
        }
        return pointList.ToArray();
    }

    public void DestroyLink(Link link) { // 摧毁连接
        UpdateLink(link, PAUSE);
        link.ArchitectA.existingLinkNum--;
        link.ArchitectB.existingLinkNum--;
        Destroy(link.LineAB);
        Destroy(link.LineBA);
        _links.Remove(link);
    }

    public void LinkFromClosestArchToPointer(bool on) => _linkFromClosestArchToPointer= on;// 从最近的建筑到鼠标,建筑必须是通过Build()产生         

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
            _lineToMouse = Instantiate(_directionalLinePrefab);
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

            if(arch.existingLinkNum>=arch.Info().maxLinkNum) { 
                Debug.Log(arch.existingLinkNum + " " +arch.Info().maxLinkNum);
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

    public float GetModifier(int unstability, out ModifierType modifierType) {
        if(unstability<=1) {
            modifierType = ModifierType.NONE;
            return 1;
        }
        var modifierCases = _modifierDict[unstability];
        int rand = (int) (UnityEngine.Random.value * 100);
        
        int caseNum = modifierCases.Keys.Where(k=>rand<k).OrderBy(k=>k).FirstOrDefault();
        if(caseNum==0) {
            modifierType = ModifierType.NONE;
            return 1;
        }
        var modifierCase = modifierCases[caseNum];
        modifierType = modifierCase.type;
        return modifierCase.modifier;
        
    }

    private void SelfExam() {
        if(disableSelfExam) return;

        HashSet<int> unstabilities = new();
        foreach( ArchitectModifer m in _modifiers) {
            if(unstabilities.Contains(m.unstability)) {
                throw new Exception("Duplicate Unstability " +m.unstability+ " found");
            } else {
                bool isNoBuffCaseSpecified = false;
                int totalProb = 0;
                foreach(var modifierCase in m.modifierCases) {
                    if(modifierCase.type==ModifierType.NONE) {
                        isNoBuffCaseSpecified = true;
                    }
                    totalProb += modifierCase.probability;
                }
                if(isNoBuffCaseSpecified && totalProb!=100) {
                    throw new Exception("NONE buff case specified but probabilities don't add up to 100 in unstability" +m.unstability);
                } else if (!isNoBuffCaseSpecified && totalProb>100) {
                    throw new Exception("Nprobabilities exceed 100 in unstability" +m.unstability);
                }
            }
                
        }
        for(int i=2;i<=4;i++) {
            if(!unstabilities.Contains(i)) {
                throw new Exception("Missing Unstability " +i);
            }
        }
        
    }
}
