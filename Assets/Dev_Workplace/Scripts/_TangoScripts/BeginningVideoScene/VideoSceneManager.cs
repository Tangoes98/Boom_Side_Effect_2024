using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class VideoSceneManager : MonoBehaviour
{




    [SerializeField] VideoPlayer _videoClip;
    bool _isVideoPlayed = false;

    [Space(15)]
    [SerializeField] Image _skippingUI;
    [SerializeField] float _skipTimer;

    [Space(15)]
    [SerializeField] TextMeshProUGUI _skipTextEN;
    [SerializeField] TextMeshProUGUI _skipTextCN;
    event Action NextSceneEvent;
    float _timer;
    bool _isActive = true;

    private void Start()
    {
        _skipTextCN.gameObject.SetActive(false);
        _skipTextEN.gameObject.SetActive(false);

        _skippingUI.fillAmount = 0f;
        _timer = _skipTimer;

        if (SceneDataManager.Instance.CurrentLanguage == "CN") _skipTextCN.gameObject.SetActive(true);
        else _skipTextEN.gameObject.SetActive(true);

        NextSceneEvent += NextSceneEventAction;
    }

    private void Update()
    {
        if (!_isActive) return;

        if (!_isVideoPlayed)
        {
            if (!UIFadeTransition.Instance._IsTransitionOver) return;
            _videoClip.Play();
            _isVideoPlayed = true;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            _timer -= Time.deltaTime;
            _skippingUI.fillAmount += Time.deltaTime / _skipTimer;
        }
        else
        {
            _skippingUI.fillAmount = 0f;
            _timer = _skipTimer;
        }

        if (_skippingUI.fillAmount == 1)
        {
            NextSceneEvent?.Invoke();
        }

    }

    void NextSceneEventAction()
    {
        UIFadeTransition.Instance.FadeIn();
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2: //! At beginning video
                StartCoroutine(WaitForNextScene(1)); //! Go to battle
                break;
            case 3: //! At Credit video
                StartCoroutine(WaitForNextScene(0)); //! Go to title
                break;
        }
    }
    IEnumerator WaitForNextScene(int sceneIndex)
    {
        var timer = new WaitForSeconds(2);
        yield return timer;
        SceneManager.LoadScene(sceneIndex);
    }





}
