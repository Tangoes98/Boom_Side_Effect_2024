using UnityEngine;
public class Link {
    public Architect FromArchitect {get;}
    public Architect ToArchitect {get;}
    public bool IsActive {get;}
    public GameObject Line {get;}

    public Link(Architect fromArchitect,Architect toArchitect, bool isActive,GameObject line) {
        FromArchitect = fromArchitect;
        ToArchitect = toArchitect;
        IsActive = isActive;
        Line = line;
    }

}