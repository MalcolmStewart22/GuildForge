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
        Label _day = container.Q<Label>("DayCount");
        _day.text = mission.MissionDay.ToString();
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

    public void ShowGuildRankInfo(VisualElement container, GuildRank currentRank, GuildRankEvaluation rankEval)
    {
        container.style.display = DisplayStyle.Flex;
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

        //Requirements
        //Level
        Label _level = container.Q<Label>("LevelText");
        _level.text = $"Required Level: {character.Level.ToString()}/{rankEval.RequiredLevel.ToString()}";

        //Primary
        Label _primaryStat = container.Q<Label>("PrimaryStat");
        _primaryStat.text = $"{rankEval.PrimaryStat} Required: {character.Base.GetStat(rankEval.PrimaryStat)}/{rankEval.PrimaryStatRequirement}";

        //Secondary
        Label _secondaryStat = container.Q<Label>("SecondaryStat");
        _primaryStat.text = $"{rankEval.SecondaryStat} Required: {character.Base.GetStat(rankEval.SecondaryStat)}/{rankEval.SecondaryStatRequirement}";

        //Gold
        Label _gold = container.Q<Label>("CostToRankUp");
        _gold.text = rankEval.PromotionCost.ToString();

        //Effects
        //Rank
        Label _rank = container.Q<Label>("NewRank");
        _rank.text = rankEval.RequiredLevel.ToString();
        //Wage
        Label _wage = container.Q<Label>("NewWage");
        _wage.text = rankEval.RequiredLevel.ToString();
        //LevelCap
        Label _levelCap = container.Q<Label>("LevelCap");
        _levelCap.text = GameStateQueries.GetLevelCap(character.Rank).ToString();


        //conditionals

        if(rankEval.MeetsRequiredLevel)
        {
            Toggle _levelToggle = container.Q<Toggle>("LevelToggle");
            _levelToggle.value = true;

            _level.style.color = Color.gray;
        }
        if(rankEval.MeetsRequiredPrimaryStat)
        {
            Toggle _primaryToggle = container.Q<Toggle>("PrimaryToggle");
            _primaryToggle.value = true;

            _primaryStat.style.color = Color.gray;
        }
        if(rankEval.MeetsRequiredSecondaryStat)
        {
            Toggle _secondaryToggle = container.Q<Toggle>("SecondaryToggle");
            _secondaryToggle.value = true;

            _secondaryStat.style.color = Color.gray;
        }
    }
}