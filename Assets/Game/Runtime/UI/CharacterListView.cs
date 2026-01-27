using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterListView : MonoBehaviour
{
    [SerializeField] 
    private ListView listView;
    private List<Character> partyRoster = new();
    public event System.Action<Character> OnCharacterSelected;


    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        listView = root.Q<ListView>("CharacterListView");

        listView.makeItem = () =>
        {
            var row = new VisualElement();
            row.AddToClassList("RosterRow");

            var _nameLabel = new Label { name = "Name" };
            _nameLabel.AddToClassList("roster-name");

            var _rankLabel = new Label { name = "Rank" };
            _rankLabel.AddToClassList("roster-rank");

            var _classLabel = new Label { name = "Class" };
            _classLabel.AddToClassList("roster-class");

            var _statusLabel = new Label { name = "Status" };
            _statusLabel.AddToClassList("roster-status");

            row.style.justifyContent = Justify.SpaceAround;
            row.Add(_nameLabel);
            row.Add(_rankLabel);
            row.Add(_classLabel);
            row.Add(_statusLabel);
            return row;
        };

        listView.bindItem = (element, index) =>
        {
            var character = partyRoster[index];
            element.Q<Label>("Name").text = character.Name;
            element.Q<Label>("Rank").text = $"Rank {character.Rank}";
            element.Q<Label>("Class").text = character.Job.ToString();
            if(character.IsResting)
            {
                element.Q<Label>("Status").text ="Resting";
                element.Q<Label>("Status").style.color = Color.yellow;
            }
            else
            {
                element.Q<Label>("Status").text ="Ready";
                element.Q<Label>("Status").style.color = Color.green;
            }
        };

        listView.selectionChanged += items =>
        {
            foreach(var item in items)
            {
                var character = (Character)item;
                OnCharacterSelected(character);
            }
        };
    }
    public void ShowRoster(List<Character> newRoster)
    {
        listView.ClearSelection();
        partyRoster = newRoster ?? new List<Character>();
        listView.itemsSource = partyRoster;
        listView.Rebuild();
    }


}