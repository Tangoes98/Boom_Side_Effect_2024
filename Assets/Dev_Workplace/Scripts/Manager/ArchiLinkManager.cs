using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private GameObject[] _basicArchitectPrefabs, _mutantArchitectPrefabs;

    private Dictionary<string,GameObject> _allMutantArchitectPrefabs;

    public Dictionary<string,GameObject> AllBasicArchitectPrefabs {get; private set;}

    private void Awake() {
        if(_instance == null) {
            _instance = this;
        } else if(_instance != this) {
            Destroy(this);
        }
    }
    
    public void Build(Vector3 position, int code) // 在对应位置instantiate建筑
    {

    }

    public void Mutate(Architect toArchitect, Architect fromArchitect) { // disable 基础建筑，在原地instantiate变种建筑
        toArchitect.gameObject.SetActive(false);
        string mutantCode = ArchitectConfig.GetMutantResult(fromArchitect.Code, toArchitect.Code);
        

    }
    

    public void Destroy(Architect architect) {

    }

    public List<Architect> GetSurroundingArchitects() // 显示可link的其他建筑
    {
        return null;
    }

    public Tuple<List<Link>,List<Link>> GetInAndOutLinks(Architect architect) // 用于显示已有link
    {
        return null;
    }

    public Tuple<List<Link>,List<Link>> SwitchLink(Architect architect,  Link link) // enable link并disable其他link，并返回更新后的in-and-out links
    {
        return null;
    }

    public void BuildLink() {
        return;
    }

}
