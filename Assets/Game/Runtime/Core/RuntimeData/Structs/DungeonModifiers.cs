using UnityEngine;

[System.Serializable]
public class DungeonModifiers
{
    [Header("Stat Requirement Weights")]
    [Range(.8f,1.2f)]
    public float MightWeight = 1f;
    [Range(.8f,1.2f)]
    public float FinesseWeight = 1f;
    [Range(.8f,1.2f)]
    public float ArcanaWeight = 1f;
    [Range(.8f,1.2f)]
    public float ControlWeight = 1f;

    [Header("Event Selection Weights")]
    [Range(.5f,2f)]
    public float CombatWeight = 1f;
    [Range(.5f,2f)]
    public float TrapWeight = 1f;
    [Range(.5f,2f)]
    public float HazardWeight = 1f;
    [Range(.5f,2f)]
    public float TreasureWeight = 1f;

    //Implement with Character Jobs - Will also need to implement damage types when we get there
    //[Header("Damage Multipliers (Multiplicative)")]
    //[Range(.5f,2f)]
    //public float PhysicalMultiplier = 1f;
    //[Range(.5f,2f)]
    //public float MagicMultiplier = 1f;




    public void CombineModifiers (DungeonModifiers y)
    {
        MightWeight = MightWeight * y.MightWeight;
        FinesseWeight = FinesseWeight * y.FinesseWeight;
        ArcanaWeight = ArcanaWeight * y.ArcanaWeight;
        ControlWeight = ControlWeight * y.ControlWeight;

        CombatWeight = CombatWeight * y.CombatWeight;
        TrapWeight = TrapWeight * y.TrapWeight;
        HazardWeight = HazardWeight * y.HazardWeight;
        TreasureWeight = TreasureWeight * y.TreasureWeight;
    }

}