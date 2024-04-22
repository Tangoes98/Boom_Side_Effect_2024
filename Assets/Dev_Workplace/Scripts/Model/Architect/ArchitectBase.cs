using System;
using UnityEngine;

[Serializable]
public abstract class ArchitectBase
{
    public enum ArchitectType {
        DEFENCE_TOWER,
        BARRACK
    }

    public string code;

    public string name;

    public bool isMutant;

    [Range(1,4)]
    public int maxLinkNum = 4;

    //public float linkRange;

    public abstract ArchitectProperty[] GetProperties();

}
