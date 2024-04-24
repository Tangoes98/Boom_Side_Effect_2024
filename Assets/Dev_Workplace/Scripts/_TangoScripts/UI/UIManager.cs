using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("REFERENCE")]
    [SerializeField] TextMeshProUGUI _currentResource;
    [SerializeField] List<TextMeshProUGUI> _ENTexts;
    [SerializeField] List<TextMeshProUGUI> _CNTexts;



    private void Update()
    {
        UpdatePlayerResourcePanel();
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
