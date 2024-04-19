using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStateManager : MonoBehaviour
{
    public static MouseStateManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }

    public enum MouseStates
    {
        None, Building
    }
    public Action PlaceBuildingEvent;


    private MouseStates _mouseStates;



    private void Start()
    {

    }
    private void Update()
    {
        switch (_mouseStates)
        {
            case MouseStates.None:
                break;
            case MouseStates.Building:
                if (!MouseController.Is_LMB_Down()) return;
                PlaceBuilding();

                break;
        }
    }

    #region Public Methods
    public void SwitchState(MouseStates state, Action enterState)
    {
        _mouseStates = state;
        enterState?.Invoke();
    }

    #endregion
    #region ==================

    void PlaceBuilding()
    {
        PlaceBuildingEvent?.Invoke();
        SwitchState(MouseStates.None, null);
    }




    #endregion

}
