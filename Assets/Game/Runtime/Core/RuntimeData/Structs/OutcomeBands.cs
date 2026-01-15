using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class OutcomeBands
{
    public Outcomes Triumph;
    public Outcomes Success;
    public Outcomes Fled;
    public Outcomes Failure;
    public Outcomes Catastrophe;



    public OutcomeBands Clone()
    {
        return new OutcomeBands
        {
            Triumph = Triumph.Clone(),
            Success = Success.Clone(),
            Fled = Fled.Clone(),
            Failure = Failure.Clone(),
            Catastrophe = Catastrophe.Clone()
        };
    }

    public int GetWeightTotal ()
    {
        int total = Triumph.Weight;
        total += Success.Weight;
        total += Fled.Weight;
        total += Failure.Weight;
        total += Catastrophe.Weight;
        return total;
    }
}