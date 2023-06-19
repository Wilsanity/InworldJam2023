using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats
{ 
    Health,
    Strength
}

public class EntityStats: MonoBehaviour
{
    [SerializedDictionary("Stat", "Amount")]
    public SerializedDictionary<Stats, int> stats;
}
