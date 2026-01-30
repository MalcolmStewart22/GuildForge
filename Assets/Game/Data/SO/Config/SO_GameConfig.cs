using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameConfig", menuName = "Scriptable Objects/SO_GameConfig")]
public class SO_GameConfig : ScriptableObject
{
    [Header("Game")] 
    public int MaxPartySize = 4;
    public int NumberOfDungeons = 10;
    public List<JobStat> JobStatMap = new();
    public HPLevers HPConfig;
    public LevelUpIncrease BaseLevelIncrease = new();


    [Header("Game Length Levers")] 
    public bool MissionCapActive = false;
    public int MissionCap = 10;

    [Header("Game Economy Levers")] 
    public int startingGold = 20;
    public float IncomeModifier = 1f;
    public float ExpenseModifier = 1f;
    [Range(0,1f)]
    public float ExpectedMissionRatio = .75f; // What percentage of days sending out a party

    [Header("Dungeon Levers")] 
    public int DungeonRankEMinimum;
    public int DungeonRankDMinimum;
    public int DungeonRankCMinimum;
    public int DungeonRankBMinimum;
    public int DungeonRankAMinimum;
    public int DungeonRankSMinimum;
    public OutcomeBands OutcomeOptions; 
    public DungeonLevers BaseDeltas;
    public List<PartyProfile> PartyProfiles = new();
    public PartyPressureThreshold PressureThresholds;

    [Header("Rank Levers")]
    public GuildRankLevers GuildRankE;
    public GuildRankLevers GuildRankD;
    public GuildRankLevers GuildRankC;
    public GuildRankLevers GuildRankB;
    public GuildRankLevers GuildRankA;
    public GuildRankLevers GuildRankS;

    public CharacterRankLevers CharacterRankE;
    public CharacterRankLevers CharacterRankD;
    public CharacterRankLevers CharacterRankC;
    public CharacterRankLevers CharacterRankB;
    public CharacterRankLevers CharacterRankA;
    public CharacterRankLevers CharacterRankS;

    [Header("Other Config Bits")]
    public SO_NameSyllableSet SyllableSet;
    public SO_PartyNameSet PartyNameSet;

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
    public int CharactersRequiredToRankUp; // needed to go to next
    public int MaxNumberOfParties; // current rank cap
    public int PromotionCost; //cost to go to next
}

[System.Serializable]
public class JobStat
{
    public CharacterJob Job;
    public StatType Primary;
    public StatType Secondary;
    public StatType CombatStat;
    public StatType Survival;

}

[System.Serializable]
public class DungeonLevers
{
    public int StatMinimum;
    public int DangerousStatDelta; // less than or equal to
    public int RiskyStatDelta; // less than or equal to
    public int SafeStatDelta; // Mostly Safe is less than or equal to -> Perfectly is Greater than

    public DungeonLevers(int min, DungeonLevers baseDelta)
    {
        StatMinimum = min;
        DangerousStatDelta = baseDelta.DangerousStatDelta; 
        RiskyStatDelta = baseDelta.RiskyStatDelta; 
        SafeStatDelta = baseDelta.SafeStatDelta; 
    }

}

[System.Serializable]
public class HPLevers
{
    public float HPRefusalThreshold = 0.3f;
    public float HPReadyForActionThreshold = 0.7f;
    public float BaseRestHeal = .3f;
    public float RetreatHPFloor = .3f;
    public float SpikeHPFloor = .5f;
}
[System.Serializable]
public class LevelUpIncrease
{
    public int PrimaryIncrease = 3;
    public int SecondaryIncrease = 2;
    public int TrainingIncrease = 1;
}