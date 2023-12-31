using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//DO NOT REFERENCE THIS SCRIPT WHEN HANDLING HEALTH CHANGE, THIS SHOULD BE A PURELY READ-ONLY CLASS
//IF YOU MUST CHANGE HEALTH VALUES USE THE RESPECTABLE DAMAGE AND HEAL CLASSES
//OR USE THE EVENTS WHEN DEALING WITH UI OR ON DEATH ACTIONS

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    public bool IsDead => health == 0f;


    public void Start()
    {
        health = maxHealth;
    }

    private void OnDestroy()
    {
        OnDeath?.Invoke();
    }

    public void Add(float value)
    {
        value = Mathf.Max(value, 0);

        health = Mathf.Min(health + value, maxHealth);
        OnHealthChanged?.Invoke(health);
    }

    public void Remove(float value)
    {
        value = Mathf.Max(value, 0);

        health = Mathf.Max(health - value, 0);

        OnHealthChanged?.Invoke(health);

        if (health <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    } 

}
