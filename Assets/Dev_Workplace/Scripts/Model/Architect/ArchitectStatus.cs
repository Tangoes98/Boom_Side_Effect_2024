using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public abstract class ArchitectStatus {
    [Header("初始化/升级时'Status'会被'Base Info'覆盖")]
    [Space(10)]
    [Inspectable,Range(1,4)]
    public int maxLinkNum;
    [Inspectable]
    public float linkRange;
    [Inspectable]
    public float range;
    
}