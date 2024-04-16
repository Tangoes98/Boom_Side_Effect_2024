using System;
using Unity.VisualScripting;

[Serializable]
public abstract class ArchitectModiferBase {
    
    [Inspectable]
    public int sourceLinkNum;
    // add to base value 
    [Inspectable]
    public float probability;
    [Inspectable]
    public int maxLinkNum;
    [Inspectable]
    public float range;
}