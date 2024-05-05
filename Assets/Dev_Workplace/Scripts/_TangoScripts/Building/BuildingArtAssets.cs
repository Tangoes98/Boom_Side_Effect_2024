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

    public bool IsTower;
    public BuildingDefenceRange TowerDefenceRangePreview;

    private void Start()
    {
        //if (IsTower) TowerDefenceRangePreview.EnabnleDefenceRange(false);

        AttackVFX.SetActive(false);



    }



}
