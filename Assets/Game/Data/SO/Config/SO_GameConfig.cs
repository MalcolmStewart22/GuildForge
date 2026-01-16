using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameConfig", menuName = "Scriptable Objects/SO_GameConfig")]
public class SO_GameConfig : ScriptableObject
{
    [Header("Game")] 
    public int NumberOfCharacters = 4;
    public int NumberOfDungeons = 10;
    public List<JobStatFocus> JobStatMap = new();

    [Header("Game Length Levers")] 
    public bool MissionCapActive = false;
    public int MissionCap = 10;

    [Header("Game Economy Levers")] 
    public int startingGold = 20;
    public int wagePerCharacter = 2;

    [Header("HP Levers")] 
    public float HPRefusalThreshold = 0.3f;
    public float HPReadyForActionThreshold = 0.7f;
    public float BaseRestHeal = .2f;
    public float retreatHpThreshold = 0;

    [Header("Character Rank Levers")] 
    public RankBalanceLevers RankE;
    public RankBalanceLevers RankD;
    public RankBalanceLevers RankC;
    public RankBalanceLevers RankB;
    public RankBalanceLevers RankA;
    public RankBalanceLevers RankS;

    [Header("Other Config Bits")]
    public SO_NameSyllableSet SyllableSet;
    public OutcomeBands OutcomeOptions; 
}

[System.Serializable]
public class RankBalanceLevers
{
    public int LevelCap; //top of current rank
    public int PromotionCost; //cost to go to next
    public int Wage; // paid to current rank
    public int RequiredLevel; //Should always be less than Level Cap - needed to go to next
    public int MainStatRequirement; //needed to go to next
    public int SecondaryStatRequirement; //needed to go to next
}

[System.Serializable]
public class JobStatFocus
{
    public CharacterJob Job;
    public StatType Primary;
    public StatType Secondary;
}