using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;


[System.Serializable]
public enum PartyAction
{
    Dungeon,
    Rest,
    Unassigned,
}
[System.Serializable]
public class Party
{
    //Party Info
    public int ID;
    public string PartyName;
    public List<Character> PartyMembers = new();
    public List<Character> LivingMembers = new();
    public StatBlock PartyStats = new();
    public TraitsModifiers TraitEffects = new();
    // Party Decisions
    public DungeonInstance CurrentMission;
    public PartyAction AssignedAction = PartyAction.Unassigned;
    public PartyProfileType Profile;
    public PartyRunPressure CurrentPressure;
    public int CurrentLoot = 0;


    public int GoHome()
    {
        CurrentMission = null;
        AssignedAction = PartyAction.Unassigned;

        int _result = CurrentLoot;
        CurrentLoot = 0;
        return _result;
    }
    public void UpdateParty()
    {
        CalculatePartyStats();
        CalculatePartyTraitEffects();
    }
    public void PrepareParty(DungeonInstance dungeon)
    {
        AssignedAction = PartyAction.Dungeon;
        CurrentMission = dungeon;
    }
    public void HealthCheck(HPLevers config )
    {
        if(AssignedAction == PartyAction.Rest)
        {
            AssignedAction = PartyAction.Unassigned;
        }
        else
        {
            List<Character> _temp = new List<Character>(PartyMembers);
            foreach (var character in _temp)
            {
                if(character.IsAlive)
                {
                    if(character.RestCheck(config.HPRefusalThreshold))
                    {
                        AssignedAction = PartyAction.Rest;
                    }
                }
                else
                {
                    PartyMembers.Remove(character);
                }
                
            }
        }
    }
    public int GetWages()
    {
        int _wages = 0;
        foreach (var character in PartyMembers)
        {
            _wages += GameStateQueries.GetWage(character.Rank);
        }
        return _wages;
    }

    private void CalculatePartyStats()
    {
        foreach (Character c in LivingMembers)
        {
            PartyStats.might += c.Actual.might;
            PartyStats.finesse += c.Actual.finesse;
            PartyStats.endurance += c.Actual.endurance;
            PartyStats.healing += c.Actual.healing;
            PartyStats.arcana += c.Actual.arcana;
            PartyStats.control += c.Actual.control;
            PartyStats.resolve += c.Actual.resolve;
        }

        PartyStats.might = PartyStats.might / LivingMembers.Count;
        PartyStats.finesse = PartyStats.finesse / LivingMembers.Count;
        PartyStats.endurance = PartyStats.endurance / LivingMembers.Count;
        PartyStats.healing = PartyStats.healing / LivingMembers.Count;
        PartyStats.arcana = PartyStats.arcana / LivingMembers.Count;
        PartyStats.control = PartyStats.control / LivingMembers.Count;
        PartyStats.resolve = PartyStats.resolve / LivingMembers.Count;
    }
    private void CalculatePartyTraitEffects()
    {
        foreach(var c in LivingMembers)
        {
            TraitEffects = TraitEffects.CombineModifiers(TraitEffects, c.TotalTraitEffects);
        }
    }
    
    //will finish Caught DamageProfile later, for now party handles traps as a team
    public List<Character> AssignDamage(DamageProfile targets, int damage, List<Character> explicitTargets = null)
    {
        List<Character> _tookDamage = new();
        damage = Mathf.RoundToInt(damage * (1 - (PartyStats.healing / 200f))); //healing caps at 50% damage reduction
        Debug.Log("Assigning " + damage + " damage!");
        if(targets == DamageProfile.TankFocused)
        {
            bool _hasTank = false;
            foreach (var c in PartyMembers)
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
            foreach (var c in PartyMembers)
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
            foreach (var c in PartyMembers)
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
            foreach (var c in PartyMembers)
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
        foreach(var c in PartyMembers)
        {
            _report.Add(new LevelUpReport(c, exp));
        }
        UpdateParty();
        return _report;
    }

    public bool StatusCheck() //return true if its time to flee
    {
        CurrentPressure = GameStateQueries.CalculateCurrentPressure(this);
        HeartbeatCheck();
        UpdateParty();
        return GameStateQueries.CalculateRetreatOrders(CurrentPressure);
    }
    private void HeartbeatCheck()
    {
        LivingMembers.Clear();
        foreach(Character c in PartyMembers)
        {
            if(c.IsAlive)
            {
                LivingMembers.Add(c);
            }
        }
    }

    public string GetSafetyRating(DungeonInstance dungeon)
    {
        DungeonLevers _levers = GameStateQueries.GetDungeonLevers(dungeon.Rank);
        int mightDifference = (PartyStats.might - dungeon.CalculateRequiredStat(EventType.Combat, _levers.StatMinimum)) + TraitEffects.CombatBonus;
        int controlDifference = (PartyStats.control - dungeon.CalculateRequiredStat(EventType.Trap, _levers.StatMinimum)) + TraitEffects.CombatBonus;
        int finesseDifference = (PartyStats.finesse - dungeon.CalculateRequiredStat(EventType.Hazard, _levers.StatMinimum)) + TraitEffects.CombatBonus;
        int arcanaDifference = (PartyStats.arcana - dungeon.CalculateRequiredStat(EventType.Treasure, _levers.StatMinimum)) + TraitEffects.CombatBonus;

        int averageDifference = (mightDifference + controlDifference + finesseDifference + arcanaDifference) / 4;

        if (averageDifference <= _levers.DangerousStatDelta)
        {
            return "Dangerous!";
        }
        else if (averageDifference <= _levers.RiskyStatDelta)
        {
            return "Risky";
        }
        else if (averageDifference <= _levers.SafeStatDelta) 
        {
            return "Mostly Safe";
        }
        else
        {
            return "Perfectly Safe";
        }
    }
}