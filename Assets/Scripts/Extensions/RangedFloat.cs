using System;
using UnityEngine;

public class RangedFloat
{
    public float Min { get; private set; }
    public float Max { get; private set; }
    public float Value 
    {
        get { return GetValue(); }
    }

    public RangedFloat(float min, float max)
    {
        Min = min;
        Max = max;
    }

    float GetValue()
    {
        return UnityEngine.Random.Range(Min, Max);
    }

    public override bool Equals(object obj)
    {
        if (obj == null) 
            return false;
        
        RangedFloat otherRangedFloat = (RangedFloat) obj;

        if (Min != otherRangedFloat.Min || Max != otherRangedFloat.Max)
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Min, Max, Value);
    }

    public static implicit operator float(RangedFloat rangedFloat)
    {
        return rangedFloat.Value;
    }
}
