using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleSettingPanel : MonoBehaviour
{
    public static TitleSettingPanel Instance;
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
        return;
    }

    [SerializeField] Toggle _ENtoggle;
    [SerializeField] Toggle _CNtoggle;

    [Space(15)]

    [SerializeField] Slider _sliderMusic;
    [SerializeField] TextMeshProUGUI _musicText;
    [SerializeField] Slider _sliderSFX;
    [SerializeField] TextMeshProUGUI _SFXText;


    private void Start()
    {
        InitializeCN();
        _ENtoggle.onValueChanged.AddListener(UpdateENToggle);
        _CNtoggle.onValueChanged.AddListener(UpdateCNToggle);
    }

    private void Update()
    {
        UpdateCurrentLanguage();

        AudioManager.Instance._MusicVolume = UpdateSoundVolume(_sliderMusic, _musicText);
        AudioManager.Instance._SFXVolume = UpdateSoundVolume(_sliderSFX, _SFXText);
    }

    #region Language
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
    #endregion
    #region Sound
    float UpdateSoundVolume(Slider slider, TextMeshProUGUI text)
    {
        text.text = Mathf.RoundToInt(slider.value * 100).ToString();
        return slider.value;
    }
    #endregion





}
