using Unity.Multiplayer.Center.Common;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_DungeonTag", menuName = "Scriptable Objects/SO_DungeonTag")]
public class SO_DungeonTag : ScriptableObject
{
    [SerializeField, HideInInspector]
    private string id;
    public string ID => id;
    public string Name;
    public TagType Category;
    public int Weight = 1;
    [TextArea]
    public string Tooltip;
    public SO_DungeonTag DefaultEnemy; //Only for Biome tags
    public DungeonModifiers Effects;

    void OnValidate()
    {
        if (Category != TagType.Biome)
        {
            DefaultEnemy = null;
        }
        else
        {
            if (DefaultEnemy == null)
            {
                Debug.LogWarning("Biome must have a Default Enemy!", this);
            }
            else if (DefaultEnemy.Category != TagType.Enemy)
            {
                Debug.LogWarning("Selected Tag for Default Enemy must be a Enemy TagType.", DefaultEnemy);
                DefaultEnemy = null;
            }
        }
        

        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
    }

}
