using System;
using UnityEngine;

[Serializable]
public class ArchitectModiferCase {
    [Header("无需申明建筑100%效果的情况")]

    [Range(1,100)]
    public int probability=50;

    public float modifier;

    public ModifierType type;
}

public enum ModifierType {
    NONE,BUFF,DEBUFF
}

