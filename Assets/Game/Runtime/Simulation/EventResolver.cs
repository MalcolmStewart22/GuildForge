using System.Collections.Generic;
using UnityEngine;

public class EventResolver
{
    //given an event
    //roll based on stats and event type
    //assign success band
    //give rewards and damage
    //return result

    private EventResult eventResult;
    
    private Outcomes eventOutcome;


    public EventResult ResolveEvent(SO_Event e, Party party, DungeonInstance dungeon, OutcomeBands outcomeOptions)
    {
        eventResult = new();
        eventResult.DungeonEvent = e;
    
        switch(e.EventType)
        {
            case EventType.Combat:
                CombatResolver(e, party, dungeon, outcomeOptions);
                break;
            case EventType.Trap:
                TrapResolver(e, party, dungeon, outcomeOptions);
                break;
            case EventType.Hazard:
                HazardResolver(e, party, dungeon, outcomeOptions);
                break;
            case EventType.Treasure:
                TreasureResolver(e, party, dungeon, outcomeOptions);
                break;
        }

        eventResult.DamageDone = Mathf.RoundToInt(e.PotentialDamage * eventOutcome.DamageModifier);
        eventResult.TookDamage = new List<Character>(party.AssignDamage(e.damageProfile, eventResult.DamageDone));
        eventResult.ExpGained = Mathf.RoundToInt(e.PotentialExp * eventOutcome.EXPModifier);
        eventResult.GoldGained = Mathf.RoundToInt(e.PotentialGold * eventOutcome.GoldGainedModifier);
        party.CurrentLoot += eventResult.GoldGained;
        Debug.Log(e.Name + " Completed");
        return eventResult;
    }

    //tags decide required stats
    //straight roll outcome modifiers damage scale
    private void CombatResolver(SO_Event e, Party party, DungeonInstance dungeon, OutcomeBands outcomeOptions)
    {
        DungeonLevers _levers = GameStateQueries.GetDungeonLevers(dungeon.Rank);
        _levers.StatMinimum = dungeon.CalculateRequiredStat(e.EventType, _levers.StatMinimum);
        int _statDifference = party.PartyStats.might - _levers.StatMinimum + party.TraitEffects.CombatBonus;
        OutcomeBands _combatOptions = outcomeOptions.Clone();
        if (_statDifference <=_levers.DangerousStatDelta)
        {
            _combatOptions.Catastrophe.Weight = _combatOptions.Catastrophe.Weight * 1;
            _combatOptions.Failure.Weight = _combatOptions.Failure.Weight * 1;
            _combatOptions.Fled.Weight = _combatOptions.Fled.Weight * 1;
            _combatOptions.Success.Weight =  (int)(_combatOptions.Success.Weight * .5);
            _combatOptions.Triumph.Weight = 0;
        }
        else if (_statDifference <= _levers.RiskyStatDelta) // leave everything as normal
        {
        }
        else if (_statDifference <= _levers.SafeStatDelta) 
        {
            _combatOptions.Catastrophe.Weight = 0;
            _combatOptions.Failure.Weight = (int)(_combatOptions.Failure.Weight * .5);
            _combatOptions.Fled.Weight = (int)(_combatOptions.Fled.Weight * .5);
            _combatOptions.Success.Weight =  _combatOptions.Success.Weight * 2;
            _combatOptions.Triumph.Weight = _combatOptions.Triumph.Weight  * 1;
        }
        else if (_statDifference > _levers.SafeStatDelta) 
        {
            _combatOptions.Catastrophe.Weight = 0;
            _combatOptions.Failure.Weight = 0;
            _combatOptions.Fled.Weight = (int)(_combatOptions.Fled.Weight * .5);
            _combatOptions.Success.Weight =  _combatOptions.Success.Weight * 1;
            _combatOptions.Triumph.Weight = _combatOptions.Triumph.Weight  * 1;
        }

        WeightedRoll(_combatOptions);
        return;
    }

    //Control
    //finesse to dodge damage -> will implement this when I add in Caught Damage Type
    private void TrapResolver(SO_Event e, Party party, DungeonInstance dungeon, OutcomeBands outcomeOptions)
    {
        DungeonLevers _levers = GameStateQueries.GetDungeonLevers(dungeon.Rank);
        _levers.StatMinimum = dungeon.CalculateRequiredStat(e.EventType, _levers.StatMinimum);
        int _statDifference = party.PartyStats.control - _levers.StatMinimum + party.TraitEffects.TrapBonus;
        OutcomeBands _trapOptions = outcomeOptions.Clone();
        if (_statDifference <= _levers.DangerousStatDelta)
        {
            _trapOptions.Catastrophe.Weight = _trapOptions.Catastrophe.Weight * 1;
            _trapOptions.Failure.Weight = _trapOptions.Failure.Weight * 1;
            _trapOptions.Fled.Weight = _trapOptions.Fled.Weight * 1;
            _trapOptions.Success.Weight =  (int)(_trapOptions.Success.Weight * .5);
            _trapOptions.Triumph.Weight = 0;
        }
        else if (_statDifference <= _levers.RiskyStatDelta)
        {
            _trapOptions.Catastrophe.Weight = (int)(_trapOptions.Catastrophe.Weight * .5);
            _trapOptions.Failure.Weight = _trapOptions.Failure.Weight * 1;
            _trapOptions.Fled.Weight = _trapOptions.Fled.Weight * 1;
            _trapOptions.Success.Weight =  _trapOptions.Success.Weight * 1;
            _trapOptions.Triumph.Weight = (int)(_trapOptions.Triumph.Weight  * .5); 
        }
        else if (_statDifference <= _levers.SafeStatDelta) 
        {
            _trapOptions.Catastrophe.Weight = 0;
            _trapOptions.Failure.Weight = (int)(_trapOptions.Failure.Weight * .5);
            _trapOptions.Fled.Weight = _trapOptions.Fled.Weight * 1;
            _trapOptions.Success.Weight =  _trapOptions.Success.Weight * 2;
            _trapOptions.Triumph.Weight = _trapOptions.Triumph.Weight  * 1;
        }
        else if (_statDifference > _levers.SafeStatDelta) 
        {
            _trapOptions.Catastrophe.Weight = 0;
            _trapOptions.Failure.Weight = 0;
            _trapOptions.Fled.Weight = (int)(_trapOptions.Fled.Weight * .5);
            _trapOptions.Success.Weight =  _trapOptions.Success.Weight * 1;
            _trapOptions.Triumph.Weight = _trapOptions.Triumph.Weight  * 1;
        }

        WeightedRoll(_trapOptions);
        return;
    }
    //finesse to get through
    private void HazardResolver(SO_Event e, Party party, DungeonInstance dungeon, OutcomeBands outcomeOptions)
    {
        DungeonLevers _levers = GameStateQueries.GetDungeonLevers(dungeon.Rank);
        _levers.StatMinimum = dungeon.CalculateRequiredStat(e.EventType, _levers.StatMinimum);
        int _statDifference = (party.PartyStats.finesse - _levers.StatMinimum) + party.TraitEffects.HazardBonus;

        OutcomeBands _hazardOptions = outcomeOptions.Clone();
        if (_statDifference <= _levers.DangerousStatDelta)
        {
            _hazardOptions.Catastrophe.Weight = _hazardOptions.Catastrophe.Weight * 1;
            _hazardOptions.Failure.Weight = _hazardOptions.Failure.Weight * 1;
            _hazardOptions.Fled.Weight = (int)(_hazardOptions.Fled.Weight * 1.5);
            _hazardOptions.Success.Weight =  (int)(_hazardOptions.Success.Weight * .5);
            _hazardOptions.Triumph.Weight = 0;
        }
        else if (_statDifference <= _levers.RiskyStatDelta)
        {
            _hazardOptions.Catastrophe.Weight = (int)(_hazardOptions.Catastrophe.Weight * .5);
            _hazardOptions.Failure.Weight = _hazardOptions.Failure.Weight * 1;
            _hazardOptions.Fled.Weight = (int)(_hazardOptions.Fled.Weight * .5);
            _hazardOptions.Success.Weight =  _hazardOptions.Success.Weight * 1;
            _hazardOptions.Triumph.Weight = (int)(_hazardOptions.Triumph.Weight  * .5); 
        }
        else if (_statDifference <= _levers.SafeStatDelta) 
        {
            _hazardOptions.Catastrophe.Weight = 0;
            _hazardOptions.Failure.Weight = (int)(_hazardOptions.Failure.Weight * .5);
            _hazardOptions.Fled.Weight = (int)(_hazardOptions.Fled.Weight * .5);
            _hazardOptions.Success.Weight =  _hazardOptions.Success.Weight * 2;
            _hazardOptions.Triumph.Weight = _hazardOptions.Triumph.Weight  * 1;
        }
        else if (_statDifference > _levers.SafeStatDelta) 
        {
            _hazardOptions.Catastrophe.Weight = 0;
            _hazardOptions.Failure.Weight = 0;
            _hazardOptions.Fled.Weight = 0;
            _hazardOptions.Success.Weight =  _hazardOptions.Success.Weight * 1;
            _hazardOptions.Triumph.Weight = _hazardOptions.Triumph.Weight  * 1;
        }

        WeightedRoll(_hazardOptions);
        return;
    }

    //arcana to spot
    private void TreasureResolver(SO_Event e, Party party, DungeonInstance dungeon, OutcomeBands outcomeOptions)
    {
        DungeonLevers _levers = GameStateQueries.GetDungeonLevers(dungeon.Rank);
        _levers.StatMinimum = dungeon.CalculateRequiredStat(e.EventType, _levers.StatMinimum);
        int _statDifference = (party.PartyStats.arcana - _levers.StatMinimum) + party.TraitEffects.TreasureBonus;
        OutcomeBands _treasureOptions = outcomeOptions.Clone();
        _treasureOptions.Failure.GoldGainedModifier = 0; //failing means you didnt recover it

        //dont need the other versions, just success or failure
        _treasureOptions.Triumph.Weight = 0;
        _treasureOptions.Fled.Weight = 0;
        _treasureOptions.Catastrophe.Weight = 0;
        if (_statDifference <= _levers.DangerousStatDelta)
        {
            _treasureOptions.Failure.Weight = _treasureOptions.Failure.Weight * 2;
            _treasureOptions.Success.Weight =  (int)Mathf.Round(_treasureOptions.Success.Weight * .25f);
            
        }
        else if (_statDifference <= _levers.RiskyStatDelta) // 50/50 chance to get the treasure
        {
            _treasureOptions.Failure.Weight = _treasureOptions.Success.Weight;

        }
        else if (_statDifference > _levers.SafeStatDelta) //guaranteed to get treasure
        {
            _treasureOptions.Failure.Weight = 0;
        }

        WeightedRoll(_treasureOptions);
        return;
    }

    private void WeightedRoll(OutcomeBands options)
    {
        
        int _totalWeight = options.GetWeightTotal();
        float _r = Random.value * _totalWeight;

        _r -= options.Catastrophe.Weight;
        if( _r <= 0 )
        {
            eventOutcome = options.Catastrophe.Clone();
            eventResult.Outcome = OutcomeTypes.Catastrophe;
            return;
        }

        _r -= options.Failure.Weight;
        if( _r <= 0 )
        {
            eventOutcome = options.Failure.Clone();
            eventResult.Outcome = OutcomeTypes.Failure;
            return;
        }

        _r -= options.Fled.Weight;
        if( _r <= 0 )
        {
            eventOutcome = options.Fled.Clone();
            eventResult.Outcome = OutcomeTypes.Fled;
            return;
        }

        _r -= options.Success.Weight;
        if( _r <= 0 )
        {
            eventOutcome = options.Success.Clone();
            eventResult.Outcome = OutcomeTypes.Success;
            return;
        }

        _r -= options.Triumph.Weight;
        if( _r <= 0 )
        {
            eventOutcome = options.Triumph.Clone();
            eventResult.Outcome = OutcomeTypes.Triumph;
            return;
        }
    }

}