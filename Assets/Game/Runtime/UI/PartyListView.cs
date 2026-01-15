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
            element.Q<Label>("CurrentHP").text = c.Character.CurrentHP.ToString() + "/" + c.Character.HPMax.ToString();
            if(c.Character.CurrentHP < c.Character.HPMax* .3f)
            {
                element.Q<Label>("CurrentHP").style.color = Color.red;
            }
            else
            {
                element.Q<Label>("CurrentHP").style.color = Color.green;
            }
            if(c.LeveledUp)
            {
                element.Q<Label>("LeveldUp").text = "LEVELED UP!";
                element.Q<Label>("LeveldUp").style.color = Color.green;
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