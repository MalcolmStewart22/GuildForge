using UnityEngine;

[System.Serializable]
public class TraitsModifiers
{
    [Header("Stat Adjustments")]
    [Tooltip("Numbers can be Positive or Negative, bonus is being used as a neutral word.")]
    public int MightBonus = 0;
    public int FinesseBonus = 0;
    public int EnduranceBonus = 0;
    public int HealingBonus = 0;
    public int AracanaBonus = 0;
    public int ControlBonus = 0;
    public int ResolveBonus = 0;


    [Header("Dungeon Adjustments")]
    [Tooltip("Numbers can be Positive or Negative, bonus is being used as a neutral word.")]
    public int CombatBonus = 0;
    public int TrapBonus = 0;
    public int HazardBonus = 0;
    public int TreasureBonus = 0;

    [Header("Result Modifiers")]
    [Range(.5f,2f)]
    public float GoldModifier = 1f;
    [Range(.5f,2f)]
    public float DamageModifier = 1f;
    [Range(.5f,2f)]
    public float EXPModifier = 1f;


    public TraitsModifiers CombineModifiers (TraitsModifiers x, TraitsModifiers y)
    {
        TraitsModifiers _newModifier = new();
        _newModifier.MightBonus = x.MightBonus + y.MightBonus;
        _newModifier.FinesseBonus = x.FinesseBonus + y.FinesseBonus;
        _newModifier.EnduranceBonus = x.EnduranceBonus + y.EnduranceBonus;
        _newModifier.HealingBonus = x.HealingBonus + y.HealingBonus;
        _newModifier.AracanaBonus = x.AracanaBonus + y.AracanaBonus;
        _newModifier.ControlBonus = x.ControlBonus + y.ControlBonus;
        _newModifier.ResolveBonus = x.ResolveBonus + y.ResolveBonus;

        _newModifier.CombatBonus = x.CombatBonus * y.CombatBonus;
        _newModifier.TrapBonus = x.TrapBonus * y.TrapBonus;
        _newModifier.HazardBonus = x.HazardBonus * y.HazardBonus;
        _newModifier.TreasureBonus = x.TreasureBonus * y.TreasureBonus;

        _newModifier.GoldModifier = x.GoldModifier * y.GoldModifier;
        _newModifier.DamageModifier = x.DamageModifier * y.DamageModifier;
        _newModifier.EXPModifier = x.EXPModifier * y.EXPModifier;
        
        return _newModifier;
    }
}