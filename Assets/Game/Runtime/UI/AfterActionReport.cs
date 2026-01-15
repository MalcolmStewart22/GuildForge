using UnityEngine;
using UnityEngine.UIElements;

public class AfterActionReport : MonoBehaviour
{
    [SerializeField]
     PartyListView partyListView;
    [SerializeField]
     EventActionListView eventActionListView;


    public void ShowReport(VisualElement container, MissionResult mission)
    {
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
}