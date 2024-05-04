using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingArtAssets : MonoBehaviour
{
    public float DissolveFinishValue;
    public bool CanAttack;
    public bool IsAOEAttack;
    public GameObject AttackVFX;
    public List<GameObject> Subassets = new();
    public List<ParticleSystem> ParticleSystems = new();
}
