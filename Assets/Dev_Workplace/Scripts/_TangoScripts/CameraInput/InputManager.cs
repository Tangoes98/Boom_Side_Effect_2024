using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    [Header("REFERENCE")]
    [SerializeField] GameObject _camObject;
    [SerializeField] float _camSpeed;


    [SerializeField] bool _isPauseGame = false;

    public bool _IsSpeedTakingControl = false;

    private void Start()
    {
    }
    private void Update()
    {
        if (TutorialUI.Instance != null && TutorialUI.Instance.IsTutorialActive) return;

        _camObject.transform.Translate(InputAxis());

        if (_IsSpeedTakingControl) return;
        PauseGameCheck();


    }


    #region Public Methods

    public static bool IS_SPACE_DOWN()
    {
        if (Input.GetKeyDown(KeyCode.Space)) return true;
        else return false;
    }

    public static bool IS_ESC_DOWN()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) return true;
        else return false;
    }
    public void PauseGame(bool bvalue) => _isPauseGame = bvalue;

    #endregion
    #region ==================
    void PauseGameCheck()
    {
        if (!_isPauseGame) Time.timeScale = 1;
        if (_isPauseGame) Time.timeScale = .1f; //? Slow Motion
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
