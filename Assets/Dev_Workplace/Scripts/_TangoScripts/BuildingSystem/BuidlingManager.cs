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
        public GameObject BuildingObject;
        public Material BuildingMaterial;

    }

    [SerializeField] List<ButtonBuildingPair> _buttonBuildingPairs;
    Dictionary<Button, GameObject> _buttonBuildingPairDictionary = new();
    [SerializeField] Transform _buildingListObject;
    [SerializeField] Material _buildingShadowMaterial;
    [SerializeField] Material _buildingForbidMaterial;



    [Header("DEBUG")]
    [SerializeField] GameObject _previewBuilding;
    [SerializeField] Material _previewBuildingMaterial;
    [field: SerializeField] public bool CanPlaceBuilding { get; private set; }



    private void Start()
    {
        AddButtonBuildingPairToDictionary(_buttonBuildingPairs, _buttonBuildingPairDictionary);

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
        _previewBuilding.GetComponentInChildren<MeshRenderer>().material = _previewBuildingMaterial;
        _previewBuildingMaterial = null;
        _previewBuilding = null;
    }

    void CancelBuildingEventAction()
    {
        Destroy(_previewBuilding);
        _previewBuildingMaterial = null;
        _previewBuilding = null;
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

    void OnButtonClick(Button btn)
    {
        CanPlaceBuilding = false;
        GameObject building = Instantiate(_buttonBuildingPairDictionary[btn], _buildingListObject);
        _previewBuilding = building;
        building.GetComponentInChildren<MeshRenderer>().material = _buildingShadowMaterial;

        foreach (var item in _buttonBuildingPairs)
        {
            if (item.BuildingObject != _buttonBuildingPairDictionary[btn]) continue;
            _previewBuildingMaterial = item.BuildingMaterial;
        }

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
