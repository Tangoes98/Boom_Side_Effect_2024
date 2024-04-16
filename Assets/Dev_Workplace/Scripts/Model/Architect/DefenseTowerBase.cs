using System;
using Unity.VisualScripting;

[Serializable]
public class DefenseTowerBase : ArchitectBase {
    public static readonly ArchitectType type = ArchitectType.DEFENCE_TOWER;

    public override ArchitectProperty[] GetProperties() => properties;

    [Inspectable]
    public DefenseTowerProperty[] properties;
}