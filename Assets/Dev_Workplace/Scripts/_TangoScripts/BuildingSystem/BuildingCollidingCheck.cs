using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollidingCheck : MonoBehaviour
{
    [field: SerializeField] public bool CanBuild { get; private set; }

    bool _colliderStayCheck;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            _colliderStayCheck = false;
            CanBuild = false;
            return;
        }

        if (!other.CompareTag("BuildingArea"))
        {
            _colliderStayCheck = false;
            CanBuild = false;
            return;
        }

        _colliderStayCheck = true;
        CanBuild = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            _colliderStayCheck = true;
            CanBuild = true;
            return;
        }

        if (other.CompareTag("BuildingArea"))
        {
            CanBuild = true;
            return;
        }

        CanBuild = false;
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.CompareTag("BuildingArea") && _colliderStayCheck)
    //     {
    //         CanBuild = true;
    //         return;
    //     }
    //     CanBuild = false;
    // }
}
