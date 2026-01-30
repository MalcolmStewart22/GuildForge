using UnityEngine;
using UnityEngine.UIElements;

public class DetailPanel : MonoBehaviour
{
    [SerializeField]
     TraitsListView traitsListView;
    [SerializeField]
     CharacterListView characterListView;
     DropdownField profileDropdown;
     Slider combatvSurvival;
     Slider conditioningvStudy;
     Slider medicinevReflection;
     Button saveButton;
     private Party currentParty;

    public void PopulateDungeonDetails(DungeonInstance dungeon, VisualElement container, string safety)
    {
        var _dungeonName = container.Q<Label>("DungeonName");
        _dungeonName.text = dungeon.Name;

        var _rank = container.Q<Label>("DungeonRank");
        _rank.text = dungeon.Rank.ToString();

        var _biome = container.Q<Label>("DungeonBiome");
        _biome.text = dungeon.Biome.Name;

        var _location = container.Q<Label>("DungeonLocation");
        _location.text = dungeon.Location.Name;

        var _enemy1 = container.Q<Label>("EnemyTag1");
        _enemy1.text = dungeon.Enemies[0].Name;

        var _enemy2 = container.Q<Label>("EnemyTag2");
        if(dungeon.Enemies.Count > 1)
        {
            _enemy2.text = dungeon.Enemies[1].Name;  
        }
        else
        {
            _enemy2.text = ""; 

        }
        
        var _safety = container.Q<Label>("DungeonSafety");
        _safety.text = safety;
    }

    public void PopulateCharacterDetails(Character character, VisualElement container, int currentGold)
    {
        var _characterName = container.Q<Label>("CharacterName");
        _characterName.text = character.Name;

        var _health = container.Q<ProgressBar>("HealthBar");
        if(character.IsResting)
        {
            _health.title = character.CurrentHP.ToString() + "/" + character.HPMax.ToString() + " (Injured)";
        }
        else
        {
            _health.title = character.CurrentHP.ToString() + "/" + character.HPMax.ToString();
        }
        _health.value = character.CurrentHP;
        _health.highValue = character.HPMax;
        var progress = _health.Q<VisualElement>(className: "unity-progress-bar__progress");

        progress.style.backgroundColor = character.CurrentHP > (character.HPMax * .3) ? Color.green : Color.red;

        

        var _rank = container.Q<Label>("CharacterRank");
        _rank.text = character.Rank.ToString();

        var _level = container.Q<Label>("CharacterLevel");
        if(character.Level < GameStateQueries.GetLevelCap(character.Rank))
        {
            _level.text = character.Level.ToString() + "/" + GameStateQueries.GetLevelCap(character.Rank).ToString();
        }
        else
        {
            _level.text = character.Level.ToString() + "/" + GameStateQueries.GetLevelCap(character.Rank).ToString() + "(MAX LEVEL)";
        }
        
        var _class = container.Q<Label>("CharacterClass");
        _class.text = character.Job.ToString();

        var _wage = container.Q<Label>("CurrentWage");
        _wage.text = GameStateQueries.GetWage(character.Rank).ToString();

        //stats
        var _might = container.Q<Label>("MightNumber");
        _might.text = character.Actual.might.ToString();

        var _finesse = container.Q<Label>("FinesseNumber");
        _finesse.text = character.Actual.finesse.ToString();

        var _endurance = container.Q<Label>("EnduranceNumber");
        _endurance.text = character.Actual.endurance.ToString();

        var _healing = container.Q<Label>("HealingNumber");
        _healing.text = character.Actual.healing.ToString();

        var _arcana = container.Q<Label>("ArcanaNumber");
        _arcana.text = character.Actual.arcana.ToString();

        var _control = container.Q<Label>("ControlNumber");
        _control.text = character.Actual.control.ToString();

        var _resolve = container.Q<Label>("ResolveNumber");
        _resolve.text = character.Actual.resolve.ToString();

        traitsListView.ShowTraits(character.Traits);

        //button costs
        //perhaps remove the text if number is 0? decide later

        var _healCost = container.Q<Label>("HealCost");
        _healCost.text = character.GetHealingCost().ToString();
        if(character.GetHealingCost() > currentGold)
        {
            container.Q<Button>("HealButton").SetEnabled(false);
        }
        else
        {
            container.Q<Button>("HealButton").SetEnabled(true);
        }

    }


    public void PopulatePartyList(VisualElement container, Party party)
    {
        currentParty = party;
        // profile
        profileDropdown = container.Q<DropdownField>("PartyProfiles");
        profileDropdown.choices = GameStateQueries.GetProfiles();
        profileDropdown.SetValueWithoutNotify(party.Profile.ToString());
        
        // Training
        combatvSurvival = container.Q<Slider>("CombatvsSurvivalSlider");
        combatvSurvival.SetValueWithoutNotify(party.TrainingInfo.CombatvsSurvival);
        conditioningvStudy = container.Q<Slider>("ConditioningvsStudySlider");
        conditioningvStudy.SetValueWithoutNotify(party.TrainingInfo.ConditioningvsStudy);
        medicinevReflection = container.Q<Slider>("MedicinevsReflectionSlider");
        medicinevReflection.SetValueWithoutNotify(party.TrainingInfo.MedicinevsReflection);

        saveButton = container.Q<Button>("SavePartyConfig");
        saveButton.style.display = DisplayStyle.None;
        
        SetupCallbacks();

        container.Q<Label>("PartyName").text = party.PartyName;

        characterListView.ShowRoster(party.PartyMembers);
    }

    #region On config changed methods and setup
    void SetupCallbacks()
    {
        profileDropdown.UnregisterValueChangedCallback(OnConfigChanged);
        combatvSurvival.UnregisterValueChangedCallback(OnConfigChanged);
        conditioningvStudy.UnregisterValueChangedCallback(OnConfigChanged);
        medicinevReflection.UnregisterValueChangedCallback(OnConfigChanged);

        profileDropdown.RegisterValueChangedCallback(OnConfigChanged);
        combatvSurvival.RegisterValueChangedCallback(OnConfigChanged);
        conditioningvStudy.RegisterValueChangedCallback(OnConfigChanged);
        medicinevReflection.RegisterValueChangedCallback(OnConfigChanged);
    }
    private void OnConfigChanged(ChangeEvent<string> newProfile)
    {
        Debug.Log("Profile Changed!");
        saveButton.style.display = DisplayStyle.Flex;
    }
    private void OnConfigChanged(ChangeEvent<float> newValue)
    {
        saveButton.style.display = DisplayStyle.Flex;
    }
    #endregion
    public void SavePartyConfig()
    {
        currentParty.Profile = GameStateQueries.GetProfileType(profileDropdown.value);
        currentParty.TrainingInfo.CombatvsSurvival = combatvSurvival.value;
        currentParty.TrainingInfo.ConditioningvsStudy = conditioningvStudy.value;
        currentParty.TrainingInfo.MedicinevsReflection = medicinevReflection.value;
    }
}