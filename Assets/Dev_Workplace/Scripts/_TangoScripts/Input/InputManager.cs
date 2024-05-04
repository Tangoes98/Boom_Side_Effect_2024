using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class InputManager : MonoBehaviour
{


    [Header("REFERENCE")]
    [SerializeField] GameObject _camObject;
    [SerializeField] float _camSpeed;

    [Header("DEBUG")]
    public TextMeshProUGUI Timer;
    float timer = 0f;
    [SerializeField] bool _isPauseGame = false;

    private void Start()
    {
    }
    private void Update()
    {
        if (TutorialUI.Instance!=null && TutorialUI.Instance.IsTutorialActive) return;

        if (IS_SPACE_DOWN()) _isPauseGame = !_isPauseGame;


        PauseGameCheck();
        timer += Time.deltaTime;
        Timer.text = timer.ToString();

        _camObject.transform.Translate(InputAxis());
    }


    #region Public Methods

    public static bool IS_SPACE_DOWN()
    {
        if (Input.GetKeyDown(KeyCode.Space)) return true;
        else return false;
    }
    #endregion
    #region ==================
    void PauseGameCheck()
    {
        if (!_isPauseGame) Time.timeScale = 1;
        if (_isPauseGame) Time.timeScale = 0f; //? Slow Motion
    }

    Vector3 InputAxis()
    {
        //return new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime, 0f, Input.GetAxis("Vertical") * Time.deltaTime);
        return new Vector3(Input.GetAxis("Horizontal") * _camSpeed * Time.deltaTime, 0f, Input.GetAxis("Vertical") * _camSpeed * Time.deltaTime);
    }





    #endregion



    // public static bool Is_RMB_Down()
    // {
    //     if (Input.GetMouseButtonDown(1)) return true;
    //     else return false;
    // }
}
