using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class Party
{
    public int ID;
    public List<Character> PartyMembers = new();
    public List<Character> ActiveMembers = new();
    public bool IsOnMission = false;
    public DungeonInstance CurrentMission;
    public StatBlock PartyStats = new();
    public TraitsModifiers TraitEffects = new();


    private void CalculatePartyStats()
    {
        foreach (Character c in ActiveMembers)
        {
            PartyStats.might += c.Actual.might;
            PartyStats.finesse += c.Actual.finesse;
            PartyStats.endurance += c.Actual.endurance;
            PartyStats.healing += c.Actual.healing;
            PartyStats.arcana += c.Actual.arcana;
            PartyStats.control += c.Actual.control;
            PartyStats.resolve += c.Actual.resolve;
        }

        PartyStats.might = PartyStats.might / ActiveMembers.Count;
        PartyStats.finesse = PartyStats.finesse / ActiveMembers.Count;
        PartyStats.endurance = PartyStats.endurance / ActiveMembers.Count;
        PartyStats.healing = PartyStats.healing / ActiveMembers.Count;
        PartyStats.arcana = PartyStats.arcana / ActiveMembers.Count;
        PartyStats.control = PartyStats.control / ActiveMembers.Count;
        PartyStats.resolve = PartyStats.resolve / ActiveMembers.Count;
        
        CalucaltePartyTraitEffects();
    }
    private void CalucaltePartyTraitEffects()
    {
        foreach(var c in ActiveMembers)
        {
            TraitEffects = TraitEffects.CombineModifiers(TraitEffects, c.TotalTraitEffects);
        }
    }
    public void PrepareParty()
    {
        foreach (Character c in PartyMembers)
        {
            if (c.IsResting || c.IsAlive == false)
            {
                ActiveMembers.Remove(c);
            }
            else if (!ActiveMembers.Contains(c))
            {
                ActiveMembers.Add(c);
            }
        }
        CalculatePartyStats();
    }

    //will finish Caught DamageProfile later, for now party handles traps as a team
    public List<Character> AssignDamage(DamageProfile targets, int damage, List<Character> explicitTargets = null)
    {
        Debug.Log("Handed" + damage + " damage!");
        List<Character> _tookDamage = new();
        damage = Mathf.RoundToInt(damage * (1 - (PartyStats.healing / 200f))); //healing caps at 50% damage reduction
        Debug.Log("Assigning " + damage + " damage!");
        if(targets == DamageProfile.TankFocused)
        {
            bool _hasTank = false;
            foreach (var c in ActiveMembers)
            {
                if (c.Job == CharacterJob.Tank)
                {
                    _hasTank = true;
                    c.TakeDamage(damage);
                    _tookDamage.Add(c);
                }
            }
            if(_hasTank == false)
            {
                targets = DamageProfile.Everyone;
            }
            else
            {
                return _tookDamage;
            }
        }
        else if (targets == DamageProfile.DamageFocused)
        {
            bool _hasDPS = false;
            foreach (var c in ActiveMembers)
            {
                if (c.Job == CharacterJob.Damage)
                {
                    _hasDPS = true;
                    c.TakeDamage(damage);
                    _tookDamage.Add(c);
                }
            }
            if(_hasDPS == false)
            {
                targets = DamageProfile.Everyone;
            }
            else
            {
                return _tookDamage;
            }
        }
        else if (targets == DamageProfile.SupportFocused)
        {
            bool _hasSup = false;
            foreach (var c in ActiveMembers)
            {
                if (c.Job == CharacterJob.Support)
                {
                    _hasSup = true;
                    c.TakeDamage(damage);
                    _tookDamage.Add(c);
                }
            }
            if(_hasSup == false)
            {
                targets = DamageProfile.Everyone;
            }
            else
            {
                return _tookDamage;
            }
        }
        else if (targets == DamageProfile.Caught)
        {
            if(explicitTargets != null)
            {
                foreach (var c in explicitTargets)
                {
                    c.TakeDamage(damage);
                    _tookDamage.Add(c);
                }
                return _tookDamage;
            }
            else
            {
                Debug.LogWarning("Attempted to damage Caught Party Memebers but list was empty!");
            }

        }
        if (targets == DamageProfile.Everyone)
        {
            foreach (var c in ActiveMembers)
            {
                    c.TakeDamage(damage);
                    _tookDamage.Add(c);
            }
            return _tookDamage;
        }
        return null;
    }

    public List<LevelUpReport> AssignExp(int exp)
    {
        List<LevelUpReport> _report = new();
        foreach(var c in ActiveMembers)
        {
            LevelUpReport _r = new LevelUpReport
            {
                Character = c,
                LeveledUp = c.GainExp(exp)
            };
            _report.Add(_r);
        }
        return _report;
    }

    public void StatusCheck()
    {
        foreach (var c in ActiveMembers)
        {
            if(!c.IsAlive)
            {
                ActiveMembers.Remove(c);
                PartyMembers.Remove(c);
            }

        }
    }

    public string GetSafetyRating(DungeonInstance dungeon)
    {
        int mightDifference = (PartyStats.might - dungeon.CalculateRequiredStat(EventType.Combat)) + TraitEffects.CombatBonus;
        int controlDifference = (PartyStats.might - dungeon.CalculateRequiredStat(EventType.Trap)) + TraitEffects.CombatBonus;
        int finesseDifference = (PartyStats.might - dungeon.CalculateRequiredStat(EventType.Hazard)) + TraitEffects.CombatBonus;
        int arcanaDifference = (PartyStats.might - dungeon.CalculateRequiredStat(EventType.Treasure)) + TraitEffects.CombatBonus;

        int averageDifference = (mightDifference + controlDifference + finesseDifference + arcanaDifference) / 4;

        if (averageDifference <= 0)
        {
            return "Dangerous!";
        }
        else if (averageDifference <= 10)
        {
            return "Risky";
        }
        else if (averageDifference <= 20) 
        {
            return "Safe";
        }
        else
        {
            return "EASY!";
        }
    }
}