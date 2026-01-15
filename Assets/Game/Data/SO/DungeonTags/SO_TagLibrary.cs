using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TagLibrary", menuName = "Scriptable Objects/SO_TagLibrary")]
public class SO_TagLibrary : ScriptableObject
{
    public List<SO_DungeonTag> BiomeTags = new();
    public List<SO_DungeonTag> LocationTags = new();
    public List<SO_DungeonTag> EnemyTags = new();
    public List<SO_DungeonTag> AllTags = new();

}
