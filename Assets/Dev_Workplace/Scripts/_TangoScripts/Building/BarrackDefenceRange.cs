using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackDefenceRange : MonoBehaviour
{
    public float _Radius;

    void Update()
    {
        _Radius = GetComponentInParent<Architect>().Status().range;
        transform.localScale = new Vector3(_Radius, transform.localScale.y, _Radius);
    }

    public void EnabnleDefenceRange(bool bvalue)
    {
        this.gameObject.SetActive(bvalue);
    }
}
