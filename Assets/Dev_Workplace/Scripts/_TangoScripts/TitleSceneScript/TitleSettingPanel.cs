using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSettingPanel : MonoBehaviour
{

    [SerializeField] Toggle _ENtoggle;
    [SerializeField] Toggle _CNtoggle;

    private void Start()
    {
        //InitializeEN();
        InitializeCN();
        _ENtoggle.onValueChanged.AddListener(UpdateENToggle);
        _CNtoggle.onValueChanged.AddListener(UpdateCNToggle);
    }

    private void Update()
    {
        UpdateCurrentLanguage();
    }

    void UpdateCurrentLanguage()
    {
        if (_ENtoggle.isOn) SceneDataManager.Instance.CurrentLanguage = "EN";
        else SceneDataManager.Instance.CurrentLanguage = "CN";
    }

    void UpdateENToggle(bool value)
    {
        _ENtoggle.isOn = value;
        _CNtoggle.isOn = !value;
    }
    void UpdateCNToggle(bool value)
    {
        _CNtoggle.isOn = value;
        _ENtoggle.isOn = !value;
    }

    void InitializeEN()
    {
        _ENtoggle.isOn = true;
        _CNtoggle.isOn = false;
    }
    void InitializeCN()
    {
        _CNtoggle.isOn = true;
        _ENtoggle.isOn = false;
    }





}
