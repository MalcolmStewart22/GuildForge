using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_DungeonTemplate", menuName = "Scriptable Objects/SO_DungeonTemplate")]
public class SO_DungeonTemplate : ScriptableObject
{
    public int Id;
    public string DisplayName;
    //public DungeonRank Rank;
    public List<SO_DungeonTag> Tags;
    public int MaxReward;
}
