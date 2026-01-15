using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public static class GameStateQueries
{
    public static List<Character> GetRoster(GameState gameState)
    {
        return gameState.Recruited;
    }
    public static List<DungeonInstance> GetFilteredDungeonList(string buttonName, GameState gameState)
    {
        switch(buttonName)
        {
            case "DesertButton":
                return gameState.Dungeons.Where(d => d.Biome.Name == "Desert").ToList();
            case "PlainsButton":
                return gameState.Dungeons.Where(d => d.Biome.Name == "Plains").ToList();
            case "MountainButton":
                return gameState.Dungeons.Where(d => d.Biome.Name == "Mountains").ToList();
            case "FrozenWastesButton":
                return gameState.Dungeons.Where(d => d.Biome.Name == "FrozenWastes").ToList();
            case "ForestButton":
                return gameState.Dungeons.Where(d => d.Biome.Name == "Forest").ToList();
            case "SwampButton":
                return gameState.Dungeons.Where(d => d.Biome.Name == "Swamp").ToList();
            case "SubterraneanButton":
                return gameState.Dungeons.Where(d => d.Biome.Name == "Subterranean").ToList();
            case "VolcanicButton":
                return gameState.Dungeons.Where(d => d.Biome.Name == "Volcanic").ToList();
        }
        return null;
    }

    public static int GetCurrentGold(GameState gameState)
    {
        return gameState.CurrentGold;
    }

    public static Party GetParty(GameState gameState)
    {
        return gameState.Parties[0];
    }
}