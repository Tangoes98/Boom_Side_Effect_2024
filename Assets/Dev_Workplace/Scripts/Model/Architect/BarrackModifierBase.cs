using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BarrackModifierBase : ArchitectModiferBase {

    [Inspectable,Range(0,4)]
    public int maxMinionNum;
    [Inspectable]
    public float coolDown;
    [Inspectable]
	public MinionModifierBase minionModifier;

}