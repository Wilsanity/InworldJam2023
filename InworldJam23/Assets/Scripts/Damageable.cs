using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the script that will be referenced when an ability or attack deals samage to a target
//You can grab the stats of the attacker and defender to calculate more realistic rpg damage

[RequireComponent(typeof(Health))]
public class Damageable : MonoBehaviour
{

    [SerializeField] private Health health = null;

    public void DealDamage(float damage)
    {
        health.Remove(damage);
    }
}
