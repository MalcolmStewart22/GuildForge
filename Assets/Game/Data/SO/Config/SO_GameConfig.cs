using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameConfig", menuName = "Scriptable Objects/SO_GameConfig")]
public class SO_GameConfig : ScriptableObject
{
    public int NumberOfCharacters = 4;
    public int NumberOfDungeons = 10;
    public bool MissionCapActive = false;
    public int MissionCap = 10;
    public int startingGold = 20;
    public int wagePerCharacter = 2;
    public float HPRefusalThreshold = 0.3f;
    public float HPReadyForActionThreshold = 0.7f;
    public float BaseRestHeal = .2f;
    public float retreatHpThreshold = 0;
    public SO_NameSyllableSet SyllableSet;
    public OutcomeBands OutcomeOptions;  
}
