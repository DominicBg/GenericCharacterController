
[System.Serializable]
public struct ClampedFloat
{
    public float value;
    public float min;
    public float max;

    public static ClampedFloat operator +(ClampedFloat clampedFloat, float addedValue)
    {
        clampedFloat.value += addedValue;
        clampedFloat.Clamp();
        return new ClampedFloat(clampedFloat.value, clampedFloat.min, clampedFloat.max);
    }

    public static ClampedFloat operator -(ClampedFloat clampedFloat, float subValue)
    {
        clampedFloat.value -= subValue;
        clampedFloat.Clamp();
        return new ClampedFloat(clampedFloat.value, clampedFloat.min, clampedFloat.max);
    }

    public void Add(float addedValue)
    {
        value += addedValue;
        Clamp();
    }

    public ClampedFloat(float value, float min, float max)
    {
        this.value = value;
        this.min = min;
        this.max = max;
    }

    public void SetValue(float value)
    {
        this.value = value;
        Clamp();
    }

    void Clamp()
    {
        if (value < min)
            value = min;
        else if (value > max)
            value = max;
    }
}