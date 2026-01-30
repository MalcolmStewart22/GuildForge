using UnityEngine;

public class PartyTraining
{
    //We are taking the training points and using the float as a percent to split them. 
    //The word on the right of the vs gets the percentage dictated by the float
    [Range(0,1)]
    public float CombatvsSurvival = .5f; //Combat Drills vs Trap Drills
    [Range(0,1)]
    public float ConditioningvsStudy = .5f;//Physical Training vs Mental Training
    [Range(0,1)]
    public float MedicinevsReflection = .5f; //Physical Healing vs Mental Healing
}