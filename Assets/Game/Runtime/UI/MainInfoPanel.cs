using UnityEngine;
using UnityEngine.UIElements;

public class MainInfoPanel : MonoBehaviour
{
    [SerializeField]
     PartyListView partyListView;
    [SerializeField]
     EventActionListView eventActionListView;


    public void ShowMissionReport(VisualElement container, MissionResult mission)
    {
        container.style.display = DisplayStyle.Flex;
        //labels
        Label _week = container.Q<Label>("WeekCount");
        _week.text = mission.MissionWeek.ToString();
        Label _dungeonName = container.Q<Label>("DungeonName");
        _dungeonName.text = mission.Dungeon.Name;
        Label _outcome = container.Q<Label>("Outcome");
        _outcome.text = mission.Outcome.ToString();
        switch (mission.Outcome)
            {
                case OutcomeTypes.Triumph:
                    _outcome.style.color = Color.green;
                    break;
                case OutcomeTypes.Success:
                    _outcome.style.color = Color.yellow;
                    break;
                case OutcomeTypes.Failure:
                    _outcome.style.color = Color.orange;
                    break;
                case OutcomeTypes.Catastrophe:
                    _outcome.style.color = Color.red;
                    break;
            }
        Label _goldGained = container.Q<Label>("GoldGained");
        _goldGained.text = mission.GoldGained.ToString();

        // Events
        eventActionListView.ShowEvents(mission.EventResults);

        // Party
        partyListView.ShowParty(mission.LevelUpReports);
    }

    public void ShowGuildRankInfo(VisualElement container, GuildRankEvaluation rankEval)
    {
        container.style.display = DisplayStyle.Flex;
        if(rankEval.CurrentRank == GuildRank.S)
        {
            container.Q<VisualElement>("RequirementsBox").style.display = DisplayStyle.None;
            container.Q<VisualElement>("EffectsBox").style.display = DisplayStyle.None;
            container.Q<VisualElement>("Cost").style.display = DisplayStyle.None;

            container.Q<Label>("GuildRankExplanation").text = "MAX RANK /n No Promotions Available!";
        }
        else
        {
            //Requirments
            Label _req = container.Q<Label>("RankText");
            _req.text = $"{rankEval.NewRank} Rank Characters: {rankEval.NumCharactersAtRank}/{rankEval.NumCharactersRequiredAtRank}";

            Label _gold = container.Q<Label>("CostToRankUp");
            _gold.text = rankEval.PromotionCost.ToString();

            //effects
            Label _rank = container.Q<Label>("NewRank");
            _rank.text = rankEval.NewRank.ToString();
            
            Label _partyCap = container.Q<Label>("NewParties");
            _partyCap.text = rankEval.NewPartyMax.ToString();

            //Conditional
            Toggle _rankToggle = container.Q<Toggle>("RankToggle");
            _rankToggle.value = rankEval.MeetsCharacterRequirement;
            _rank.style.color = rankEval.MeetsCharacterRequirement ? Color.gray : Color.black;
        }
        

    }
    public void ShowLedger(VisualElement container)
    {
        container.style.display = DisplayStyle.Flex;
    }

     public void ShowCharacterRankInfo(VisualElement container, Character character, CharacterRankEvaluation rankEval)
    {
        container.style.display = DisplayStyle.Flex;

        Label _name = container.Q<Label>("RankUpName");
        _name.text = "Promote " + character.Name + "?";

        if(rankEval.CurrentRank == CharacterRank.S)
        {
            container.Q<VisualElement>("RequirementsBox").style.display = DisplayStyle.None;
            container.Q<VisualElement>("EffectsBox").style.display = DisplayStyle.None;
            container.Q<VisualElement>("Cost").style.display = DisplayStyle.None;

            container.Q<Label>("CharacterRankExplanation").text = "MAX RANK /n No Promotions Available!";
        }
        else
        {
            container.Q<Label>("CharacterRankExplanation").text = "Promotion recognizes a character as a higher-ranked operative of the guild. /n" + 
                "Higher rank allows greater growth and is required for advanced contracts, but carries lasting obligations." + 
                "Promoted characters command higher wages, increasing the guild’s daily expenses./n" +
                "Promotions are permanent and reflect a long-term investment.";
            //Requirements;
            
            //Level
            Label _level = container.Q<Label>("LevelText");
            _level.text = $"Required Level: {character.Level}/{rankEval.RequiredLevel}";

            //Primary
            Label _primaryStat = container.Q<Label>("PrimaryStat");
            _primaryStat.text = $"{rankEval.PrimaryStat} Required: {character.Base.GetStat(rankEval.PrimaryStat)}/{rankEval.PrimaryStatRequirement}";

            //Secondary
            Label _secondaryStat = container.Q<Label>("SecondaryStat");
            _secondaryStat.text = $"{rankEval.SecondaryStat} Required: {character.Base.GetStat(rankEval.SecondaryStat)}/{rankEval.SecondaryStatRequirement}";

            //Gold
            Label _gold = container.Q<Label>("CostToRankUp");
            _gold.text = rankEval.PromotionCost.ToString();

            //Effects
            //Rank
            Label _rank = container.Q<Label>("NewRank");
            _rank.text = rankEval.NextRank.ToString();
            //Wage
            Label _wage = container.Q<Label>("NewWage");
            _wage.text = rankEval.NextWage.ToString();
            //LevelCap
            Label _levelCap = container.Q<Label>("LevelCap");
            _levelCap.text = GameStateQueries.GetLevelCap(rankEval.NextRank).ToString();


            //conditionals
            Toggle _levelToggle = container.Q<Toggle>("LevelToggle");
            _levelToggle.value = rankEval.MeetsRequiredLevel;
            _level.style.color = rankEval.MeetsRequiredLevel ? Color.gray : Color.black;

            Toggle _primaryToggle = container.Q<Toggle>("PrimaryToggle");
            _primaryToggle.value = rankEval.MeetsRequiredPrimaryStat;
            _primaryStat.style.color = rankEval.MeetsRequiredPrimaryStat ? Color.gray : Color.black;

            Toggle _secondaryToggle = container.Q<Toggle>("SecondaryToggle");
            _secondaryToggle.value = rankEval.MeetsRequiredSecondaryStat;
            _secondaryStat.style.color = rankEval.MeetsRequiredSecondaryStat ? Color.gray : Color.black;
        }

    }
}