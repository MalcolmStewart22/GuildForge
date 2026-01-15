using UnityEngine;

//not who will take damage but who can
public enum DamageProfile
{
    TankFocused,
    Everyone, //hazards, area traps
    DamageFocused,
    SupportFocused,
    Caught, //damage to who set it off - floor panel spike trap etc
}