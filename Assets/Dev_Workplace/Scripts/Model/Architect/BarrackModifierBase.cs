using System;
using Unity.VisualScripting;

[Serializable]
public class BarrackModiferBase : ArchitectModiferBase {

    [Inspectable]
    public int maxMinionNum;
    [Inspectable]
    public float coolDown;
    [Inspectable]
	public MinionModifierBase minionModifier;

}