using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public static TitleUI Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

    }




    [Header("Title Menu Items")]
    [SerializeField] Button _startGame;
    [SerializeField] Button _setting;
    [SerializeField] Button _credits;
    [SerializeField] Button _quitGame;
    [SerializeField] Button _finishSetting;


    [Space(10)]
    [Header("Button Items")]
    [SerializeField] GameObject _settingPanel;


    [Space(10)]
    [Header("Locolization")]
    [SerializeField] UITexts _texts;
    private void Start()
    {


        ButtonEvents(_startGame, StartGame);
        ButtonEvents(_setting, OpenSetting);
        ButtonEvents(_credits, OpenCredits);
        ButtonEvents(_quitGame, QuitGame);
        ButtonEvents(_finishSetting, FinishSetting);
    }

    private void Update()
    {
        SwitchLanguage(SceneDataManager.Instance.CurrentLanguage);

    }



    #region Button events
    void StartGame()
    {
        UIFadeTransition.Instance.FadeIn();
        StartCoroutine(WaitForSecond());
        _settingPanel.SetActive(false);
    }
    IEnumerator WaitForSecond()
    {
        var timer = new WaitForSeconds(2);
        yield return timer;
        SceneManager.LoadScene(1);
        UIFadeTransition.Instance.FadeWait();

        //! GOing to the next scene
    }

    void OpenSetting()
    {
        _settingPanel.SetActive(true);
    }
    void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    void QuitGame()
    {
        Application.Quit();
    }
    void FinishSetting()
    {
        _settingPanel.SetActive(false);
    }

    #endregion
    #region ===============

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

    void ButtonEvents(Button btn, UnityAction buttonEvent)
    {
        btn.onClick.AddListener(buttonEvent);
    }



    #endregion

}
