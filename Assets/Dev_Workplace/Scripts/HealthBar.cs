using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private IHealthBar _parent;
    [SerializeField] private Image _bar,_barBackground;
    [SerializeField] private float _length=20;
    [SerializeField] private float _height = 2;
    void Start()
    {
        _parent = GetComponentInParent<IHealthBar>();
        _bar.rectTransform.sizeDelta = new Vector2(_length, _height);
        _barBackground.rectTransform.sizeDelta = new Vector2(_length + 0.4f, _height + 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        _bar.fillAmount = math.clamp(_parent.GetHealth() / _parent.GetMaxHealth(),0,1);
        transform.rotation = Camera.main.transform.rotation;
        //transform.rotation = Quaternion.LookRotation(transform.position - Camera.current.transform.position);
    }
}
