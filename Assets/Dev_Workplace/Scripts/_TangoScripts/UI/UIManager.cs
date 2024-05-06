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
        SwitchLanguage("CN");
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


    private void Start()
    {
        Cursor.SetCursor(_mouseCursorDefault, _hotSpot, _cursorMode);


    }
    private void Update()
    {
        UpdatePlayerResourcePanel();


        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchLanguage("CN");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SwitchLanguage("EN");
        }

    }
    void OnMouseEnter()
    {
    }



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
