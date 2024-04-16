using System;
using UnityEngine;
public class Link {
    public enum LinkStatus {
        A_TO_B,
        B_TO_A,
        PAUSE
    }

    public Architect ArchitectA {get;}
    public Architect ArchitectB {get;}
    
    public GameObject LineAB {get;set;} // AB BA 箭头方向不同，按需求enable
    public GameObject LineBA {get;set;}
    public bool IsActive => Status != LinkStatus.PAUSE;
    public LinkStatus Status {get;set;}

    public Link(Architect fromArchitect,Architect toArchitect, GameObject lineAB, GameObject lineBA) {
        ArchitectA = fromArchitect;
        ArchitectB = toArchitect;
        LineAB = lineAB;
        LineBA = lineBA;
        Status = LinkStatus.A_TO_B;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Link);
    }

    public bool Equals(Link other)
    {
        return other != null &&
               ((ArchitectA == other.ArchitectA &&
               ArchitectB == other.ArchitectB) ||
               (ArchitectA == other.ArchitectB &&
               ArchitectB == other.ArchitectA));
    }

    public override int GetHashCode() {
        int hashA = ArchitectA.GetHashCode();
        int hashB = ArchitectB.GetHashCode();
        return hashA<hashB? HashCode.Combine(ArchitectA,ArchitectB) : HashCode.Combine(ArchitectB,ArchitectA); 
    }


}