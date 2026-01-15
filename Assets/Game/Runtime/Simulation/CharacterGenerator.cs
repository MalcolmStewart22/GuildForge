using UnityEngine;

public class CharacterGenerator
{
    System.Random rng = new System.Random();
    public Character GenerateCharacter(int _id, SO_NameSyllableSet syllableSet, SO_TraitLibrary library)
    {
        Debug.Log("Creating a Character");
        Character _curCharacter = new Character();
        _curCharacter.CharacterID = _id;
        _curCharacter.Name = GenerateName(syllableSet);
        
        //temp solution until recruitment is added
        switch(_curCharacter.CharacterID)
        {
            case 0:
                _curCharacter.Job = CharacterJob.Damage;
                break;
            case 1:
                _curCharacter.Job = CharacterJob.Tank;
                break;
            case 2:
                _curCharacter.Job = CharacterJob.Support;
                break;
            case 3:
                _curCharacter.Job = CharacterJob.Control;
                break;
            default:
                _curCharacter.Job = CharacterJob.Damage;
                break;
        }

        //I want a different solution for rolling stats later
        _curCharacter.Base.might = RollStat();
        _curCharacter.Base.finesse = RollStat();
        _curCharacter.Base.endurance = RollStat();
        _curCharacter.Base.healing = RollStat();
        _curCharacter.Base.arcana = RollStat();
        _curCharacter.Base.control = RollStat();
        _curCharacter.Base.resolve = RollStat();

        //add a single random trait after you've built trait system
        float _r = Random.value * library.AllTraits.Count;

        foreach(var trait in library.AllTraits)
        {
            _r -= 1;
            if(_r <= 0)
            {
                _curCharacter.Traits.Add(trait);
                break;
            }
        }
        _curCharacter.CalculateActualStats();
        _curCharacter.HPMax = (int)(1.5 * _curCharacter.Actual.endurance);
        _curCharacter.CurrentHP = _curCharacter.HPMax;
        
        Debug.Log(_curCharacter.Name + " Created!");
        return _curCharacter;
    }

    private string GenerateName(SO_NameSyllableSet set)
    {
        string _prefix = set.PrefixSyllable[rng.Next(set.PrefixSyllable.Count)];
        string _suffix = set.SuffixSyllable[rng.Next(set.SuffixSyllable.Count)];
        string _middle = "";

        if(rng.NextDouble() < .5)
        {
            _middle = set.MiddleSyllable[rng.Next(set.MiddleSyllable.Count)];
        }
        return (_prefix + _middle + _suffix);
    }

    private int RollStat()
    {
        int roll = 0;
        int min = 6;
        for (int i = 0; i < 4; i++)
        {
            int newRoll = UnityEngine.Random.Range(1,7);
            if( newRoll < min)
            {
                min = newRoll;
            }
            roll += newRoll;
        }

        roll -= min;

        return roll;
    }
}
