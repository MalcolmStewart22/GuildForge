using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EventActionListView : MonoBehaviour
{
    private ListView listView;
    private List<EventResult> currentEvents = new();
    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        listView = root.Q<ListView>("EventsListView");

        listView.makeItem = () =>
        {
            var row = new VisualElement();
            row.AddToClassList("EventRow");

            var _title = new Label { name = "Title" };
            _title.AddToClassList("EventTitle");
            _title.style.unityFontStyleAndWeight = FontStyle.Bold;

            var _outcome = new Label { name = "Outcome" };
            _outcome.AddToClassList("EventOutcome");
            _outcome.style.unityFontStyleAndWeight = FontStyle.Bold;
           

            row.style.justifyContent = Justify.SpaceAround;
            row.style.flexDirection = FlexDirection.Row;
            row.Add(_title);
            row.Add(_outcome);

            return row;
        };

        listView.bindItem = (element, index) =>
        {
            var e = currentEvents[index];
            element.Q<Label>("Title").text = e.DungeonEvent.Name;
            element.Q<Label>("Outcome").text = e.Outcome.ToString();
            switch (e.Outcome)
            {
                case OutcomeTypes.Triumph:
                     element.Q<Label>("Outcome").style.color = Color.green;
                    break;
                case OutcomeTypes.Success:
                     element.Q<Label>("Outcome").style.color = Color.yellow;
                    break;
                case OutcomeTypes.Failure:
                     element.Q<Label>("Outcome").style.color = Color.orange;
                    break;
                case OutcomeTypes.Catastrophe:
                     element.Q<Label>("Outcome").style.color = Color.red;
                    break;
            }
        };

    }

    public void ShowEvents(List<EventResult> newEvents)
    {
        currentEvents = newEvents ?? new List<EventResult>();
        listView.itemsSource = currentEvents;
        listView.Rebuild();
    }
}