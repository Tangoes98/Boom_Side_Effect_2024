using System;
using System.Collections.Generic;
using UnityEngine;
using static Link.LinkStatus;
public class Link {
    public enum LinkStatus {
        A_TO_B,
        B_TO_A,
        PAUSE
    }

    public Architect ArchitectA {get;set;}
    public Architect ArchitectB {get;set;}
    
    public GameObject LineAB {get;set;} // AB BA 箭头方向不同，按需求enable
    public GameObject LineBA {get;set;}
    public LinkStatus Status {get;set;}

    public bool IsActive => Status != PAUSE;
    public Architect From {
        get {
            return IsActive? (Status==A_TO_B? ArchitectA : ArchitectB) : null;
        }
        set {
            if(IsActive) {
                if(Status==A_TO_B) {
                    ArchitectA = value;
                } else {
                    ArchitectB = value;
                }
            } else {
                throw new InvalidOperationException("inactive link doesn't have From");
            }
        }
    }
    public Architect To {
        get {
            return IsActive? (Status==A_TO_B? ArchitectB : ArchitectA) : null;
        }
        set {
            if(IsActive) {
                if(Status==A_TO_B) {
                    ArchitectB = value;
                } else {
                    ArchitectA = value;
                }
            } else {
                throw new InvalidOperationException("inactive link doesn't have To");
            }
        }
    } 

    public Link(Architect fromArchitect,Architect toArchitect, GameObject lineAB, GameObject lineBA) {
        ArchitectA = fromArchitect;
        ArchitectB = toArchitect;
        LineAB = lineAB;
        LineBA = lineBA;
        Status = PAUSE;
        ArchitectA.existingLinkNum++;
        ArchitectB.existingLinkNum++;
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