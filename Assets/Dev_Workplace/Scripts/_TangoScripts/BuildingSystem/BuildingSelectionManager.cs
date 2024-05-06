using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] TextMeshProUGUI _upgradeCost;
    [SerializeField] TextMeshProUGUI _demolishReturn;

    [Header("DEBUG")]
    public Transform CurrentSelectedBuilding;



    public event Action<bool> UIPanelSelectionEvent;

    private void Start()
    {
        _buildingUIPanel.SetActive(false);

        //todo: Button funcitons
        _upgradeBtn.onClick.AddListener(UpgradeAction);
        _demolitionBtn.onClick.AddListener(DemolishAction);
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

        //* Update Upgrade and Demolish cost TEXT
        Architect building = CurrentSelectedBuilding.GetComponent<Architect>();
        if (!building.IsUpgradable())
        {
            _upgradeBtn.gameObject.SetActive(false);
            _upgradeCost.gameObject.SetActive(false);
        }
        else
        {
            _upgradeBtn.gameObject.SetActive(true);
            _upgradeCost.gameObject.SetActive(true);
            _upgradeCost.text = building.GetUpgradeCost().ToString();
        }

        _demolishReturn.text = Mathf.RoundToInt(building.GetBuildCost() * .8f).ToString();

        //*Enable the defence tower range preview
        var buildingAssets = CurrentSelectedBuilding.GetComponentInChildren<BuildingArtAssets>();
        buildingAssets.EnableDefencRangePreview(true);

        //* update tutorial
        if (TutorialUI.Instance.IsTutorialActive) TutorialUI.Instance.TutorialStep++;

        ShowSelectionPanels();
        MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.SelectionPanelInspection, null);

    }



    #region Publice methods
    public void CloseSelectionPanel()
    {
        //*Diable tower defence range preview
        if (CurrentSelectedBuilding != null)
        {
            var buildingAssets = CurrentSelectedBuilding.GetComponentInChildren<BuildingArtAssets>();
            buildingAssets.EnableDefencRangePreview(false);
        }

        SelectBuildingLinks(false);
        CurrentSelectedBuilding = null;
        _buildingUIPanel.SetActive(false);
        UIPanelSelectionEvent?.Invoke(true);

    }

    #endregion
    #region Event Methods
    void UpgradeAction()
    {
        if (!CurrentSelectedBuilding) return;
        Architect building = CurrentSelectedBuilding.GetComponent<Architect>();

        //* check if building is upgradeable
        if (!building.IsUpgradable())
        {
            Debug.Log("Cant Upgrade");
            return;
        }

        //* check if enough resources to upgrade 
        if (!ResourceManager.Instance.CanUpgrade(building.GetUpgradeCost())) return;

        building.Upgrade();
    }

    void DemolishAction()
    {
        if (!CurrentSelectedBuilding) return;
        Architect building = CurrentSelectedBuilding.GetComponent<Architect>();

        //*Return Resource to player
        ResourceManager.Instance.GainResorce(Mathf.RoundToInt(building.GetBuildCost() * .8f));

        MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.Selecting, CloseSelectionPanel);
        building.SelfDestroy();

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
        if (CurrentSelectedBuilding == null) return;

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

    void ShowSelectionPanels()
    {
        UIPanelSelectionEvent?.Invoke(false);
        _buildingUIPanel.SetActive(true);
        _buildingUIPanel.GetComponent<RectTransform>().anchoredPosition = GetBuildingScreenCanvasPosition(CurrentSelectedBuilding);
        SelectBuildingLinks(true);
    }








    #endregion
}
