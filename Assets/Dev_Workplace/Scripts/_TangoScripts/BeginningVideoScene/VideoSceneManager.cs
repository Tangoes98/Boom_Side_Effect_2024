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
    event Action<VideoPlayer> NextSceneEvent;
    float _timer;
    bool _isActive = true;

    [Space(15)]
    [SerializeField] VideoPlayer _winningVideo;
    [SerializeField] VideoPlayer _losingVideo;

    [Space(15)]
    [SerializeField] GameObject _introVideoObj;
    [SerializeField] VideoPlayer _introVideo;

    private void Awake()
    {
        _skipTextCN.gameObject.SetActive(false);
        _skipTextEN.gameObject.SetActive(false);
    }


    private void Start()
    {
        NextSceneEvent += NextSceneEventAction;
        _videoClip.loopPointReached += NextSceneEventAction;


        _introVideo.loopPointReached += IntroVideoFinishEventAction;


        _winningVideo.loopPointReached += NextSceneEventAction;
        _losingVideo.loopPointReached += NextSceneEventAction;


        _skippingUI.fillAmount = 0f;
        _timer = _skipTimer;

        if (SceneManager.GetActiveScene().buildIndex == 4) return;

        StartCoroutine(WaitForHint());
    }


    IEnumerator WaitForHint()
    {
        var timer = new WaitForSeconds(4);
        yield return timer;
        if (SceneDataManager.Instance.CurrentLanguage == "CN") _skipTextCN.gameObject.SetActive(true);
        else _skipTextEN.gameObject.SetActive(true);
    }

    void IntroVideoFinishEventAction(VideoPlayer source)
    {
        _introVideoObj.SetActive(false);
        _videoClip.Play();
    }





    private void Update()
    {
        if (!_isActive) return;

        if (!_isVideoPlayed)
        {
            if (!UIFadeTransition.Instance._IsTransitionOver) return;

            if (SceneManager.GetActiveScene().buildIndex == 4) // at final scnene
            {
                if (SceneDataManager.Instance.IsWinning)
                {
                    _winningVideo.Play();
                }
                else
                {
                    _losingVideo.Play();
                }
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2) // at beginning scene
            {
                _introVideo.Play();
                _videoClip.Prepare();
            }
            else _videoClip.Play();

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
            NextSceneEvent?.Invoke(_videoClip);
        }

    }

    void NextSceneEventAction(VideoPlayer vp)
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
            case 4: //! At Final video
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
