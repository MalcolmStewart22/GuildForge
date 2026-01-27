using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PartyListView : MonoBehaviour
{
    [SerializeField] 
    private ListView listView;
    private List<Party> roster = new();
    public event System.Action<Party> OnPartySelected;


    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        listView = root.Q<ListView>("GuildRosterListView");

        listView.makeItem = () =>
        {
            var row = new VisualElement();
            row.AddToClassList("RosterRow");

            var _nameLabel = new Label { name = "Name" };
            _nameLabel.AddToClassList("roster-name");
            _nameLabel.style.fontSize = 20;

            var _statusLabel = new Label { name = "Status" };
            _statusLabel.AddToClassList("roster-status");

            row.style.justifyContent = Justify.SpaceAround;
            row.Add(_nameLabel);
            row.Add(_statusLabel);
            return row;
        };

        listView.bindItem = (element, index) =>
        {
            var _party = roster[index];
            element.Q<Label>("Name").text = _party.PartyName;
            element.Q<Label>("Status").text = _party.AssignedAction.ToString();
        };

        listView.selectionChanged += items =>
        {
            foreach(var item in items)
            {
                var _party = (Party)item;
                OnPartySelected(_party);
            }
        };
    }
    public void ShowRoster(List<Party> newRoster)
    {
        listView.ClearSelection();
        roster = newRoster ?? new List<Party>();
        listView.itemsSource = roster;
        listView.Rebuild();
    }


}