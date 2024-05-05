using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitArtAssets : MonoBehaviour
{
    public bool _IsAOE;
    public GameObject _AttackVFX;

    private void Start()
    {
        _AttackVFX.SetActive(false);
    }
}
