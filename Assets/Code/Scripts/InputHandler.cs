using System.Collections;
using System.Collections.Generic;
using AlligatorUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //TODO: Define all the input actions.
    // Create an int array 
    public const string DEFAULT_INPUT = "550000000000000055000000";
    public static char[] InputValues = DEFAULT_INPUT.ToCharArray();

    [Header("Rotaries")]
    public Rotary Rotary1 = new Rotary(value: 0, maxValue: 6, minValue: 0, name: "One"); 
    public Rotary Rotary2 = new Rotary(value: 0, maxValue: 2, minValue: 0, name: "Two"); 
    public Rotary Rotary3 = new Rotary(value: 0, maxValue: 3, minValue: 0, name: "Three"); 
    public Rotary Rotary4 = new Rotary(value: 0, maxValue: 3, minValue: 0, name: "Four"); 
    public Rotary ActiveRotary;

    [Header("Press Buttons")]
    public List<PushButton> RightPushButtons = new();
    public List<PushButton> LeftPushButtons = new();

    #region Input
    private ControllerSetup _controllerSetup;
    private InputAction _rotaryBtn1, _rotaryBtn2, _rotaryBtn3, _rotaryBtn4;
    private InputAction _upShift,_downshift;
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

    private void Awake()
    {
        _controllerSetup = new ControllerSetup();
        ControllerSetup.ControlsActions controls = _controllerSetup.Controls;
        SetActiveRotary(Rotary1);

        _rotaryBtn1 = controls.Rotary1;
        _rotaryBtn2 = controls.Rotary2;
        _rotaryBtn3 = controls.Rotary3;
        _rotaryBtn4 = controls.Rotary4;

        _upShift = controls.UpShift;
        _downshift = controls.DownShift;

        _rotaryBtn1.performed += ctx => SetActiveRotary(Rotary1);
        _rotaryBtn2.performed += ctx => SetActiveRotary(Rotary2);
        _rotaryBtn3.performed += ctx => SetActiveRotary(Rotary3);
        _rotaryBtn4.performed += ctx => SetActiveRotary(Rotary4);

        _upShift.performed += ctx => 
        {
           ActiveRotary.RotateUp();
        };

        _downshift.performed += ctx => 
        {
            ActiveRotary.RotateDown();
        };
    }

    private void SetActiveRotary(Rotary rotary)
    {
        ActiveRotary = rotary;
    }

    public void OnEnable()
    {
        _controllerSetup.Controls.Enable();
    }
    public void OnDisable()
    {
        _controllerSetup.Controls.Disable();
    }
}
