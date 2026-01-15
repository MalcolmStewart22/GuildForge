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

        List<SO_Event> _missionEvents = new();
        
        float _totalWeight = 0;
        foreach (var e in library.AllEvents)
        {
            _totalWeight += CalculateEffectiveWeight(e, dungeon.CalculatedModifier);            
        }
        
        List<SO_Event> _temp = new(library.AllEvents);

        for (int i = 0; i < dungeon.NumberOfEvents; i++)
        {
            float _r = Random.value * _totalWeight;
            SO_Event _selected = null;
            foreach (var e in _temp)
            {
                _r -= CalculateEffectiveWeight(e, dungeon.CalculatedModifier);
                if(_r <= 0)
                {
                    _missionEvents.Add(e);
                    _selected = e;
                    break;
                }
            }
            _temp.Remove(_selected);
            _totalWeight -= CalculateEffectiveWeight(_selected, dungeon.CalculatedModifier);
        }

        foreach (var e in _missionEvents)
        {
            Debug.Log(e.Name);
            missionResult.EventResults.Add(eventResolver.ResolveEvent(e, party, dungeon, outcomeOptions));
        }
        
        //when I institute dungeon fleeing this will have to change somewhat
        int _outcomeAsNum = 0;
        int _totalExp = 0;
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
            _totalExp += e.ExpGained;
        }
        missionResult.LevelUpReports = new List<LevelUpReport>(party.AssignExp(_totalExp));
        _outcomeAsNum = _outcomeAsNum / missionResult.EventResults.Count;
        switch(_outcomeAsNum)
        {
            case 1:
                missionResult.Outcome = OutcomeTypes.Catastrophe;
                break;
            case 2:
                missionResult.Outcome = OutcomeTypes.Failure;
                break;
            case 3:
                missionResult.Outcome = OutcomeTypes.Success;
                break;
            case 4:
                missionResult.Outcome = OutcomeTypes.Triumph;
                break;
        }

        Debug.Log("==== Mission Completed ====");
        return missionResult;
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
}


