using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionResult
{
    public List<EventResult> EventResults = new();
    public Party Party;
    public DungeonInstance Dungeon;
    public int GoldGained;
    public int MissionDay;
    public OutcomeTypes Outcome;
    public List<LevelUpReport> LevelUpReports = new();
}

[System.Serializable]
public class LevelUpReport
{
    public Character Character;
    public bool LeveledUp = false;
}
