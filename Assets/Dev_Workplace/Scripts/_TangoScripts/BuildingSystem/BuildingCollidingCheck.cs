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
            CanBuild = IsSpawnPointInRangeIfBarrack();
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
            CanBuild = IsSpawnPointInRangeIfBarrack();
            return;
        }

        if (other.CompareTag("BuildingArea"))
        {
            CanBuild = false;
            return;
        }

        if (other.CompareTag("NoBuildingArea"))
        {
            CanBuild = IsSpawnPointInRangeIfBarrack();
            return;
        }

        //CanBuild = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NoBuildingArea") || other.CompareTag("Building"))
        {
            CanBuild = false;
            return;
        } 
        CanBuild = IsSpawnPointInRangeIfBarrack();
        
    }
    private bool IsSpawnPointInRangeIfBarrack() {
        if(BuidlingManager.Instance.PreviewBarrack==null) {
            return true;
        }
        return BuidlingManager.Instance.PreviewBarrack.GetIdlePosition()!=Vector3.zero;
    }
}
