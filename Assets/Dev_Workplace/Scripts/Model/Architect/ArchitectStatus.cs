using System;
using Unity.VisualScripting;

[Serializable]
public abstract class ArchitectStatus {
    [Inspectable]
    public int maxLinkNum;
    [Inspectable]
    public int existingLinkNum;
    [Inspectable]
    public int activeOutputLinkNum;
    [Inspectable]
    public float linkRange;
    [Inspectable]
    public float range;
    [Inspectable]
    public ArchitectModiferBase modifer;
}