using System;
using Unity.VisualScripting;

[Serializable]
public class DefenseTowerProperty : ArchitectProperty {
    public override ArchitectModiferBase[] GetModifers() => modifers;

    [Inspectable]
    public float damage;
    [Inspectable]
    public DamageType damageType;
    [Inspectable]
    public float attackSpeed;
    [Inspectable]
    public DefenseTowerModifierBase[] modifers;

}
