using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public static MouseController Instance;

    [SerializeField] LayerMask _mouseGroundLayerMask;
    [SerializeField] LayerMask _buildingLayerMask;



    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }

    void Update()
    {

        if (EventSystem.current.IsPointerOverGameObject()) return;
        transform.position = MouseRaycastHit(_mouseGroundLayerMask).point;

    }

    #region Public Methods
    public Vector3 GetMouseWorldPosition() => transform.position;
    public Transform GetSelectedBuilding() => MouseRaycastHit(_buildingLayerMask).transform;

    public static bool Is_LMB_Down()
    {
        if (Input.GetMouseButtonDown(0)) return true;
        else return false;
    }
    public static bool Is_RMB_Down()
    {
        if (Input.GetMouseButtonDown(1)) return true;
        else return false;
    }




    #endregion
    #region =================

    RaycastHit MouseRaycastHit(LayerMask layerMask)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return new RaycastHit();
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, layerMask);
        return raycastHit;
    }

    #endregion

}
