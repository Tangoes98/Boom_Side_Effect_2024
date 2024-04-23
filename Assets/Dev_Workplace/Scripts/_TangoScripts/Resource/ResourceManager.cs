using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }

    public event Action DropResourcesEvent;




    [field: SerializeField] public int PlayerResource { get; private set; }

    //? Interact when building actions
    //? Interact when minion dies

    public bool CanBuild(int buildingCost)
    {
        if (buildingCost > PlayerResource) return false;

        PlayerResource -= buildingCost;
        if (PlayerResource < 0) PlayerResource = 0;
        return true;
    }

    public void DropResource(int resourceAmount)
    {
        DropResourcesEvent?.Invoke();
        StartCoroutine(DropResourceDelay(5f, resourceAmount));
    }
    IEnumerator DropResourceDelay(float waitTime, int resourceAmount)
    {
        var wait = new WaitForSeconds(waitTime);
        yield return wait;
        PlayerResource += resourceAmount;
    }


}
