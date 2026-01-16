using UnityEngine;

[System.Serializable]
public class RankEvaluation
{
    public bool CanPromote;
    public int PromotionCost;
    public int CurrentWage;
    public int NextWage;
    public CharacterRank CurrentRank;
    public CharacterRank NextRank;
    public int MainStatRequirement;
    public int SecondaryStatRequirement;
    public int RequiredLevel;

}