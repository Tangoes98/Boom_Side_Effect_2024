using UnityEngine;

public class BuildingChecker : MonoBehaviour
{
    [field: SerializeField] public bool CanPlaceBuilding { get; private set; }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Building"))
    //     {
    //         CanPlaceBuild = false;
    //         return;
    //     }

    //     // if (!other.CompareTag("BuildingArea"))
    //     // {
    //     //     CanBuild = false;
    //     //     return;
    //     // }

    //     // if (other.CompareTag("NoBuildingArea"))
    //     // {
    //     //     CanBuild = false;
    //     //     return;
    //     // }


    //     CanPlaceBuild = true;
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Building"))
    //     {
    //         CanPlaceBuild = true;
    //         return;
    //     }

    //     // if (other.CompareTag("BuildingArea"))
    //     // {
    //     //     CanBuild = true;
    //     //     return;
    //     // }

    //     // if (other.CompareTag("NoBuildingArea"))
    //     // {
    //     //     CanBuild = true;
    //     //     return;
    //     // }


    //     CanPlaceBuild = false;
    // }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("BuildingArea"))
        {
            CanPlaceBuilding = false;
            return;
        }
        CanPlaceBuilding = true;
    }

}
