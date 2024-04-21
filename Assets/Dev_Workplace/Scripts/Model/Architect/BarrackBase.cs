using System;
using UnityEngine;

[Serializable]
public class BarrackBase : ArchitectBase {
    public static readonly ArchitectType type = ArchitectType.BARRACK;

    public override ArchitectProperty[] GetProperties() => properties;

    public float manufactureInterval;

    public float manufactureTime;
    public GameObject minionPrefab;

    public BarrackProperty[] properties;
}