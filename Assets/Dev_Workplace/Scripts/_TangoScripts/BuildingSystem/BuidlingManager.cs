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
    }

    public enum BuildingActionState
    {
        Preview, Building
    }

    [Header("REFERENCE")]
    [SerializeField] BuildingActionState _buildingActionStates;
    [SerializeField] List<ButtonBuildingPair> _buttonBuildingPairs;
    Dictionary<Button, GameObject> _buttonBuildingPairDictionary = new();
    Dictionary<Button, string> _buttonAndBuildingCodeDic = new();
    [SerializeField] Transform _buildingListObject;
    [SerializeField] Material _buildingShadowMaterial;
    [SerializeField] Material _buildingForbidMaterial;
    [SerializeField] Material _buildingDissolveMaterial;
    [SerializeField] GameObject _buildingPlopVFX;
    [SerializeField] float _dissolveTime;
    [SerializeField] float _dissolveSpeed;
    [SerializeField] GameObject _buildingUIPanel;


    [Header("DEBUG")]
    [SerializeField] GameObject _previewBuilding;
    [SerializeField] string _buildingCode;
    [field: SerializeField] public bool CanPlaceBuilding { get; private set; }
    [field: SerializeField] public bool HaveEnoughResourceToBuild { get; private set; }
    [SerializeField] bool _isBuilding;



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

        BuildingSelectionManager.Instance.UIPanelSelectionEvent += UIPanelSelectionEventAction;
    }
    private void Update()
    {

        switch (_buildingActionStates)
        {
            case BuildingActionState.Preview:
                if (!_previewBuilding) return;
                if (_isBuilding) return;
                BuildingValidation();
                UpdatePreviewBuildingPosition(_previewBuilding);
                break;
            case BuildingActionState.Building:
                if (_dissolveTime > 1) return;
                _buildingDissolveMaterial.SetFloat("_CutoffHeight", Mathf.Lerp(0f, 15f, _dissolveTime));
                _dissolveTime += Time.deltaTime * _dissolveSpeed;
                break;
        }



    }


    #region Public Methods




    #endregion
    #region Event Methods
    void PlaceBuildingEventAction()
    {

        //* Check if there is enough resource to build
        if (!ResourceManager.Instance.CanBuild(_previewBuilding.GetComponent<Architect>().GetBuildCost()))
        {
            HaveEnoughResourceToBuild = false;
            return;
        }
        else //* Build Action
        {
            HaveEnoughResourceToBuild = true;

            StartCoroutine(BuildingActionAnimation());

        }
        ArchiLinkManager.Instance.LinkFromClosestArchToPointer(false);
    }

    void CancelBuildingEventAction()
    {
        ArchiLinkManager.Instance.LinkFromClosestArchToPointer(false);
        Destroy(_previewBuilding);
        _previewBuilding = null;
    }

    void UIPanelSelectionEventAction(bool isBuildUIOn)
    {
        _buildingUIPanel.SetActive(isBuildUIOn);
    }




    #endregion
    #region Coroutine Methods
    IEnumerator BuildingActionAnimation()
    {
        var waitTime3s = new WaitForSeconds(3);
        var waitTime1s = new WaitForSeconds(1);

        _isBuilding = true;
        _previewBuilding.GetComponentInChildren<MeshRenderer>().material = _buildingDissolveMaterial;
        _dissolveTime = 0f;
        _buildingActionStates = BuildingActionState.Building;
        Vector3 buildingPos = _previewBuilding.transform.position;
        yield return waitTime3s;

        GameObject builfVFX = Instantiate(_buildingPlopVFX, buildingPos, Quaternion.identity);
        ArchiLinkManager.Instance.Build(_previewBuilding.transform.position, _buildingCode);
        Destroy(_previewBuilding);
        _previewBuilding = null;
        yield return waitTime1s;

        Destroy(builfVFX);
        _isBuilding = false;
        _buildingActionStates = BuildingActionState.Preview;

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
        CanPlaceBuilding = false;
        ArchiLinkManager.Instance.LinkFromClosestArchToPointer(true);
        _buildingActionStates = BuildingActionState.Preview;
        _buildingCode = _buttonAndBuildingCodeDic[btn];
        GameObject building = Instantiate(_buttonBuildingPairDictionary[btn], _buildingListObject);
        _previewBuilding = building;
        building.GetComponentInChildren<MeshRenderer>().material = _buildingShadowMaterial;




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
