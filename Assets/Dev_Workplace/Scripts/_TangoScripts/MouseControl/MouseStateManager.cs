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
        Selecting, Building
    }
    public event Action PlaceBuildingEvent;
    public event Action CancelBuildingEvent;


    [SerializeField] private MouseStates _mouseStates;
    public MouseStates MouseState { get { return _mouseStates; } }


    private void Start()
    {

    }
    private void Update()
    {
        switch (_mouseStates)
        {
            case MouseStates.Selecting:
                if (!MouseController.Instance.GetSelectedBuilding())
                {
                    BuildingSelectionManager.Instance.CurrentSelectedBuilding = null;
                    return;
                }
                
                BuildingSelectionManager.Instance.CurrentSelectedBuilding = MouseController.Instance.GetSelectedBuilding();

                break;
            case MouseStates.Building:

                //*Check if Cancel the Building action
                if (MouseController.Is_RMB_Down())
                {
                    CancelBuilding();
                    return;
                }

                //*Check if Place the Building 
                if (!MouseController.Is_LMB_Down()) return;

                //*Check if is a valid position to place the Building
                if (!BuidlingManager.Instance.CanPlaceBuilding) return;

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
        if (!BuidlingManager.Instance.HaveEnoughResourceToBuild) return;
        SwitchState(MouseStates.Selecting, null);
    }
    void CancelBuilding()
    {
        CancelBuildingEvent?.Invoke();
        SwitchState(MouseStates.Selecting, null);
    }






    #endregion

}
