using System;
using UnityEngine;

[Serializable]
public abstract class ArchitectStatus {
    [Header("初始化/升级时'Status'会被'Base Info'覆盖")]
    [Space(10)]

    public float range;

    
}