using System;
using Unity.VisualScripting;

[Serializable]
public abstract class ArchitectProperty {
    
    [Inspectable]
    public int level;
    [Inspectable]
    public int maxLinkNum;
    [Inspectable]
    public float linkRange;
    [Inspectable]
    public float range;

    public abstract ArchitectModiferBase[] GetModifers();
    
}
