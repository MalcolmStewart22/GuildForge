using System;
using UnityEngine;

[System.Serializable]
public class StatBlock
{
    public int might;
    public int finesse;
    public int endurance;
    public int healing;
    public int arcana;
    public int control;
    public int resolve;

    // ---- Access helpers ----

    public int GetStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.Might: return might;
            case StatType.Finesse: return finesse;
            case StatType.Endurance: return endurance;
            case StatType.Healing: return healing;
            case StatType.Arcana: return arcana;
            case StatType.Control: return control;
            case StatType.Resolve: return resolve;
            default: return 0;
        }
    }

    public void ModifyStat(StatType statType, int amount)
    {
        switch (statType)
        {
            case StatType.Might: might += amount; break;
            case StatType.Finesse: finesse += amount; break;
            case StatType.Endurance: endurance += amount; break;
            case StatType.Healing: healing += amount; break;
            case StatType.Arcana: arcana += amount; break;
            case StatType.Control: control += amount; break;
            case StatType.Resolve: resolve += amount; break;
        }
    }
}
