using System;
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



    public event Action<bool> UIPanelSelectionEvent;

    private void Start()
    {
        _buildingUIPanel.SetActive(false);

        //todo: Button funcitons
    }

    private void Update()
    {

        if (MouseStateManager.Instance.MouseState != MouseStateManager.MouseStates.Selecting) return;

        //*Close Selection UI Panel
        if (MouseController.Is_LMB_Down() && CurrentSelectedBuilding != MouseController.Instance.GetSelectedBuilding())
        {
            CloseSelectionPanel();
            return;
        }


        //* Open Selection UI Panel
        if (!MouseController.Is_LMB_Down()) return;
        if (!CurrentSelectedBuilding) return;

        UIPanelSelectionEvent?.Invoke(false);
        _buildingUIPanel.SetActive(true);
        _buildingUIPanel.GetComponent<RectTransform>().anchoredPosition = GetBuildingScreenCanvasPosition(CurrentSelectedBuilding);
        SelectBuildingLinks(true);
        MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.SelectionPanelInspection, null);


    }



    #region Publice methods
    public void CloseSelectionPanel()
    {
        SelectBuildingLinks(false);
        CurrentSelectedBuilding = null;
        _buildingUIPanel.SetActive(false);
        UIPanelSelectionEvent?.Invoke(true);
    }






    #endregion
    #region ========================
    Vector3 GetBuildingScreenCanvasPosition(Transform building)
    {
        var rectTransform = _buildingUIPanel.GetComponent<RectTransform>();
        Vector3 buildingPosWorldToviewport = Camera.main.WorldToViewportPoint(building.position);
        Vector3 buildingScreenPosition = new(buildingPosWorldToviewport.x * rectTransform.sizeDelta.x - rectTransform.sizeDelta.x * .5f,
                                            buildingPosWorldToviewport.y * rectTransform.sizeDelta.y - rectTransform.sizeDelta.y * .5f,
                                            0f);
        return -buildingScreenPosition;
    }

    void SelectBuildingLinks(bool showLink)
    {
        Architect building = CurrentSelectedBuilding.GetComponent<Architect>();

        ArchiLinkManager.Instance.GetInAndOutAndPauseLinks(building,
                                                        out List<Link> incomingLinks,
                                                        out List<Link> outgoingLinks,
                                                        out List<Link> pausedLinks);
        if (showLink)
        {
            ShowLinks(incomingLinks);
            ShowLinks(outgoingLinks);
            ShowLinks(pausedLinks);
        }
        else
        {
            HideLinks(incomingLinks);
            HideLinks(outgoingLinks);
            HideLinks(pausedLinks);
        }
    }

    void ShowLinks(List<Link> linkList)
    {
        foreach (var item in linkList)
        {
            item.ShowLine();
        }
    }
    void HideLinks(List<Link> linkList)
    {
        foreach (var item in linkList)
        {
            item.HideLine();
        }
    }










    #endregion
}
