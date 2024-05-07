using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Preview, PlayBuildingDissolveEffect
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
    [field: SerializeField] public bool CanPlopBuilding { get; private set; }
    [field: SerializeField] public bool HaveEnoughResourceToBuild { get; private set; }
    [SerializeField] bool _isBuilding;
    BuildingArtAssets _buildingAssets;

    public Barrack PreviewBarrack { get; private set; }


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
            case BuildingActionState.PlayBuildingDissolveEffect:
                _buildingDissolveMaterial.SetFloat("_CutoffHeight", Mathf.Lerp(0f, _buildingAssets.DissolveFinishValue, _dissolveTime));
                _dissolveTime += Time.deltaTime * _dissolveSpeed;
                if (_buildingDissolveMaterial.GetFloat("_CutoffHeight") == _buildingAssets.DissolveFinishValue)
                {
                    StartCoroutine(BuildingAction());
                    _buildingActionStates = BuildingActionState.Preview;
                }
                break;
        }



    }


    #region Public Methods




    #endregion
    #region Event Methods
    void PlaceBuildingEventAction()
    {
        if (TutorialUI.Instance.IsTutorialActive) TutorialUI.Instance.TutorialStep++;

        //* Check if there is enough resource to build
        if (!ResourceManager.Instance.CanBuild(_previewBuilding.GetComponent<Architect>().GetBuildCost()))
        {
            HaveEnoughResourceToBuild = false;
            return;
        }
        else //* Build Action
        {
            HaveEnoughResourceToBuild = true;
            _buildingUIPanel.SetActive(false);
            MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.None, null);

            //*Diable defence range preview
            _buildingAssets.EnableDefencRangePreview(false);
            //*Diable Upgrade VFX
            _buildingAssets.BuidldingUpgradeVFX.gameObject.SetActive(false);

            _isBuilding = true;
            foreach (var item in _buildingAssets.Subassets)
            {
                item.GetComponent<MeshRenderer>().material = _buildingDissolveMaterial;
            }
            _dissolveTime = 0f;
            _buildingActionStates = BuildingActionState.PlayBuildingDissolveEffect;

            //*Play SFX
            AudioManager.Instance.PlayBuildingSFX(AudioManager.BuildingSFX.Build);
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
    IEnumerator BuildingAction()
    {
        var wait1Time = new WaitForSeconds(1);
        _buildingUIPanel.SetActive(true);
        MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.Selecting, null);

        Vector3 buildingPos = _previewBuilding.transform.position;
        GameObject builfVFX = Instantiate(_buildingPlopVFX, buildingPos, Quaternion.identity);
        var newBuild = ArchiLinkManager.Instance.Build(buildingPos, _buildingCode);

        //*Diable defence range preview
        newBuild.GetComponentInChildren<BuildingArtAssets>().EnableDefencRangePreview(false);
        //*Diable Upgrade VFX
        newBuild.GetComponentInChildren<BuildingArtAssets>().BuidldingUpgradeVFX.gameObject.SetActive(false);


        Destroy(_previewBuilding);

        yield return wait1Time;
        Destroy(builfVFX);
        _isBuilding = false;

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
    #region Button Click Event

    void OnButtonClick(Button btn)
    {
        CanPlopBuilding = false;

        //*Show Links
        ArchiLinkManager.Instance.LinkFromClosestArchToPointer(true);

        //*Switch Mouse States
        _buildingActionStates = BuildingActionState.Preview;

        //*Get and build preview of the building
        _buildingCode = _buttonAndBuildingCodeDic[btn];
        GameObject building = Instantiate(_buttonBuildingPairDictionary[btn], _buildingListObject);
        _previewBuilding = building;
        _previewBuilding.GetComponent<Architect>().IsPreview = true;
        PreviewBarrack = _previewBuilding.GetComponent<Barrack>();

        //* Switch all material into preview material
        _buildingAssets = building.GetComponentInChildren<BuildingArtAssets>();
        foreach (var item in _buildingAssets.Subassets)
        {
            item.GetComponent<MeshRenderer>().material = _buildingShadowMaterial;
        }

        //*Enable defence range preview
        _buildingAssets.EnableDefencRangePreview(true);
        //*Diable Upgrade VFX
        _buildingAssets.BuidldingUpgradeVFX.gameObject.SetActive(false);


        //* Stop all buidling VFX
        if (_buildingAssets.CanAttack && _buildingAssets.AttackVFX != null) _buildingAssets.AttackVFX.SetActive(false);
        if (_buildingAssets.ParticleSystems.Count > 0)
        {
            foreach (var item in _buildingAssets.ParticleSystems)
            {
                item.gameObject.SetActive(false);
            }
        }

        //* update mouse state
        MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.Building,
                                             () => { Debug.Log("EnterBuildingState"); });

        //* Play SFX
        AudioManager.Instance.PlayBuildingSFX(AudioManager.BuildingSFX.Selected);

        if (TutorialUI.Instance.IsTutorialActive) TutorialUI.Instance.TutorialStep++;
    }

    #endregion
    void UpdatePreviewBuildingPosition(GameObject building)
    {
        building.transform.position = MouseController.Instance.GetMouseWorldPosition();
    }

    void BuildingValidation()
    {
        var buildingCollidCheacker = _previewBuilding.GetComponent<BuildingCollidingCheck>();
        //var buildingCheacker = _previewBuilding.GetComponentInChildren<BuildingChecker>();

        if (!buildingCollidCheacker.CanBuild)
        {

            foreach (var item in _buildingAssets.Subassets)
            {
                var materials = item.GetComponent<MeshRenderer>().materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = _buildingForbidMaterial;
                }
            }
            CanPlopBuilding = false;
        }
        else if (buildingCollidCheacker.CanBuild)
        {

            foreach (var item in _buildingAssets.Subassets)
            {
                var materials = item.GetComponent<MeshRenderer>().materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = _buildingShadowMaterial;
                }
            }
            CanPlopBuilding = true;
        }
    }

    #endregion

}
