using System;
using Unity.VisualScripting;

[Serializable]
public class DefenseTowerModifierBase : ArchitectModiferBase {
    
    [Inspectable]
    public float damage;
    [Inspectable]
    public DamageType damageType; // change damage type
    [Inspectable]
    public float attackSpeed;
}