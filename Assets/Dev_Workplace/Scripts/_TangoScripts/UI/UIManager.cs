using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        //SwitchLanguage(SceneDataManager.Instance.CurrentLanguage);
        // SwitchLanguage("CN");
    }


    [Header("MouseCursor")]
    [SerializeField] Texture2D _mouseCursorDefault;
    [SerializeField] Texture2D _mouseCursorSelected;
    Vector2 _hotSpot = Vector2.zero;
    CursorMode _cursorMode = CursorMode.Auto;

    [Space(10f)]
    [Header("Resource")]
    [SerializeField] TextMeshProUGUI _currentResource;

    [Space(10f)]
    [Header("Locolization")]
    [SerializeField] UITexts _texts;

    [Space(10f)]
    [Header("ButtonFunction")]
    [SerializeField] Toggle _pauseGameToggle;
    [SerializeField] Button _settingButton;
    [SerializeField] GameObject _settingPanel;
    [SerializeField] Button _finishSettingButton;
    [SerializeField] Button _returnToTitleButton;
    [SerializeField] GameObject _gamePausedPanel;

    private void Start()
    {
        Cursor.SetCursor(_mouseCursorDefault, _hotSpot, _cursorMode);

        _pauseGameToggle.onValueChanged.AddListener(ToggleButtonEventAction);
        _settingButton.onClick.AddListener(SettingButtonEventAction);
        _finishSettingButton.onClick.AddListener(FinishSettingButtonEventAction);
        _returnToTitleButton.onClick.AddListener(ReturnToTitleEventAction);

    }
    private void Update()
    {
        UpdatePlayerResourcePanel();

        if (SceneDataManager.Instance)
        {
            SwitchLanguage(SceneDataManager.Instance.CurrentLanguage);
        }
        else
        {
            Debug.Log("Current language not selected");
            SwitchLanguage("CN");
        }

        if (InputManager.IS_ESC_DOWN())
        {
            if (_settingPanel.activeSelf) _settingPanel.SetActive(false);
            else _settingPanel.SetActive(true);
        }

        if (InputManager.IS_SPACE_DOWN())
        {
            _pauseGameToggle.isOn = !_pauseGameToggle.isOn;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchLanguage("CN");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SwitchLanguage("EN");
        }

    }






    #region Button Events
    void ToggleButtonEventAction(bool bvalue)
    {
        InputManager.Instance.PauseGame(bvalue);
        if (_gamePausedPanel.activeSelf) _gamePausedPanel.SetActive(false);
        else _gamePausedPanel.SetActive(true);
    }
    void SettingButtonEventAction()
    {
        _settingPanel.SetActive(true);
    }
    void FinishSettingButtonEventAction()
    {
        _settingPanel.SetActive(false);
    }
    void ReturnToTitleEventAction()
    {
        SceneManager.LoadScene(0);
    }


    #endregion
    #region ===============
    void UpdatePlayerResourcePanel()
    {
        _currentResource.text = ResourceManager.Instance.PlayerResource.ToString();
    }

    void SwitchLanguage(string language)
    {
        if (_texts == null) return;

        switch (language)
        {
            case "EN":
                LanguageChange(_texts.ENTexts, true);
                LanguageChange(_texts.CNTexts, false);
                break;
            case "CN":
                LanguageChange(_texts.ENTexts, false);
                LanguageChange(_texts.CNTexts, true);
                break;
        }
    }

    void LanguageChange(List<TextMeshProUGUI> texts, bool isActive)
    {
        foreach (var item in texts)
        {
            item.gameObject.SetActive(isActive);
        }
    }



    #endregion

}
