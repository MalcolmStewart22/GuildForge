using UnityEngine;

[System.Serializable]
public class Outcomes
{
    public float GoldGainedModifier = 1f;
    public float DamageModifier = 1f;
    public float EXPModifier = 1f;
    public int Weight = 2;
    public int DifferenceToPrevent = 0;



    public Outcomes Clone()
    {
        return new Outcomes
        {
            GoldGainedModifier = GoldGainedModifier,
            DamageModifier = DamageModifier,
            Weight = Weight,
            DifferenceToPrevent = DifferenceToPrevent
        };
    }
}