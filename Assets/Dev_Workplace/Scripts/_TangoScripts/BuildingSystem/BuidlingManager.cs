using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuidlingManager : MonoBehaviour
{
    public static BuidlingManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }

    [System.Serializable]
    public struct ButtonBuildingPair
    {
        public Button Button;
        public string BuildingCode;
        public GameObject BuildingObject;
        public Material BuildingMaterial;

    }

    [SerializeField] List<ButtonBuildingPair> _buttonBuildingPairs;
    Dictionary<Button, GameObject> _buttonBuildingPairDictionary = new();
    Dictionary<Button, string> _buttonAndBuildingCodeDic = new();
    [SerializeField] Transform _buildingListObject;
    [SerializeField] Material _buildingShadowMaterial;
    [SerializeField] Material _buildingForbidMaterial;



    [Header("DEBUG")]
    [SerializeField] GameObject _previewBuilding;
    //[SerializeField] Material _previewBuildingMaterial;
    [SerializeField] string _buildingCode;
    [field: SerializeField] public bool CanPlaceBuilding { get; private set; }
    [field: SerializeField] public bool HaveEnoughResourceToBuild { get; private set; }



    private void Start()
    {
        AddButtonBuildingPairToDictionary(_buttonBuildingPairs, _buttonBuildingPairDictionary);
        AddBuildingCodeToDictionary(_buttonBuildingPairs, _buttonAndBuildingCodeDic);

        //* Button Event
        foreach (Button btn in _buttonBuildingPairDictionary.Keys)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }

        MouseStateManager.Instance.PlaceBuildingEvent += PlaceBuildingEventAction;
        MouseStateManager.Instance.CancelBuildingEvent += CancelBuildingEventAction;
    }
    private void Update()
    {
        if (!_previewBuilding) return;
        BuildingValidation();
        UpdatePreviewBuildingPosition(_previewBuilding);
    }

    #region Public Methods






    #endregion
    #region Event Methods
    void PlaceBuildingEventAction()
    {
        //_previewBuilding.GetComponentInChildren<MeshRenderer>().material = _previewBuildingMaterial;

        //* Check if there is enough resource to build
        if (!ResourceManager.Instance.CanBuild(_previewBuilding.GetComponent<Architect>().GetBuildCost()))
        {
            // Switch mouse state back to building
            // MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.Building,
            //                          () => { Debug.Log("Dont have enough resource to build"); });
            Debug.Log("Dont have enough resource to build");
            HaveEnoughResourceToBuild = false;
            return;
        }
        else
        {
            HaveEnoughResourceToBuild = true;
            ArchiLinkManager.Instance.Build(_previewBuilding.transform.position, _buildingCode);
            Destroy(_previewBuilding);
            _previewBuilding = null;
        }
        ArchiLinkManager.Instance.LinkFromClosestArchToPointer(false);

        //_previewBuildingMaterial = null;
    }

    void CancelBuildingEventAction()
    {
        ArchiLinkManager.Instance.LinkFromClosestArchToPointer(false);
        Destroy(_previewBuilding);
        _previewBuilding = null;
        //_previewBuildingMaterial = null;
    }




    #endregion
    #region ===============
    void AddButtonBuildingPairToDictionary(List<ButtonBuildingPair> list, Dictionary<Button, GameObject> dic)
    {
        foreach (var item in list)
        {
            dic.Add(item.Button, item.BuildingObject);
        }
    }
    void AddBuildingCodeToDictionary(List<ButtonBuildingPair> list, Dictionary<Button, string> dic)
    {
        foreach (var item in list)
        {
            dic.Add(item.Button, item.BuildingCode);
        }
    }

    void OnButtonClick(Button btn)
    {
        ArchiLinkManager.Instance.LinkFromClosestArchToPointer(true);
        CanPlaceBuilding = false;
        _buildingCode = _buttonAndBuildingCodeDic[btn];
        GameObject building = Instantiate(_buttonBuildingPairDictionary[btn], _buildingListObject);
        _previewBuilding = building;
        building.GetComponentInChildren<MeshRenderer>().material = _buildingShadowMaterial;

        // foreach (var item in _buttonBuildingPairs)
        // {
        //     if (item.BuildingObject != _buttonBuildingPairDictionary[btn]) continue;
        //     _previewBuildingMaterial = item.BuildingMaterial;
        // }

        MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.Building,
                                             () => { Debug.Log("EnterBuildingState"); });
    }

    void UpdatePreviewBuildingPosition(GameObject building)
    {
        building.transform.position = MouseController.Instance.GetMouseWorldPosition();
    }

    void BuildingValidation()
    {
        var buildingCheacker = _previewBuilding.GetComponent<BuildingChecker>();
        if (!buildingCheacker.CanBuild)
        {
            _previewBuilding.GetComponentInChildren<MeshRenderer>().material = _buildingForbidMaterial;
            CanPlaceBuilding = false;
        }
        else
        {
            _previewBuilding.GetComponentInChildren<MeshRenderer>().material = _buildingShadowMaterial;
            CanPlaceBuilding = true;
        }
    }

    #endregion

}
