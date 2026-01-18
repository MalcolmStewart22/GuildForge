using System.Collections.Generic;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class DungeonInstance
{
    public int ID;
    public string Name;
    public DungeonRank Rank;
    public SO_DungeonTag Biome;
    public SO_DungeonTag Location;
    public List<SO_DungeonTag> Enemies = new();
    public int NumberOfEvents;
    public int MinimumPayout;
    public DungeonModifiers CalculatedModifier = new();



    public int CalculateRequiredStat(EventType e)
    {
        int statMinimum = GameStateQueries.GetStatMinimum(Rank);
        switch(e)
        {
            case EventType.Combat:
                DungeonModifiers _combatMod = new();
                _combatMod = _combatMod.CombineModifiers(_combatMod, Biome.Effects);
                foreach(var enemy in Enemies)
                {
                    _combatMod = _combatMod.CombineModifiers(_combatMod, enemy.Effects);
                }
                return (int)(statMinimum * _combatMod.MightWeight);
            case EventType.Trap:
                DungeonModifiers _trapMod = new();
                _trapMod = _trapMod.CombineModifiers(Location.Effects, Biome.Effects);
                return (int)(statMinimum * _trapMod.ControlWeight);
            case EventType.Hazard:
                DungeonModifiers _hazardMod = new();
                _hazardMod = _hazardMod.CombineModifiers(Location.Effects, Biome.Effects);
                return (int)(statMinimum * _hazardMod.FinesseWeight);
            case EventType.Treasure:
                DungeonModifiers _treasureMod = new();
                _treasureMod = _treasureMod.CombineModifiers(Location.Effects, Biome.Effects);
                return (int)(statMinimum * _treasureMod.ArcanaWeight);
        }
        return statMinimum;
    }
}
