using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_PartyNameSet", menuName = "Scriptable Objects/SO_PartyNameSet")]
public class SO_PartyNameSet : ScriptableObject
{
    public List<string> AdjectiveList;
    public List<string> NounList;
}
