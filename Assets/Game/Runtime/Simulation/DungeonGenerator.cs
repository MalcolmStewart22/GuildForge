using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator
{
    public DungeonInstance GenerateDungeon(int id, SO_TagLibrary library)
    {
        Debug.Log("Creating A Dungeon");
        DungeonInstance _d = new DungeonInstance();
        
        _d.ID = id;
        
        //Temporary version of Rank decision for MVP testing
        switch (_d.ID)
        {
            case 0:
            case 1:
                _d.Rank = DungeonRank.E;
                break;
            case 2:
            case 3:
                _d.Rank = DungeonRank.D;
                break;
            case 4:
            case 5:
                _d.Rank = DungeonRank.C;
                break;
            case 6:
            case 7:
                _d.Rank = DungeonRank.B;
                break;
            case 8:
                _d.Rank = DungeonRank.A;
                break;
            case 9:
                _d.Rank = DungeonRank.S;
                break;

        }
        switch(_d.Rank)
        {
            case DungeonRank.E:
                _d.NumberOfEvents = 2;
                _d.MinimumPayout = GameStateQueries.GetMaxPartySize() * GameStateQueries.GetWage(CharacterRank.E);
                break;
            case DungeonRank.D:
                _d.NumberOfEvents = 3;
                _d.MinimumPayout = GameStateQueries.GetMaxPartySize() * GameStateQueries.GetWage(CharacterRank.D);
                break;
            case DungeonRank.C:
                _d.NumberOfEvents = 3;
                _d.MinimumPayout = GameStateQueries.GetMaxPartySize() * GameStateQueries.GetWage(CharacterRank.C);
                break;
            case DungeonRank.B:
                _d.NumberOfEvents = 4;
                _d.MinimumPayout = GameStateQueries.GetMaxPartySize() * GameStateQueries.GetWage(CharacterRank.B);
                break;
            case DungeonRank.A:
                _d.NumberOfEvents = 5;
                _d.MinimumPayout = GameStateQueries.GetMaxPartySize() * GameStateQueries.GetWage(CharacterRank.A);
                break;
            case DungeonRank.S:
                _d.NumberOfEvents = 6;
                _d.MinimumPayout = GameStateQueries.GetMaxPartySize() * GameStateQueries.GetWage(CharacterRank.S);
                break;
        }
        
        #region Tags
        //biome
        int _totalWeight = 0;
        foreach(var tag in library.BiomeTags)
        {
            _totalWeight += tag.Weight;
        }

        float _r = Random.value * _totalWeight;

        foreach(var tag in library.BiomeTags)
        {
            //Debug.Log(tag.Name);
            _r -= tag.Weight;
            if(_r <= 0)
            {
                _d.Biome = tag;
                break;
            }
        }
        
        //Location
        _totalWeight = 0;
        foreach(var tag in library.LocationTags)
        {
            _totalWeight += tag.Weight;
        }
        
        _r = Random.value * _totalWeight;

        foreach(var tag in library.LocationTags)
        {
            _r -= tag.Weight;
            if(_r <= 0)
            {
                _d.Location = tag;
                break;
            }
        }
        //Enemy

        
        int _numOfEnemyTags = Random.Range(0,3);
        //Debug.Log("Number of Enemies Rolled: " + _numOfEnemyTags);
        if(_numOfEnemyTags > 0)
        {
            List<SO_DungeonTag> _tags = new List<SO_DungeonTag>(library.EnemyTags);

            _totalWeight = 0;
            foreach(var tag in _tags)
            {
                _totalWeight += tag.Weight;
            }
            
            for (int i = 0; i < _numOfEnemyTags; i++)
            {
                _r = Random.value * _totalWeight;
                SO_DungeonTag _t = null;
                foreach(var tag in _tags)
                {
                    _r -= tag.Weight;
                    
                    if(_r <= 0)
                    {
                        _d.Enemies.Add(tag);
                        _t = tag;
                        break;
                    }
                }
                _tags.Remove(_t);
                _totalWeight -= _t.Weight;
            } 
        }
        else
        {
            
            _d.Enemies.Add(_d.Biome.DefaultEnemy);
        }
        _d.Name = _d.Location.Name + " in " + _d.Biome.Name;
        #endregion
        
        _d.CalculatedModifier = _d.CalculatedModifier.CombineModifiers (_d.Biome.Effects, _d.Location.Effects);
        foreach (var tag in _d.Enemies)
        {
            _d.CalculatedModifier = _d.CalculatedModifier.CombineModifiers(_d.CalculatedModifier, tag.Effects);
        }

        Debug.Log(_d.Name + " Created!");
        return _d;
    }
}
