using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TraitsListView : MonoBehaviour
{
    [SerializeField] 
    private ListView listView;
    private List<SO_Trait> traits = new();


    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        listView = root.Q<ListView>("TraitListView");

        listView.makeItem = () =>
        {
            var row = new VisualElement();
            row.AddToClassList("TraitsRow");

            var _nameLabel = new Label { name = "Name" };
            _nameLabel.AddToClassList("traits-name");

            var _rankLabel = new Label { name = "Description" };
            _rankLabel.AddToClassList("traits-description");

            row.style.justifyContent = Justify.SpaceAround;
            row.Add(_nameLabel);
            row.Add(_rankLabel);

            return row;
        };

        listView.bindItem = (element, index) =>
        {
            var trait = traits[index];
            element.Q<Label>("Name").text = trait.DisplayName;
            element.Q<Label>("Description").text = trait.TextInfo;
        
        };

    }
    public void ShowTraits(List<SO_Trait> newtraits)
    {
        traits = newtraits ?? new List<SO_Trait>();
        listView.itemsSource = traits;
        listView.Rebuild();
    }


}