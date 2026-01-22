using System.Collections.Generic;
using UnityEngine;

//might rename seeing as it actually ended up only deciding finishing touches for the mission and the letting event resolver do all the lifting
public class DungeonResolver
{
    private MissionResult missionResult;

    public MissionResult EnterDungeon
        (DungeonInstance dungeon, Party party, SO_EventLibrary library, 
            EventResolver eventResolver, OutcomeBands outcomeOptions, int day)
    {
        Debug.Log("=== Mission Start====");
        
        missionResult = new MissionResult ();
        missionResult.Party = party;
        missionResult.Dungeon = dungeon;
        missionResult.MissionDay = day;

        foreach (var e in RollEventList(library))
        {
            missionResult.EventResults.Add(eventResolver.ResolveEvent(e, party, dungeon, outcomeOptions));
        }

        missionResult.Outcome = CalculateMissionOutcome();

        int _goldMinimum = CalculateGoldMinimum(missionResult.Dungeon.MinimumPayout, missionResult.Outcome);
        if(missionResult.GoldGained < _goldMinimum ) //enforcing a minimum gold return to make sure you dont lose from drawing bad events when you were successful
        {
            missionResult.GoldGained = _goldMinimum;
        }

        SendPartyHome();

        Debug.Log($"==== {party.PartyName} Mission Completed ====");
        return missionResult;
    }
    private List<SO_Event> RollEventList(SO_EventLibrary library)
    {
        List<SO_Event> _missionEvents = new();
        
        float _totalWeight = 0;
        foreach (var e in library.AllEvents)
        {
            Debug.Log($"{e.Name}");
            Debug.Log($"{missionResult.Dungeon.Name}");
            _totalWeight += CalculateEffectiveWeight(e, missionResult.Dungeon.CalculatedModifier);            
        }
        
        List<SO_Event> _temp = new(library.AllEvents);

        for (int i = 0; i < missionResult.Dungeon.NumberOfEvents; i++)
        {
            float _r = Random.value * _totalWeight;
            SO_Event _selected = null;
            foreach (var e in _temp)
            {
                _r -= CalculateEffectiveWeight(e, missionResult.Dungeon.CalculatedModifier);
                if(_r <= 0)
                {
                    _missionEvents.Add(e);
                    _selected = e;
                    break;
                }
            }
            _temp.Remove(_selected);
            _totalWeight -= CalculateEffectiveWeight(_selected, missionResult.Dungeon.CalculatedModifier);
        }
        return _missionEvents;
    }
    private float CalculateEffectiveWeight(SO_Event e, DungeonModifiers modifier)
    {
        switch(e.EventType)
        {
            case EventType.Combat:
                return e.Weight * modifier.CombatWeight;
            case EventType.Treasure:
                return e.Weight * modifier.TreasureWeight;
            case EventType.Trap:
                return e.Weight * modifier.TrapWeight;
            case EventType.Hazard:
                return e.Weight * modifier.HazardWeight;
            default:
                return e.Weight;
        }
    }

    private int CalculateGoldMinimum(int min, OutcomeTypes outcome)
    { 
        int _result = 0;
        switch(outcome)
        {
            case OutcomeTypes.Triumph:
                _result = (int)(min * GameStateQueries.GetOutcomeGoldModifier(OutcomeTypes.Triumph));
                break;
            case OutcomeTypes.Success:
                _result = (int)(min * GameStateQueries.GetOutcomeGoldModifier(OutcomeTypes.Success));
                break;
            case OutcomeTypes.Failure:
                _result = (int)(min * GameStateQueries.GetOutcomeGoldModifier(OutcomeTypes.Failure));
                break;
            case OutcomeTypes.Catastrophe:
                _result = (int)(min * GameStateQueries.GetOutcomeGoldModifier(OutcomeTypes.Catastrophe));
                break;
        }
        return _result;
    }
    private OutcomeTypes CalculateMissionOutcome()
    {
        //when I institute dungeon fleeing this will have to change somewhat
        int _outcomeAsNum = 0;
        foreach( var e in missionResult.EventResults)
        {
            missionResult.GoldGained += e.GoldGained;
            switch (e.Outcome)
            {
                case OutcomeTypes.Triumph:
                    _outcomeAsNum += 4;
                    break;
                case OutcomeTypes.Success:
                    _outcomeAsNum += 3;
                    break;
                case OutcomeTypes.Fled:
                    _outcomeAsNum += 2;
                    break;
                case OutcomeTypes.Failure:
                    _outcomeAsNum += 2;
                    break;
                case OutcomeTypes.Catastrophe:
                    _outcomeAsNum += 1;
                    break;
            }
        }
        _outcomeAsNum = Mathf.RoundToInt(_outcomeAsNum / (missionResult.EventResults.Count * 1f));// I want a failure + Success to equal success. Just weighting it up a little
        switch (_outcomeAsNum)
        {
            case 1:
                return OutcomeTypes.Catastrophe;
            case 2:
                return OutcomeTypes.Failure;
            case 3:
                return OutcomeTypes.Success;
            case 4:
                return OutcomeTypes.Triumph;
        }
        return OutcomeTypes.Error;
    }
    private void SendPartyHome()
    {
        int _totalExp = 0;
        foreach( var e in missionResult.EventResults)
        {
            _totalExp += e.ExpGained;
        }
        missionResult.LevelUpReports = new List<LevelUpReport>(missionResult.Party.AssignExp(_totalExp));
        missionResult.Party.GoHome();
    }

}


