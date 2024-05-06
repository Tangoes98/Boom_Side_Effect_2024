using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public int TutorialStep;
    public bool IsTutorialActive;
    public int LineClickCount;
    [SerializeField] List<GameObject> _tutorialScenes;
    [SerializeField] Button _battleStartButton;
    public float Timer;
    float _timer;
    bool _isActive;

    [Header("DEBUG")]
    public bool EnableTutorial;

    private void Start()
    {
        if (!EnableTutorial) return;

        TutorialStep = 0;
        IsTutorialActive = true;
        _battleStartButton.gameObject.SetActive(false);
        _timer = Timer;
        _isActive = true;
    }


    private void Update()
    {
        if (!_isActive) return;

        UpdateTutorialScene(TutorialStep);

        switch (TutorialStep)
        {
            case 4:
                MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.Tutorial, null);
                _timer -= Time.deltaTime;
                if (_timer > 0) return;
                _timer = Timer;
                TutorialStep++;
                MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.Selecting, null);
                break;
            case 6:
                if (LineClickCount < 3) return;
                TutorialStep++;
                break;
            case 13:
                MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.None, null);
                _battleStartButton.gameObject.SetActive(true);
                _battleStartButton.onClick.AddListener(BattleStartBtnEvent);
                break;
        }

        if (TutorialStep > 6 && TutorialStep < 13)
        {
            MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.Tutorial, null);
            _timer -= Time.deltaTime;
            if (_timer > 0) return;
            _timer = Timer;
            TutorialStep++;
        }

    }


    #region Publice
    public void ResetTimer()
    {
        _timer = Timer;
    }
    #endregion





    void BattleStartBtnEvent()
    {
        TutorialStep++;
        UpdateTutorialScene(TutorialStep);
        IsTutorialActive = false;
        _isActive = false;
        BuildingSelectionManager.Instance.CloseSelectionPanel();
        MouseStateManager.Instance.SwitchState(MouseStateManager.MouseStates.Selecting, null);

        //* Environment FX
        //EnvironmentFX.Instance.PlayBoomVFX(1);

        //*Play music
        AudioManager.Instance.PlayMusic(AudioManager.MusicType.Combat);


    }

    void UpdateTutorialScene(int sceneIndex)
    {
        for (int i = 0; i < _tutorialScenes.Count; i++)
        {
            if (i != sceneIndex)
            {
                _tutorialScenes[i].SetActive(false);
            }
            else
            {
                _tutorialScenes[i].SetActive(true);
            }
        }
    }


}
