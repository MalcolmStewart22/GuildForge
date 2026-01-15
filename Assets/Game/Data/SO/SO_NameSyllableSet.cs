using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_NameSyllableSet", menuName = "Scriptable Objects/SO_NameSyllableSet")]
public class SO_NameSyllableSet : ScriptableObject
{
    public List<string> PrefixSyllable;
    public List<string> MiddleSyllable;
    public List<string> SuffixSyllable;

}
