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
    private GameObject _linePrefab;

    [SerializeField]
    private StringGameObjectPair[] _basicArchitectPrefabs;

    public Dictionary<string,GameObject> AllBasicArchitectPrefabs;

    private void Awake() {
        if(_instance == null) {
            _instance = this;
        } else if(_instance != this) {
            Destroy(this);
        }
        AllBasicArchitectPrefabs = _basicArchitectPrefabs.ToDictionary(p=>p.code,p=>p.gameObject);
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

    public List<Architect> GetSurroundingArchitects(Architect architect) // 显示可link的其他建筑, 假设范围是圆形的
    {
        return null;
    }

    public Tuple<List<Link>,List<Link>> GetInAndOutLinks(Architect architect) // 用于显示建筑已有link
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
