using UnityEngine;

[System.Serializable]
public class RankEvaluation
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