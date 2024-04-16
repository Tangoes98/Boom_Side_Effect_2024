using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BarrackStatus : ArchitectStatus {
    [Inspectable]
    public List<Minion> currentMinions; 
    [Inspectable,Range(1,4)]
    public int maxMinionNum;
    [Inspectable]
    public float coolDown;
    [Inspectable]
    public string minionCode;
    [Inspectable]
    public MinionModifierBase minionModifier;
    

}