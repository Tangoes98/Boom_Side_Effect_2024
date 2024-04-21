using System;
using UnityEngine;

[Serializable]
public class ArchitectModifer {
    [Header("加成：乘法")]
    
    [Range(2,4)]
    public int unstability;
    public ArchitectModiferCase[] modifierCases;
}

