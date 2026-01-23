using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionResult
{
    public List<EventResult> EventResults = new();
    public DungeonInstance Dungeon;
    public int GoldGained;
    public int MissionWeek;
    public OutcomeTypes Outcome;
    public List<LevelUpReport> LevelUpReports = new();
}

[System.Serializable]
public class LevelUpReport
{
    public int CharacterID;
    public string CharacterName;
    public CharacterRank Rank;
    public int CurrentHP;
    public int MaxHP;
    public bool IsResting;
    public int EXP;
    public int Level;
    public bool LeveledUp = false;

    public LevelUpReport(Character c, int exp)
    {
        CharacterID = c.CharacterID;
        CharacterName = c.Name;
        Rank = c.Rank;
        CurrentHP = c.CurrentHP;
        MaxHP = c.HPMax;
        IsResting = c.IsResting;
        LeveledUp = c.GainExp(exp);
        EXP = c.EXP;
        Level = c.Level;
    }
}