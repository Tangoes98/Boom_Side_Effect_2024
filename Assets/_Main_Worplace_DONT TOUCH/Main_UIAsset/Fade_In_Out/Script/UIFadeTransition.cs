using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeTransition : MonoBehaviour
{
    public static UIFadeTransition Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        //_fadeImage.gameObject.SetActive(true);
        FadeOut();
    }


    [field: SerializeField] public Animator FadeAnimator { get; private set; }

    [SerializeField] GameObject _transitionCanvas;
    [SerializeField] RawImage _fadeImage;
    [SerializeField] string _fadeIn;
    [SerializeField] string _fadeOut;

    public void FadeIn()
    {
        _transitionCanvas.SetActive(true);
        FadeAnimator.Play(_fadeIn);
        StartCoroutine(WaitForSecond());
    }
    public void FadeOut()
    {
        _transitionCanvas.SetActive(true);
        FadeAnimator.Play(_fadeOut);
        StartCoroutine(WaitForSecond());
    }


    private void Start()
    {
    }

    IEnumerator WaitForSecond()
    {
        var timer = new WaitForSeconds(1);
        yield return timer;
        _transitionCanvas.SetActive(false);
    }




}
