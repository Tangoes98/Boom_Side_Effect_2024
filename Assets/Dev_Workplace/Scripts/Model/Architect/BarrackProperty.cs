using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BarrackProperty : ArchitectProperty {
    public override ArchitectModiferBase[] GetModifers() => modifers;

    [Inspectable]
    public string minionCode;

    [Inspectable,Range(1,4)]
    public int maxMinionNum;
    [Inspectable]
    public float coolDown;
    [Inspectable]
    public BarrackModifierBase[] modifers;

}
