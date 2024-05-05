using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDefenceRange : MonoBehaviour
{

    public float _Radius;


    void Update()
    {
        transform.localScale = new Vector3(_Radius, transform.localScale.y, _Radius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _Radius);
    }
}
