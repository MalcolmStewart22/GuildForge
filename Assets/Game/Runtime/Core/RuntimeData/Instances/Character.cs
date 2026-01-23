using System.Collections.Generic;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class Character
{
    public int CharacterID;
    public string Name;
    public int Age;
    public CharacterJob Job;
    public CharacterRank Rank;
    public int Level = 1;
    public int EXP = 0;
    public int HPMax;
    public int CurrentHP;
    public StatBlock Base = new();
    public StatBlock Actual = new();
    public List<SO_Trait> Traits = new();
    public TraitsModifiers TotalTraitEffects = new();
    public bool IsAlive = true;
    public bool IsResting = false;
    public int LastDamage;


    public void CalculateActualStats()
    {
        CalculateTotalTraitEffects();

        Actual.might = Base.might + TotalTraitEffects.MightBonus;
        Actual.finesse = Base.finesse + TotalTraitEffects.FinesseBonus;
        Actual.endurance = Base.endurance + TotalTraitEffects.EnduranceBonus;
        Actual.healing = Base.healing + TotalTraitEffects.HealingBonus;
        Actual.arcana = Base.arcana + TotalTraitEffects.AracanaBonus;
        Actual.control = Base.control + TotalTraitEffects.ControlBonus;
        Actual.resolve = Base.resolve + TotalTraitEffects.ResolveBonus;
    }
    
    public void CalculateTotalTraitEffects()
    {
        foreach(var t in Traits)
        {
            TotalTraitEffects = TotalTraitEffects.CombineModifiers(TotalTraitEffects, t.Effects);
        }
    }
    // Probably will be more complicated in the future. 
    public int GetHealingCost()
    {
        int _healCost = (HPMax - CurrentHP) / 2;
        return _healCost;
    }

    public void RankUp()
    {
        switch (Rank)
        {
            case CharacterRank.E:
                Rank = CharacterRank.D;
                return;
            case CharacterRank.D:
                Rank = CharacterRank.C;
                return;
            case CharacterRank.C:
                Rank = CharacterRank.B;
                return;
            case CharacterRank.B:
                Rank = CharacterRank.A;
                return;
            case CharacterRank.A:
                Rank = CharacterRank.S;
                return;
        }
    }

    public void MagicHeal()
    {
        CurrentHP = HPMax;
        IsResting = false;
    }

    public bool GainExp(int exp)
    {
        if(Level < GameStateQueries.GetLevelCap(Rank))
        {
            float _mod = 1f;
            foreach(var t in Traits)
            {
                _mod *= t.Effects.EXPModifier;
            }

            EXP += (int)(exp * _mod);

            if(EXP >= 100)
            {
                LevelUp();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }
    public void LevelUp()
    {
        EXP = EXP - 100;
        Level += 1;

        switch (Job)
        {
            case CharacterJob.Damage:
                Base.might += 3;
                Base.finesse += 1;
                Base.endurance += 2;
                Base.healing += 1;
                Base.arcana += 2;
                Base.control += 1;
                Base.resolve += 1;
                break;
            case CharacterJob.Tank:
                Base.might += 2;
                Base.finesse += 1;
                Base.endurance += 3;
                Base.healing += 1;
                Base.arcana += 1;
                Base.control += 1;
                Base.resolve += 2;
                break;
            case CharacterJob.Support:
                Base.might += 1;
                Base.finesse += 1;
                Base.endurance += 2;
                Base.healing += 3;
                Base.arcana += 2;
                Base.control += 1;
                Base.resolve += 1;
                break;
            case CharacterJob.Control:
                Base.might += 1;
                Base.finesse += 2;
                Base.endurance += 2;
                Base.healing += 1;
                Base.arcana += 1;
                Base.control += 3;
                Base.resolve += 1;
                break;
        }
        CalculateActualStats();

        float hpRatio = 1f * CurrentHP / HPMax;
        HPMax = Mathf.RoundToInt(1.5f * Actual.endurance);
        CurrentHP = Mathf.RoundToInt(HPMax * hpRatio);
    }

    public void TakeDamage(int damage)
    {
        float _mod = 1f;
        foreach(var t in Traits)
        {
            _mod *= t.Effects.DamageModifier;
        }
        LastDamage = Mathf.RoundToInt(damage * (1 - (Actual.endurance / 200f)) * _mod); //endurance caps at 50% damage reduction
        CurrentHP -= LastDamage;
        Debug.Log($"{Name} took {LastDamage} damage!" );
        if (CurrentHP <= 0)
        {
            CharacterDeath();
        }
    }

    private void CharacterDeath()
    {
        Debug.Log(Name + " has died!");
        IsAlive = false;
    }

    public bool RestCheck(float tooLow)
    {
        if (CurrentHP < (HPMax * tooLow))
        {
            IsResting = true;
            return true;
        }
        IsResting = false;
        return false;
    }
    public void Heal(float amount)
    {
        CurrentHP += Mathf.Clamp(Mathf.RoundToInt(HPMax * amount * (Actual.healing / 200f)),0,HPMax);
    }

}
