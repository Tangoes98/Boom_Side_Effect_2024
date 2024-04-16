using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public abstract class ArchitectProperty {
    
    [Inspectable,Range(1,5)]
    public int level;
    [Inspectable,Range(1,4)]
    public int maxLinkNum;
    [Inspectable]
    public float linkRange;
    [Inspectable]
    public float range;

    public abstract ArchitectModiferBase[] GetModifers();
    
}
