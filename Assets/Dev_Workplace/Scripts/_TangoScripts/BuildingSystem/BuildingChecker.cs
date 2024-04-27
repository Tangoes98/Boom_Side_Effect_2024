using UnityEngine;

public class BuildingChecker : MonoBehaviour
{
    [field: SerializeField] public bool CanBuild { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            CanBuild = false;
            return;
        }

        if (!other.CompareTag("BuildingArea"))
        {
            CanBuild = false;
            return;
        }


        CanBuild = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            CanBuild = true;
            return;
        }
        
        if (!other.CompareTag("BuildingArea"))
        {
            CanBuild = true;
            return;
        }


        CanBuild = false;
    }

}
