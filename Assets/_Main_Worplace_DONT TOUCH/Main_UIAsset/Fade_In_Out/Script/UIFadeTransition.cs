using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeTransition : MonoBehaviour
{
    public static UIFadeTransition Instance;

    void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;


        FadeOut();
    }

    [field: SerializeField] public Animator FadeAnimator { get; private set; }

    [SerializeField] GameObject _transitionCanvas;
    [SerializeField] RawImage _fadeImage;
    [SerializeField] string _fadeIn;
    [SerializeField] string _fadeOut;
    [SerializeField] string _fadeWait;

    [Header("Loading Image")]
    [SerializeField] GameObject _loadingObjects;
    [SerializeField] Animator _loadingCircle;
    [SerializeField] string _loading;


    public void FadeIn()
    {
        _transitionCanvas.SetActive(true);

        FadeAnimator.Play(_fadeIn);
        StartCoroutine(WaitForFadeIn());
    }
    IEnumerator WaitForFadeIn()
    {
        var timer = new WaitForSeconds(1);
        yield return timer;
        _loadingObjects.SetActive(true);
        _loadingCircle.Play(_loading);
    }


    public void FadeOut()
    {
        _transitionCanvas.SetActive(true);
        _loadingObjects.SetActive(false);

        FadeAnimator.Play(_fadeOut);
        StartCoroutine(WaitForFadeOut());
    }
    IEnumerator WaitForFadeOut()
    {
        var timer = new WaitForSeconds(2);
        yield return timer;
        _transitionCanvas.SetActive(false);
    }


    public void FadeWait()
    {
        _transitionCanvas.SetActive(true);
        FadeAnimator.Play(_fadeWait);
        StartCoroutine(WaitForLoading());

    }
    IEnumerator WaitForLoading()
    {
        var timer = new WaitForSeconds(2);
        yield return timer;
        FadeOut();
    }




}
