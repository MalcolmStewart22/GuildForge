using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonListView : MonoBehaviour
{
    private ListView listView;
    private List<DungeonInstance> _currentDungeons = new();
    public event System.Action<DungeonInstance> OnDungeonSelected;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        listView = root.Q<ListView>("DungeonsListView");

        listView.makeItem = () =>
        {
            var row = new VisualElement();
            row.AddToClassList("DungeonRow");

            var title = new Label { name = "Title" };
            title.AddToClassList("DungeonTitle");
            row.Add(title);

            return row;
        };

        listView.bindItem = (element, index) =>
        {
            var dungeon = _currentDungeons[index];
            element.Q<Label>("Title").text = $"{dungeon.Name}  \n (Rank {dungeon.Rank})";
        };

        listView.selectionChanged += items =>
        {
            foreach(var item in items)
            {
                var dungeon = (DungeonInstance)item;
                OnDungeonSelected(dungeon);
            }
        };
    }

    public void ShowDungeon(List<DungeonInstance> newDungeons)
    {
        _currentDungeons = newDungeons ?? new List<DungeonInstance>();
        listView.itemsSource = _currentDungeons;
        listView.Rebuild();
    }
}







