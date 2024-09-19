using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public const string DEFAULT_INPUT = "550000000000000055000000";
    public static char[] InputValues = DEFAULT_INPUT.ToCharArray();
    public Rotary   Rotary1 = new Rotary(value: 0, maxValue: 6, minValue: 0),
                    Rotary2 = new Rotary(value: 0, maxValue: 2, minValue: 0),
                    Rotary3 = new Rotary(value: 0, maxValue: 3, minValue: 0),
                    Rotary4 = new Rotary(value: 0, maxValue: 3, minValue: 0);

    #region Input
    
    #endregion

    private int ConvertJoystickValue(float axisValue)
    {
        if (axisValue <= -0.75f)
            return 1;
        else if (axisValue <= -0.5f)
            return 2;
        else if (axisValue <= -0.25f)
            return 3;
        else if (axisValue < 0f)
            return 4;
        else if (axisValue == 0f)
            return 5;
        else if (axisValue < 0.25f)
            return 6;
        else if (axisValue < 0.5f)
            return 7;
        else if (axisValue < 0.75f)
            return 8;
        else
            return 9;
    }
}
