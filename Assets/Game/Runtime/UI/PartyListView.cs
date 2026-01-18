using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PartyListView : MonoBehaviour
{
    private ListView listView;
    private List<LevelUpReport> currentLevelUps = new();
    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        listView = root.Q<ListView>("PartyListView");

        listView.makeItem = () =>
        {
            var row = new VisualElement();
            row.AddToClassList("PartyRow");

            var _title = new Label { name = "Name" };
            _title.AddToClassList("CharacterName");

            var _currentHP = new Label { name = "CurrentHP" };
            _currentHP.AddToClassList("CurrentHP");

            var _LeveledUP = new Label { name = "LeveldUp" };
            _LeveledUP.AddToClassList("LeveldUp");

            row.style.justifyContent = Justify.SpaceAround;
            row.style.flexDirection = FlexDirection.Row;
            row.Add(_title);
            row.Add(_currentHP);
            row.Add(_LeveledUP);

            return row;
        };

        listView.bindItem = (element, index) =>
        {
            var c = currentLevelUps[index];
            element.Q<Label>("Name").text = c.Character.Name;
            if(c.Character.IsResting)
            {
                element.Q<Label>("CurrentHP").text = "Current HP: " + c.Character.CurrentHP.ToString() + "/" + c.Character.HPMax.ToString() + " (INJURED)";

            }
            else
            {
                element.Q<Label>("CurrentHP").text = "Current HP: " + c.Character.CurrentHP.ToString() + "/" + c.Character.HPMax.ToString();

            }
            
            if(c.LeveledUp)
            {
                element.Q<Label>("LeveldUp").text = "LEVELED UP! Reached level " + c.Character.Level;
            }
            else if(c.Character.Level == GameStateQueries.GetLevelCap(c.Character.Rank))
            {
                if(c.LeveledUp)
                {
                    element.Q<Label>("LeveldUp").text = "LEVELED UP! Now at Max Level!";
                }
                element.Q<Label>("LeveldUp").text = "AT MAAX LEVEL!";
            }
            else
            {
                element.Q<Label>("LeveldUp").text = "Current EXP: " + c.Character.EXP.ToString() + "/100";
            }
            
        };

    }

    public void ShowParty(List<LevelUpReport> newLevelUps)
    {
        currentLevelUps = newLevelUps ?? new List<LevelUpReport>();
        listView.itemsSource = currentLevelUps;
        listView.Rebuild();
    }
}