using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healable : MonoBehaviour
{
    [SerializeField] private Health health = null;

    public void Heal(float value)
    {
        health.Add(value);
    }
}