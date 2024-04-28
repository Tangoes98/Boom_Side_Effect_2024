using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
    [SerializeField] List<TextMeshProUGUI> _ENTexts;
    [SerializeField] List<TextMeshProUGUI> _CNTexts;


    private void Start()
    {
        Cursor.SetCursor(_mouseCursorDefault, _hotSpot, _cursorMode);

    }
    private void Update()
    {
        UpdatePlayerResourcePanel();
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
                LanguageChange(_ENTexts, true);
                LanguageChange(_CNTexts, false);
                break;
            case "CN":
                LanguageChange(_ENTexts, false);
                LanguageChange(_CNTexts, true);
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
