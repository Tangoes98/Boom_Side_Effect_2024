using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private GameObject _line1,_line2,_line3;
    void Awake()
    {
        _line2.SetActive(true);
        _line1.SetActive(false);
        _line3.SetActive(false);
    }

    public void SetLevel(int lvl) {
        if(lvl==1) {
            _line2.SetActive(true);
            _line1.SetActive(false);
            _line3.SetActive(false);
        }
        else if(lvl==2) {
            _line1.SetActive(true);
            _line2.SetActive(true);
            _line3.SetActive(false);
        } else {
            _line1.SetActive(true);
            _line2.SetActive(true);
            _line3.SetActive(true);
        }
    }
}
