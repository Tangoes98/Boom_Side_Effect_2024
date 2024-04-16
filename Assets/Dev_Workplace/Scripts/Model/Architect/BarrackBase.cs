using System;
using Unity.VisualScripting;

[Serializable]
public class BarrackBase : ArchitectBase {
    public static readonly ArchitectType type = ArchitectType.BARRACK;

    public override ArchitectProperty[] GetProperties() => properties;
    
    [Inspectable]
    public BarrackProperty[] properties;
}