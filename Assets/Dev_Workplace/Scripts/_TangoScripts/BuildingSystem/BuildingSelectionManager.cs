using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelectionManager : MonoBehaviour
{
    public static BuildingSelectionManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }

    public Transform CurrentSelectedBuilding;










}
