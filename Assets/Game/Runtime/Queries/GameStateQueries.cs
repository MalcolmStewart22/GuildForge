using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameStateQueries
{
    private static SO_GameConfig Config;

    public static void Setup(SO_GameConfig config)
    {
        Config = config;
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
                return gameState.Dungeons.Where(d => d.Biome.Name == "Frozen Wastes").ToList();
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

    public static float GetOutcomeGoldModifier(OutcomeTypes outcome)
    {
        switch (outcome)
        {
            case OutcomeTypes.Triumph:
                return Config.OutcomeOptions.Triumph.GoldGainedModifier;
            case OutcomeTypes.Success:
                return Config.OutcomeOptions.Success.GoldGainedModifier;
            case OutcomeTypes.Fled:
                return Config.OutcomeOptions.Fled.GoldGainedModifier;
            case OutcomeTypes.Failure:
                return Config.OutcomeOptions.Failure.GoldGainedModifier;
            case OutcomeTypes.Catastrophe:
                return Config.OutcomeOptions.Catastrophe.GoldGainedModifier;
        }
        
        return 1; //if the switch somehow breaks (should be impossible) dont break the game 
    }
    public static int GetStatMinimum(DungeonRank rank)
    {
        int result = 0;
        switch(rank)
        {
            case DungeonRank.E:
                result =  Config.DungeonMinimumStatRankE;
                break;
            case DungeonRank.D:
                result =  Config.DungeonMinimumStatRankE;
                break;
            case DungeonRank.C:
                result =  Config.DungeonMinimumStatRankE;
                break;
            case DungeonRank.B:
                result =  Config.DungeonMinimumStatRankE;
                break;
            case DungeonRank.A:
                result =  Config.DungeonMinimumStatRankE;
                break;
            case DungeonRank.S:
                result =  Config.DungeonMinimumStatRankE;
                break;
        }
        return result;
    }
    public static Party GetParty(GameState gameState)
    {
        return gameState.Parties[0];
    }
    public static int GetMaxPartySize()
    {
        return Config.MaxPartySize;
    }

    public static CharacterRankEvaluation GetCharacterRankEvaluation(Character character, int gold)//Cant be called on S rank character - Prevention logic will live elsewhere
    {
        CharacterRankEvaluation _result = new();
        JobStatFocus _focus = Config.JobStatMap.First(x => x.Job == character.Job);
        _result.PrimaryStat = _focus.Primary;
        _result.SecondaryStat = _focus.Secondary;

        switch(character.Rank)
        {
            case CharacterRank.E:
                _result.PromotionCost = Config.RankE.PromotionCost;
                _result.CurrentWage = Config.RankE.Wage;
                _result.NextWage = Config.RankD.Wage;
                _result.NextRank = CharacterRank.D;
                _result.PrimaryStatRequirement = Config.RankE.PrimaryStatRequirement;
                _result.SecondaryStatRequirement = Config.RankE.SecondaryStatRequirement;
                _result.RequiredLevel = Config.RankE.RequiredLevel;
                break;
            case CharacterRank.D:
                _result.PromotionCost = Config.RankD.PromotionCost;
                _result.CurrentWage = Config.RankD.Wage;
                _result.NextWage = Config.RankC.Wage;
                _result.NextRank = CharacterRank.C;
                _result.PrimaryStatRequirement = Config.RankD.PrimaryStatRequirement;
                _result.SecondaryStatRequirement = Config.RankD.SecondaryStatRequirement;
                _result.RequiredLevel = Config.RankD.RequiredLevel;
                break;
            case CharacterRank.C:
                _result.PromotionCost = Config.RankC.PromotionCost;
                _result.CurrentWage = Config.RankC.Wage;
                _result.NextWage = Config.RankB.Wage;
                _result.NextRank = CharacterRank.B;
                _result.PrimaryStatRequirement = Config.RankC.PrimaryStatRequirement;
                _result.SecondaryStatRequirement = Config.RankC.SecondaryStatRequirement;
                _result.RequiredLevel = Config.RankC.RequiredLevel;
                break;
            case CharacterRank.B:
                _result.PromotionCost = Config.RankB.PromotionCost;
                _result.CurrentWage = Config.RankB.Wage;
                _result.NextWage = Config.RankA.Wage;
                _result.NextRank = CharacterRank.A;
                _result.PrimaryStatRequirement = Config.RankB.PrimaryStatRequirement;
                _result.SecondaryStatRequirement = Config.RankB.SecondaryStatRequirement;
                _result.RequiredLevel = Config.RankB.RequiredLevel;
                break;
            case CharacterRank.A:
                _result.PromotionCost = Config.RankA.PromotionCost;
                _result.CurrentWage = Config.RankA.Wage;
                _result.NextWage = Config.RankS.Wage;
                _result.NextRank = CharacterRank.S;
                _result.PrimaryStatRequirement = Config.RankA.PrimaryStatRequirement;
                _result.SecondaryStatRequirement = Config.RankA.SecondaryStatRequirement;
                _result.RequiredLevel = Config.RankA.RequiredLevel;
                break;
        }

        _result.MeetsRequiredLevel = character.Level >= _result.RequiredLevel;
        _result.MeetsRequiredPrimaryStat = character.Base.GetStat(_focus.Primary) >= _result.PrimaryStatRequirement;
        _result.MeetsRequiredSecondaryStat = character.Base.GetStat(_focus.Secondary) >= _result.SecondaryStatRequirement;
        _result.HasEnoughGold = gold >= _result.PromotionCost;
        _result.CanPromote = 
            _result.MeetsRequiredLevel && 
            _result.MeetsRequiredPrimaryStat &&
            _result.MeetsRequiredSecondaryStat &&
            _result.HasEnoughGold;

        return _result;   
    }

    public static GuildRankEvaluation GetGuildRankEvaluation(GameState gameState)
    {
        return new GuildRankEvaluation();
    }

    public static int GetWage(CharacterRank rank)
    {
        switch(rank)
        {
            case CharacterRank.E:
                return Config.RankE.Wage;
            case CharacterRank.D:
                return Config.RankD.Wage;
            case CharacterRank.C:
                return Config.RankC.Wage;
            case CharacterRank.B:
                return Config.RankB.Wage;
            case CharacterRank.A:
                return Config.RankA.Wage;
            case CharacterRank.S:
                return Config.RankS.Wage;
        }
        return Config.RankE.Wage;
    }

    public static int GetLevelCap(CharacterRank rank)
    {
        switch(rank)
        {
            case CharacterRank.E:
                return Config.RankE.LevelCap;
            case CharacterRank.D:
                return Config.RankD.LevelCap;
            case CharacterRank.C:
                return Config.RankC.LevelCap;
            case CharacterRank.B:
                return Config.RankB.LevelCap;
            case CharacterRank.A:
                return Config.RankA.LevelCap;
            case CharacterRank.S:
                return Config.RankS.LevelCap;
        }
        return Config.RankE.Wage;
    }
}