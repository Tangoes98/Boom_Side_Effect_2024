using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]
public class BarrackStatus : ArchitectStatus {
    [Inspectable]
    public List<Minion> curMinions; 
    [Inspectable]
    public int maxMinionNum;
    [Inspectable]
    public float coolDown;
}