using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        SwitchLanguage("EN");
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


    private void Start()
    {
        Cursor.SetCursor(_mouseCursorDefault, _hotSpot, _cursorMode);


    }
    private void Update()
    {
        //UpdatePlayerResourcePanel();
        // if (Input.GetMouseButtonDown(0))
        // {
        //     SceneManager.LoadScene(1);
        // }
        // if (Input.GetMouseButtonDown(1))
        // {
        //     SceneManager.LoadScene(0);
        // }

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
        _currentResource.text = $"CURRENT RESOURCE: {ResourceManager.Instance.PlayerResource}";
    }

    void SwitchLanguage(string language)
    {
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
