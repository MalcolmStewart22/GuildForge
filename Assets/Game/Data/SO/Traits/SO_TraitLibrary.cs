using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TraitLibrary", menuName = "Scriptable Objects/SO_TraitLibrary")]
public class SO_TraitLibrary : ScriptableObject
{
    public List<SO_Trait> AllTraits = new();
}
