using System;

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
    public GameObject LinePause {get;set;}
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

    public void ShowLine() {
        switch(Status) {
            case A_TO_B:
                LineAB.SetActive(true);
                break;
            case B_TO_A:
                LineBA.SetActive(true);
                break;
            case PAUSE:
                LinePause.SetActive(true);
                break;
        }
    }

    public void HideLine() {
        LineAB.SetActive(false);
        LineBA.SetActive(false);
        LinePause.SetActive(false);
    }

    public Link(Architect fromArchitect,Architect toArchitect, GameObject lineAB, GameObject lineBA, GameObject linePause) {
        ArchitectA = fromArchitect;
        ArchitectB = toArchitect;
        LineAB = lineAB;
        LineBA = lineBA;
        LinePause = linePause;
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

    public Link.LinkStatus NextState() {
        if(Status==Link.LinkStatus.A_TO_B) return Link.LinkStatus.B_TO_A;
        if(Status==Link.LinkStatus.B_TO_A) return Link.LinkStatus.PAUSE;
        else return Link.LinkStatus.A_TO_B;
    }
}