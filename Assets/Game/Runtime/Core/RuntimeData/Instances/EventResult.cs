using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EventResult
{
    public SO_Event DungeonEvent;
    public int GoldGained;
    public int ExpGained;
    public int DamageDone;
    public List<Character> TookDamage = new();
    public OutcomeTypes Outcome;


}
