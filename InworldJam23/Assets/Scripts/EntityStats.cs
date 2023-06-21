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

    public float GetStat(Stats statType)
    {
        foreach(var stat in stats)
        {
            if (stat.Key.Equals(statType))
                return stat.Value;
        }

        return 0;
    }
}
