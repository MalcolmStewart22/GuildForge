using UnityEngine;

[CreateAssetMenu(fileName = "SO_Trait", menuName = "Scriptable Objects/SO_Trait")]
public class SO_Trait : ScriptableObject
{
    public int id;
    public string DisplayName;
    public string TextInfo;
    public TraitsModifiers Effects; 
}
