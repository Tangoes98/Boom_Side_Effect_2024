using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebugScripts : MonoBehaviour
{

    public GameObject _DebugPanel;
    [Space(15)]
    public Button _AddResource;
    public int _GainResourceAmount;
    [Space(15)]
    public Button _SpeedUp;
    public float _GameSpeed;
    public TextMeshProUGUI Timer;
    float timer = 0f;
    [Space(15)]
    public Button _SkipTutorial;
    [Space(15)]
    public MainBase _MainBase;
    public Button _NeverDie;

    private void Start()
    {
        ButtonEvent(_AddResource, AddResourceAction);
        ButtonEvent(_SpeedUp, SpeedUpAction);
        ButtonEvent(_SkipTutorial, SkipTutorialAction);
        ButtonEvent(_NeverDie, NeverDieAction);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (_DebugPanel.activeSelf) _DebugPanel.SetActive(false);
            else _DebugPanel.SetActive(true);
        }

        timer += Time.deltaTime;
        Timer.text = timer.ToString();
    }


    void AddResourceAction()
    {
        ResourceManager.Instance.GainResorce(_GainResourceAmount);
    }

    void SpeedUpAction()
    {
        if (InputManager.Instance._IsSpeedTakingControl)
            InputManager.Instance._IsSpeedTakingControl = false;
        else
        {
            InputManager.Instance._IsSpeedTakingControl = true;
            Time.timeScale = _GameSpeed;
        }
    }
    void SkipTutorialAction()
    {
        TutorialUI.Instance.TutorialStep = 13;
    }
    void NeverDieAction()
    {
        _MainBase.health = float.MaxValue;
    }

    #region ==========

    void ButtonEvent(Button btn, UnityAction uaction)
    {
        btn.onClick.AddListener(uaction);
    }
    #endregion




}
