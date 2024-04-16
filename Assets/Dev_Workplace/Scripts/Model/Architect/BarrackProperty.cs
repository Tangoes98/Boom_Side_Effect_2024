using System;
using Unity.VisualScripting;

[Serializable]
public class BarrackProperty : ArchitectProperty {
    public override ArchitectModiferBase[] GetModifers() => modifers;

    [Inspectable]
    public MinionBase minionBase;
    [Inspectable]
    public int maxMinionNum;
    [Inspectable]
    public float coolDown;
    [Inspectable]
    public BarrackModiferBase[] modifers;

}
