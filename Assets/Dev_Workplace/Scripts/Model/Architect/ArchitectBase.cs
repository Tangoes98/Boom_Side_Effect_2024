using System;
using Unity.VisualScripting;

[Serializable]
public abstract class ArchitectBase
{
    public enum ArchitectType {
        DEFENCE_TOWER,
        BARRACK
    }
    [Inspectable]
    public string code;
    [Inspectable]
    public string name;
    [Inspectable]
    public bool isMutant;

    public abstract ArchitectProperty[] GetProperties();

}
