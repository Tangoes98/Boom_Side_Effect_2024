using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingArtAssets : MonoBehaviour
{
    public float DissolveFinishValue;
    public bool CanAttack;
    public ParticleSystem AttackVFX;
    public List<GameObject> Subassets = new();
    public List<ParticleSystem> ParticleSystems = new();
}
