using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameConfig", menuName = "Scriptable Objects/SO_GameConfig")]
public class SO_GameConfig : ScriptableObject
{
    [Header("Game")] 
    public int MaxPartySize = 4;
    public int NumberOfDungeons = 10;
    public List<JobStatFocus> JobStatMap = new();

    [Header("Game Length Levers")] 
    public bool MissionCapActive = false;
    public int MissionCap = 10;

    [Header("Game Economy Levers")] 
    public int startingGold = 20;
    public float IncomeModifier = 1f;
    public float ExpenseModifier = 1f;
    public int RestingCost = 1;
    public int BaseMissionGoldPerPartyMember = 2;

    [Header("HP Levers")] 
    public float HPRefusalThreshold = 0.3f;
    public float HPReadyForActionThreshold = 0.7f;
    public float BaseRestHeal = .2f;
    public float retreatHpThreshold = 0;

    [Header("Character Rank Levers")] 
    public CharacterRankLevers RankE;
    public CharacterRankLevers RankD;
    public CharacterRankLevers RankC;
    public CharacterRankLevers RankB;
    public CharacterRankLevers RankA;
    public CharacterRankLevers RankS;

    [Header("Dungeon Levers")] 
    public int DungeonMinimumStatRankE;
    public int DungeonMinimumStatRankD;
    public int DungeonMinimumStatRankC;
    public int DungeonMinimumStatRankB;
    public int DungeonMinimumStatRankA;
    public int DungeonMinimumStatRankS;
    public OutcomeBands OutcomeOptions; 

    [Header("Guild Rank Levers")]
    public GuildRankLevers GuildRankE;
    public GuildRankLevers GuildRankD;
    public GuildRankLevers GuildRankC;
    public GuildRankLevers GuildRankB;
    public GuildRankLevers GuildRankA;
    public GuildRankLevers GuildRankS;
    
    [Header("Other Config Bits")]
    public SO_NameSyllableSet SyllableSet;

}

[System.Serializable]
public class CharacterRankLevers
{
    public int LevelCap; //top of current rank
    public int PromotionCost; //cost to go to next
    public int Wage; // paid to current rank
    public int RequiredLevel; //Should always be less than Level Cap - needed to go to next
    public int PrimaryStatRequirement; //needed to go to next
    public int SecondaryStatRequirement; //needed to go to next
}

[System.Serializable]
public class GuildRankLevers
{
    public int MaxNumberOfParties;
    public int PromotionCost; //cost to go to next
}

[System.Serializable]
public class JobStatFocus
{
    public CharacterJob Job;
    public StatType Primary;
    public StatType Secondary;
}

[System.Serializable]
public class DungeonLevers
{
    public int StatMinimum;
    public int DangerousStatDelta;
    public int RiskyStatDelta;
    public int MostlySafeStatDelta;
    public int PerfectlySafeStatDelta;

}