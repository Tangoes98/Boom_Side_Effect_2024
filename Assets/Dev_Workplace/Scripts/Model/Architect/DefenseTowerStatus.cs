using System;
using Unity.VisualScripting;

[Serializable]
public class DefenseTowerStatus : ArchitectStatus {
    [Inspectable]
    public float damage; 
    [Inspectable]
    public DamageType damageType;
    [Inspectable]
    public float attackSpeed;

}