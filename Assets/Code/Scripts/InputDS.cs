using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
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
public class Joystick
{
    public String Name;
    public PushButton Button1;
    public PushButton Button2;
    public int XVal,YVal;

    public Joystick(string name)
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
    private string _outputString = "";
    public Joystick Joystick;
    public List<PushButton> PushButtons;

    public List<Rotary> Rotaries;

    public ArmRest(Joystick joystick, List<PushButton> pushButtons, List<Rotary> rotaries = null)
    {
        Joystick = joystick;
        PushButtons = pushButtons;
        Rotaries = rotaries;
    }

    public string GetOutputString()
    {
        string joystickAxisVal = Joystick.XVal.ToString() + Joystick.YVal.ToString();
        string pushButtonVal = "";
        foreach(var button in PushButtons)
        {
            pushButtonVal += button.Value.ToString();
        }
        _outputString = joystickAxisVal + pushButtonVal;

        if (Rotaries == null)
            return _outputString;
        foreach(var rotary in Rotaries)
        {
            _outputString += rotary.Value;
        }
        return _outputString;
    }
}