using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }

    public event Action DropResourcesEvent;


    [SerializeField] private GameObject _textBesideCursorEN,_textBesideCursorCN;
    private float _timer = 1f;
    private bool _isShowTextByCursor = false;

    [field: SerializeField] public int PlayerResource { get; private set; }

    //? Interact when building actions
    //? Interact when minion dies

    public bool CanBuild(int buildingCost)
    {
        if (buildingCost > PlayerResource)
        {
            
            TurnOnTextBesideCursor();
            
            Debug.Log("Dont have enough resource to build");
            return false;
        }

        PlayerResource -= buildingCost;
        if (PlayerResource < 0) PlayerResource = 0;
        return true;
    }
    public bool CanUpgrade(int upgradeCost)
    {
        if (upgradeCost > PlayerResource)
        {
            TurnOnTextBesideCursor();
            Debug.Log("Dont have enough resource upgrade");
            return false;
        }

        PlayerResource -= upgradeCost;
        if (PlayerResource < 0) PlayerResource = 0;
        return true;
    }

    public void DropResource(int resourceAmount)
    {
        DropResourcesEvent?.Invoke();
        StartCoroutine(DropResourceDelay(5f, resourceAmount));
    }
    IEnumerator DropResourceDelay(float waitTime, int resourceAmount)
    {
        var wait = new WaitForSeconds(waitTime);
        yield return wait;
        PlayerResource += resourceAmount;
    }

    public void GainResorce(int amount)
    {
        PlayerResource += amount;
    }

    private void Update() {

        if(_isShowTextByCursor) {
            _timer-=Time.deltaTime;

            if(_timer <0) {
                _isShowTextByCursor = false;
                _textBesideCursorEN.SetActive(false);
                _textBesideCursorCN.SetActive(false);
            }
        }
    }

    private void TurnOnTextBesideCursor() {
        _isShowTextByCursor = true;
        _timer = 1;
        GameObject textBesideCursor = (SceneDataManager.Instance==null || SceneDataManager.Instance.CurrentLanguage == "CN")?
                                            _textBesideCursorCN : _textBesideCursorEN;
        textBesideCursor.transform.position = Input.mousePosition ;
        textBesideCursor.SetActive(true);
    }
}
