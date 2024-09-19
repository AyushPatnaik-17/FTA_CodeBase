[System.Serializable]
public struct Rotary
{
    public int Value;
    public int MaxValue;
    public int MinValue;

    public Rotary(int value, int maxValue, int minValue)
    {
        Value = value;
        MaxValue = maxValue;
        MinValue = minValue;
    }
}