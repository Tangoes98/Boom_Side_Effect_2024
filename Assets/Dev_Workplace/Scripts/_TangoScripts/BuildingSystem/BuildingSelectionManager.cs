using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Reference")]
    [SerializeField] GameObject _buildingUIPanel;
    [SerializeField] Button _upgradeBtn;
    [SerializeField] Button _demolitionBtn;

    [Header("DEBUG")]
    public Transform CurrentSelectedBuilding;
    public Transform Cube;


    private void Start()
    {
        _buildingUIPanel.SetActive(false);
    }

    private void Update()
    {
        //_buildingUIPanel.transform.LookAt(Camera.main.transform);

        if (MouseStateManager.Instance.MouseState != MouseStateManager.MouseStates.Selecting) return;
        
        // //*Close Selection UI Panel
        // if (!CurrentSelectedBuilding)
        // {
        //     _buildingUIPanel.SetActive(false);
        //     return;
        // }

        //*Close Selection UI Panel
        if (MouseController.Is_RMB_Down())
        {
            CurrentSelectedBuilding = null;
            _buildingUIPanel.SetActive(false);
            return;
        }

        if (!MouseController.Is_LMB_Down()) return;

        _buildingUIPanel.SetActive(true);
        _buildingUIPanel.GetComponent<RectTransform>().anchoredPosition = GetBuildingScreenCanvasPosition(CurrentSelectedBuilding);




    }

    Vector3 GetBuildingScreenCanvasPosition(Transform building)
    {
        var rectTransform = _buildingUIPanel.GetComponent<RectTransform>();
        Vector3 buildingPosWorldToviewport = Camera.main.WorldToViewportPoint(building.position);
        Vector3 buildingScreenPosition = new(buildingPosWorldToviewport.x * rectTransform.sizeDelta.x - rectTransform.sizeDelta.x * .5f,
                                            buildingPosWorldToviewport.y * rectTransform.sizeDelta.y - rectTransform.sizeDelta.y * .5f,
                                            0f);
        return -buildingScreenPosition;
    }





}
