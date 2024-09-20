using System;
[Serializable]
public class Rotary
{
    public string Name;
    public int Value;
    public int MaxValue;
    public int MinValue;

    public Rotary(int value, int maxValue, int minValue, string name)
    {
        Value = value;
        MaxValue = maxValue;
        MinValue = minValue;
        Name = name;
    }
    private void SetValue(int newValue)
    {
        Value = Math.Clamp(newValue, MinValue, MaxValue);
    }
    public void RotateUp()
    {
        SetValue(Value + 1);
    }

    public void RotateDown()
    {
        SetValue(Value - 1);
    }
}

[Serializable]
public class PushButton
{
    public string Name;
    public int Value;

    public PushButton(int index, int value)
    {
        Name = $"Push Button {index}";
        Value = value;
    }
}

[Serializable]
public class JoyStick
{
    public String Name;
    public PushButton Button1;
    public PushButton Button2;
    public int XVal,YVal;

    public JoyStick(string name)
    {
        Name = name;
        Button1 = new PushButton(1,0);
        Button2 = new PushButton(2,0);
        XVal = 5;
        YVal = 5;
    }

}
public class ArmRest
{

}