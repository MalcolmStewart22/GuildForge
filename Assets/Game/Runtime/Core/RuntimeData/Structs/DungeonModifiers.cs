using UnityEngine;

[System.Serializable]
public class DungeonModifiers
{
    [Header("Stat Requirement Weights")]
    [Range(.5f,2f)]
    public float MightWeight = 1f;
    [Range(.5f,2f)]
    public float FinesseWeight = 1f;
    [Range(.5f,2f)]
    public float EnduranceWeight = 1f;
    [Range(.5f,2f)]
    public float HealingWeight = 1f;
    [Range(.5f,2f)]
    public float ArcanaWeight = 1f;
    [Range(.5f,2f)]
    public float ControlWeight = 1f;
    [Range(.5f,2f)]
    public float ResolveWeight = 1f;

    [Header("Event Selection Weights")]
    [Range(.5f,2f)]
    public float CombatWeight = 1f;
    [Range(.5f,2f)]
    public float TrapWeight = 1f;
    [Range(.5f,2f)]
    public float HazardWeight = 1f;
    [Range(.5f,2f)]
    public float TreasureWeight = 1f;

    //Implement with Character Jobs
    [Header("Damage Multipliers (Multiplicative)")]
    [Range(.5f,2f)]
    public float PhysicalMultiplier = 1f;
    [Range(.5f,2f)]
    public float MagicMultiplier = 1f;




    public DungeonModifiers CombineModifiers (DungeonModifiers x, DungeonModifiers y)
    {
        DungeonModifiers _newModifier = new();
        _newModifier.MightWeight = x.MightWeight * y.MightWeight;
        _newModifier.FinesseWeight = x.FinesseWeight * y.FinesseWeight;
        _newModifier.EnduranceWeight = x.EnduranceWeight * y.EnduranceWeight;
        _newModifier.HealingWeight = x.HealingWeight * y.HealingWeight;
        _newModifier.ArcanaWeight = x.ArcanaWeight * y.ArcanaWeight;
        _newModifier.ControlWeight = x.ControlWeight * y.ControlWeight;
        _newModifier.ResolveWeight = x.ResolveWeight * y.ResolveWeight;

        _newModifier.CombatWeight = x.CombatWeight * y.CombatWeight;
        _newModifier.TrapWeight = x.TrapWeight * y.TrapWeight;
        _newModifier.HazardWeight = x.HazardWeight * y.HazardWeight;
        _newModifier.TreasureWeight = x.TreasureWeight * y.TreasureWeight;

        _newModifier.PhysicalMultiplier = x.PhysicalMultiplier * y.PhysicalMultiplier;
        _newModifier.MagicMultiplier = x.MagicMultiplier * y.MagicMultiplier;
        
        return _newModifier;
    }
}