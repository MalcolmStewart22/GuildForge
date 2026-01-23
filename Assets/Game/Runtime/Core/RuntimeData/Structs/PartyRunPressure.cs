using UnityEngine;

[System.Serializable]
public class PartyRunPressure
{
    public float HPPressure;
    public float SpikePressure;
    public float DeathPressure;
    public float LootPressure;

    public void ResetPressure()
    {
        HPPressure = 0;
        SpikePressure = 0;
        DeathPressure = 0;
        LootPressure = 0;
    }
}