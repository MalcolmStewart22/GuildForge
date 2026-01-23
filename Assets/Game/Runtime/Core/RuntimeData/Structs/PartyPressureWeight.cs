using UnityEditor.EditorTools;
using UnityEngine;

[System.Serializable]
public class PartyPressureWeight
{
    
    [Range(.5f,2)]
    public float HPPressureModifier;
    [Range(.5f,2)]
    public float SpikePressureModifier;
    [Range(.5f,2)]
    public float DeathPressureModifier;
    [Range(.5f,2)]
    public float LootPressureModifier;

}