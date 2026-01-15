using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Event", menuName = "Scriptable Objects/SO_Event")]
public class SO_Event : ScriptableObject
{
    public string Name;
    public int Weight = 1;
    public EventType EventType;  
    public int PotentialGold = 1;
    public DamageProfile damageProfile;
    public int PotentialDamage;
    public bool CanZeroGold;
    public int PotentialExp;

    //traitChangeOptions (list, optional)
}
