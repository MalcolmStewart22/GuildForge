using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_EventLibrary", menuName = "Scriptable Objects/SO_EventLibrary")]
public class SO_EventLibrary : ScriptableObject
{
    public List<SO_Event> AllEvents = new();

}
