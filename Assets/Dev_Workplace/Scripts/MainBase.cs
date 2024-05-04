using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : MonoBehaviour,IHealthBar
{
    [SerializeField] private float _maxHealth;
    public float health;
    void Start()
    {
        health = _maxHealth;
    }

    public void TakeDamage(float damage) {
        health-=damage;
        if(health <=0) {
            Debug.Log("LOSE");
        }
    }

    public float GetHealth() => health;

    public float GetMaxHealth() => _maxHealth;
}
