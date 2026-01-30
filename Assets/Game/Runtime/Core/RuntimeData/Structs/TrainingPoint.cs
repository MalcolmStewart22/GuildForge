using UnityEngine;

public class TrainingPoint
{
    public float Combat; //increase combat stat
    public float Survival; //increase survival stat
    public float Conditioning; //Increase Endurance
    public float Study; //Increase Primary
    public float Medicine; // Increase healing
    public float Reflection; // increase Resolve


    public void AddNewTrainingPoints(TrainingPoint training)
    {
        Combat += training.Combat;
        Survival += training.Survival;
        Conditioning += training.Conditioning;
        Study += training.Study;
        Medicine += training.Medicine;
        Reflection += training.Reflection;
    }
    public TrainingPoint(PartyTraining training)
    {
        Combat = 1 - training.CombatvsSurvival;
        Survival = training.CombatvsSurvival;
        Conditioning = 1 - training.ConditioningvsStudy;
        Study = training.ConditioningvsStudy;
        Medicine = 1 - training.MedicinevsReflection;
        Reflection = training.MedicinevsReflection;
    }
    public TrainingPoint()
    {
        Combat =  0;
        Survival = 0;
        Conditioning = 0;
        Study = 0;
        Medicine = 0;
        Reflection = 0;
    }
    public void Reset()
    {
        Combat =  0;
        Survival = 0;
        Conditioning = 0;
        Study = 0;
        Medicine = 0;
        Reflection = 0;
    }
}