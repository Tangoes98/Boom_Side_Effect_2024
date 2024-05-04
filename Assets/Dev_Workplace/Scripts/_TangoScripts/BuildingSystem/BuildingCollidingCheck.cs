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
            //_colliderStayCheck = false;
            CanBuild = false;
            return;
        }
        //else CanBuild = true;

        // if (!other.CompareTag("BuildingArea"))
        // {
        //     _colliderStayCheck = false;
        //     CanBuild = false;
        //     return;
        // }

        if (other.CompareTag("BuildingArea"))
        {
            //_colliderStayCheck = false;
            CanBuild = true;
            return;
        }
        //else CanBuild = false;

        if (other.CompareTag("NoBuildingArea"))
        {
            //_colliderStayCheck = false;
            CanBuild = false;
            return;
        }
        //else CanBuild = true;

        // _colliderStayCheck = true;
        // CanBuild = true;

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
            CanBuild = false;
            return;
        }

        if (other.CompareTag("NoBuildingArea"))
        {
            CanBuild = true;
            return;
        }

        //CanBuild = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NoBuildingArea"))
        {
            CanBuild = false;
            return;
        }
        
    }
}
