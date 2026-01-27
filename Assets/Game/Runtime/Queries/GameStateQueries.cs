using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
    public static List<string> GetProfiles()
    {
        List<string> _result = new();
        foreach(PartyProfile p in Config.PartyProfiles)
        {
            _result.Add(p.ProfileType.ToString());
        }
        return _result;
    }
    public static PartyProfileType GetProfileType(string name)
    {
        PartyProfileType _result = PartyProfileType.Default;

        switch(name)
        {
            case "Default":
                _result = PartyProfileType.Default;
                break;
            case "Greedy":
                _result = PartyProfileType.Default;
                break;
            case "Cautious":
                _result = PartyProfileType.Default;
                break;
            case "Reckless":
                _result = PartyProfileType.Default;
                break;
        }
        return _result;
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
    public static DungeonLevers GetDungeonLevers(DungeonRank rank)
    {
        switch(rank)
        {
            case DungeonRank.E:
                return new DungeonLevers(Config.DungeonRankEMinimum, Config.BaseDeltas);
            case DungeonRank.D:
                return new DungeonLevers(Config.DungeonRankDMinimum, Config.BaseDeltas);
            case DungeonRank.C:
                return new DungeonLevers(Config.DungeonRankCMinimum, Config.BaseDeltas);
            case DungeonRank.B:
                return new DungeonLevers(Config.DungeonRankBMinimum, Config.BaseDeltas);
            case DungeonRank.A:
                return new DungeonLevers(Config.DungeonRankAMinimum, Config.BaseDeltas);
            case DungeonRank.S:
                return new DungeonLevers(Config.DungeonRankSMinimum, Config.BaseDeltas);
        }
        return null;
    }
    public static Party GetParty(GameState gameState)
    {
        return gameState.Parties[0];
    }
    public static int GetDungeonMinimumPayout(DungeonRank rank)
    {
        return  (int)MathF.Round(Config.MaxPartySize * GetWage((CharacterRank)rank) * (1 / Config.ExpectedMissionRatio));
    }

    public static CharacterRankEvaluation GetCharacterRankEvaluation(Character character, int gold)//Cant be called on S rank character - Prevention logic will live elsewhere
    {
        CharacterRankEvaluation _result = new();
        _result.CurrentRank = character.Rank;
        if(character.Rank == CharacterRank.S)
        {
            _result.CanPromote = false;
            return _result;
        }
        JobStatFocus _focus = Config.JobStatMap.First(x => x.Job == character.Job);
        _result.PrimaryStat = _focus.Primary;
        _result.SecondaryStat = _focus.Secondary;
        Debug.Log($"Focus collected: {_focus.Job} {_focus.Primary} {_focus.Secondary}");
        switch(character.Rank)
        {
            case CharacterRank.E:
                _result.PromotionCost = Config.CharacterRankE.PromotionCost;
                _result.CurrentWage = Config.CharacterRankE.Wage;
                _result.NextWage = Config.CharacterRankD.Wage;
                _result.NextRank = CharacterRank.D;
                _result.PrimaryStatRequirement = Config.CharacterRankE.PrimaryStatRequirement;
                _result.SecondaryStatRequirement = Config.CharacterRankE.SecondaryStatRequirement;
                _result.RequiredLevel = Config.CharacterRankE.RequiredLevel;
                break;
            case CharacterRank.D:
                _result.PromotionCost = Config.CharacterRankD.PromotionCost;
                _result.CurrentWage = Config.CharacterRankD.Wage;
                _result.NextWage = Config.CharacterRankC.Wage;
                _result.NextRank = CharacterRank.C;
                _result.PrimaryStatRequirement = Config.CharacterRankD.PrimaryStatRequirement;
                _result.SecondaryStatRequirement = Config.CharacterRankD.SecondaryStatRequirement;
                _result.RequiredLevel = Config.CharacterRankD.RequiredLevel;
                break;
            case CharacterRank.C:
                _result.PromotionCost = Config.CharacterRankC.PromotionCost;
                _result.CurrentWage = Config.CharacterRankC.Wage;
                _result.NextWage = Config.CharacterRankB.Wage;
                _result.NextRank = CharacterRank.B;
                _result.PrimaryStatRequirement = Config.CharacterRankC.PrimaryStatRequirement;
                _result.SecondaryStatRequirement = Config.CharacterRankC.SecondaryStatRequirement;
                _result.RequiredLevel = Config.CharacterRankC.RequiredLevel;
                break;
            case CharacterRank.B:
                _result.PromotionCost = Config.CharacterRankB.PromotionCost;
                _result.CurrentWage = Config.CharacterRankB.Wage;
                _result.NextWage = Config.CharacterRankA.Wage;
                _result.NextRank = CharacterRank.A;
                _result.PrimaryStatRequirement = Config.CharacterRankB.PrimaryStatRequirement;
                _result.SecondaryStatRequirement = Config.CharacterRankB.SecondaryStatRequirement;
                _result.RequiredLevel = Config.CharacterRankB.RequiredLevel;
                break;
            case CharacterRank.A:
                _result.PromotionCost = Config.CharacterRankA.PromotionCost;
                _result.CurrentWage = Config.CharacterRankA.Wage;
                _result.NextWage = Config.CharacterRankS.Wage;
                _result.NextRank = CharacterRank.S;
                _result.PrimaryStatRequirement = Config.CharacterRankA.PrimaryStatRequirement;
                _result.SecondaryStatRequirement = Config.CharacterRankA.SecondaryStatRequirement;
                _result.RequiredLevel = Config.CharacterRankA.RequiredLevel;
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
        GuildRankEvaluation _result = new();
        _result.CurrentRank = gameState.CurrentGuildRank;
        if(gameState.CurrentGuildRank == GuildRank.S)
        {
            _result.CanPromote = false;
            return _result;
        }
        switch(gameState.CurrentGuildRank)
        {
            case GuildRank.E:
                _result.NumCharactersAtRank = GetRecruitedCharactersOfRank(CharacterRank.D, gameState).Count;
                _result.NumCharactersRequiredAtRank = Config.GuildRankE.CharactersRequiredToRankUp;
                _result.PromotionCost = Config.GuildRankE.PromotionCost;
                _result.NewPartyMax = Config.GuildRankD.MaxNumberOfParties;
                _result.NewRank =  GuildRank.D;
                break;
            case GuildRank.D:
                _result.NumCharactersAtRank = GetRecruitedCharactersOfRank(CharacterRank.C, gameState).Count;
                _result.NumCharactersRequiredAtRank = Config.GuildRankD.CharactersRequiredToRankUp;
                _result.PromotionCost = Config.GuildRankD.PromotionCost;
                _result.NewPartyMax = Config.GuildRankC.MaxNumberOfParties;
                _result.NewRank =  GuildRank.C;
                break;
            case GuildRank.C:
                _result.NumCharactersAtRank = GetRecruitedCharactersOfRank(CharacterRank.B, gameState).Count;
                _result.NumCharactersRequiredAtRank = Config.GuildRankC.CharactersRequiredToRankUp;
                _result.PromotionCost = Config.GuildRankC.PromotionCost;
                _result.NewPartyMax = Config.GuildRankB.MaxNumberOfParties;
                _result.NewRank =  GuildRank.B;
                break;
            case GuildRank.B:
                _result.NumCharactersAtRank = GetRecruitedCharactersOfRank(CharacterRank.A, gameState).Count;
                _result.NumCharactersRequiredAtRank = Config.GuildRankB.CharactersRequiredToRankUp;
                _result.PromotionCost = Config.GuildRankB.PromotionCost;
                _result.NewPartyMax = Config.GuildRankA.MaxNumberOfParties;
                _result.NewRank =  GuildRank.A;
                break;
            case GuildRank.A:
                _result.NumCharactersAtRank = GetRecruitedCharactersOfRank(CharacterRank.S, gameState).Count;
                _result.NumCharactersRequiredAtRank = Config.GuildRankA.CharactersRequiredToRankUp;
                _result.PromotionCost = Config.GuildRankA.PromotionCost;
                _result.NewPartyMax = Config.GuildRankS.MaxNumberOfParties;
                _result.NewRank =  GuildRank.S;
                break;
        }
        
        if(_result.NumCharactersAtRank >= _result.NumCharactersRequiredAtRank)
        {
            _result.MeetsCharacterRequirement = true;
        }
        if(gameState.CurrentGold >= _result.PromotionCost)
        {
            _result.HasEnoughGold = true;
        }
        _result.CanPromote = _result.MeetsCharacterRequirement && _result.HasEnoughGold;

        return _result;
    }
    public static List<Character> GetRecruitedCharactersOfRank(CharacterRank rank, GameState gameState)
    {
        switch(rank)
        {
            case CharacterRank.E:
                return gameState.Recruited.Where(c => c.Rank == CharacterRank.E).ToList();
            case CharacterRank.D:
                return gameState.Recruited.Where(c => c.Rank == CharacterRank.D).ToList();
            case CharacterRank.C:
                return gameState.Recruited.Where(c => c.Rank == CharacterRank.C).ToList();
            case CharacterRank.B:
                return gameState.Recruited.Where(c => c.Rank == CharacterRank.B).ToList();
            case CharacterRank.A:
                return gameState.Recruited.Where(c => c.Rank == CharacterRank.A).ToList();
            case CharacterRank.S:
                return gameState.Recruited.Where(c => c.Rank == CharacterRank.S).ToList();
        }
        return null;
    }

    public static int GetWage(CharacterRank rank)
    {
        switch(rank)
        {
            case CharacterRank.E:
                return Config.CharacterRankE.Wage;
            case CharacterRank.D:
                return Config.CharacterRankD.Wage;
            case CharacterRank.C:
                return Config.CharacterRankC.Wage;
            case CharacterRank.B:
                return Config.CharacterRankB.Wage;
            case CharacterRank.A:
                return Config.CharacterRankA.Wage;
            case CharacterRank.S:
                return Config.CharacterRankS.Wage;
        }
        return Config.CharacterRankE.Wage;
    }

    public static int GetLevelCap(CharacterRank rank)
    {
        switch(rank)
        {
            case CharacterRank.E:
                return Config.CharacterRankE.LevelCap;
            case CharacterRank.D:
                return Config.CharacterRankD.LevelCap;
            case CharacterRank.C:
                return Config.CharacterRankC.LevelCap;
            case CharacterRank.B:
                return Config.CharacterRankB.LevelCap;
            case CharacterRank.A:
                return Config.CharacterRankA.LevelCap;
            case CharacterRank.S:
                return Config.CharacterRankS.LevelCap;
        }
        return Config.CharacterRankE.Wage;
    }

    public static string GenerateName(string item)
    {
        System.Random rng = new System.Random();
        string name = "ReplaceMe";
        switch(item)
        {
            case "Character":
                string _prefix = Config.SyllableSet.PrefixSyllable[rng.Next(Config.SyllableSet.PrefixSyllable.Count)];
                string _suffix = Config.SyllableSet.SuffixSyllable[rng.Next(Config.SyllableSet.SuffixSyllable.Count)];
                string _middle = "";

                if(rng.NextDouble() < .5)
                {
                    _middle = Config.SyllableSet.MiddleSyllable[rng.Next(Config.SyllableSet.MiddleSyllable.Count)];
                }
                name = _prefix + _middle + _suffix;
                break;
            case "Party":
                string _adj = Config.PartyNameSet.AdjectiveList[rng.Next(Config.PartyNameSet.AdjectiveList.Count)];
                string _noun = Config.PartyNameSet.NounList[rng.Next(Config.PartyNameSet.NounList.Count)];
                name = $"The {_adj} {_noun}";
                break;
        }
        return name;
    }


    public static List<Party> GetUnassignedParties(GameState gameState)
    {
        return gameState.Parties.Where(p => p.AssignedAction == PartyAction.Unassigned).ToList();
    }

    public static PartyRunPressure CalculateCurrentPressure(Party party)
    {
        PartyPressureWeight weights = GetPressureWeights(party.Profile);
        PartyRunPressure _result = new();
        //DeathPressure
        List<Character> temp = new List<Character>(party.PartyMembers);
        foreach (var c in temp)
        {
            if(!c.IsAlive)
            {
                _result.DeathPressure = 1 * weights.DeathPressureModifier;
            }
        }
    
        // hp pressure & Spike Pressure
        foreach(Character c in party.PartyMembers)
        {
            float currentHPPercent = (float)c.CurrentHP / c.HPMax;
            _result.HPPressure += Mathf.Clamp01((1 - currentHPPercent) / (1 - Config.HPConfig.RetreatHPFloor)) * weights.HPPressureModifier; 

            _result.SpikePressure += Mathf.Clamp01(c.LastDamage / (Config.HPConfig.SpikeHPFloor * c.HPMax)) * weights.SpikePressureModifier;
        }
        _result.HPPressure = _result.HPPressure / party.PartyMembers.Count;
        _result.SpikePressure = _result.SpikePressure / party.PartyMembers.Count;
        
        //LootPressure
        int _min = GetDungeonMinimumPayout(party.CurrentMission.Rank);
        _result.LootPressure = Mathf.Clamp01((float)party.CurrentLoot / (_min * 2)) * weights.LootPressureModifier;

        return _result;
    }
    public static bool CalculateRetreatOrders(PartyRunPressure pressure)
    {
        if(pressure.HPPressure > Config.PressureThresholds.HPThreshold)
        {
            return true;
        }
        else if(pressure.SpikePressure > Config.PressureThresholds.SpikeThreshold)
        {
            return true;
        }
        else if(pressure.DeathPressure > Config.PressureThresholds.DeathThreshold)
        {
            return true;
        }
        else if(pressure.LootPressure > Config.PressureThresholds.LootThreshold)
        {
            return true;
        }
        else if(pressure.HPPressure + pressure.SpikePressure + pressure.DeathPressure + pressure.LootPressure > Config.PressureThresholds.TotalThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static PartyPressureWeight GetPressureWeights(PartyProfileType profile)
    {
        return Config.PartyProfiles.FirstOrDefault(p => p.ProfileType == profile).Weights;
    }
}