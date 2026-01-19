using UnityEngine;

[System.Serializable]
public class CharacterRankEvaluation
{
    //Bools
    public bool CanPromote;
    public bool MeetsRequiredLevel;
    public bool MeetsRequiredPrimaryStat;
    public bool MeetsRequiredSecondaryStat;
    public bool HasEnoughGold;

    //Requirements
    public int RequiredLevel;
    public StatType PrimaryStat;
    public int PrimaryStatRequirement;
    public StatType SecondaryStat;
    public int SecondaryStatRequirement;
    public int PromotionCost;

    //Effects
    public int CurrentWage;
    public int NextWage;
    public CharacterRank NextRank;
}

[System.Serializable]
public class GuildRankEvaluation
{
    //bools
    public bool CanPromote;
    public bool HasEnoughGold;
    public bool MeetsCharacterRequirement;

    //requirements
    public int NumCharactersAtRank;
    public int NumCharactersRequiredAtRank;
    public int PromotionCost;

    //effects
    public int NewPartyMax;
    public GuildRank NewRank;
}