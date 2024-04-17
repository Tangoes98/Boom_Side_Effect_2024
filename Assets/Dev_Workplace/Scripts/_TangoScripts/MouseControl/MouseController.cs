using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public static MouseController Instance;

    [SerializeField] LayerMask _mouseGroundLayerMask;

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
        transform.position = MouseWorldPosition(_mouseGroundLayerMask);


        // // For mouse cursor if possible
        // Debug.Log(GetMouseWorldPosition());
        // // // Check if mouse button is active
        // // Is_LMB_Down();
        // // Is_RMB_Down();
    }
    #region Public Methods
    public Vector3 GetMouseWorldPosition() => transform.position;




    #endregion
    #region =================


    private Vector3 MouseWorldPosition(LayerMask layerMask)
    {
        // Generate ray from main camera to the world
        // return the world position
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, layerMask);
        return raycastHit.point;
    }



    #endregion

}
