using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public abstract class ArchitectModiferBase {
    
    [Inspectable,Range(1,4)]
    public int sourceLinkNum;
    // add to base value 
    [Inspectable,Range(0,1)]
    public float probability;
    [Inspectable,Range(0,4)]
    public int maxLinkNum;
    [Inspectable]
    public float range;
}