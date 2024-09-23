using System.Collections;
using System.Collections.Generic;
using AlligatorUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //TODO: manipulate vlaues of push buttons, joystick and output result
    [Header("ArmRests")]
    public ArmRest RightArmRest;
    public ArmRest LeftArmRest;

    [Header("Joysticks")]
    public Joystick RightJoystick = new Joystick("Right Joystick");
    public Joystick LeftJoystick = new Joystick("Left Joystick");
    
    [Header("Rotaries")]
    public Rotary Rotary1 = new Rotary(value: 0, maxValue: 6, minValue: 0, name: "One"); 
    public Rotary Rotary2 = new Rotary(value: 0, maxValue: 2, minValue: 0, name: "Two"); 
    public Rotary Rotary3 = new Rotary(value: 0, maxValue: 3, minValue: 0, name: "Three"); 
    public Rotary Rotary4 = new Rotary(value: 0, maxValue: 3, minValue: 0, name: "Four"); 
    public Rotary ActiveRotary;

    [Header("Push Buttons")]
    public List<PushButton> RightPushButtons = new();
    public List<PushButton> LeftPushButtons = new();

    #region Input
    private ControllerSetup _controllerSetup;
    private InputAction _rotaryBtn1, _rotaryBtn2, _rotaryBtn3, _rotaryBtn4;
    private InputAction _upShift,_downshift;
    private InputAction RPb1, RPb2,RPb3,RPb4,RPb5,RPb6,RPb7,RPb8; 
    private InputAction LPb1, LPb2,LPb3,LPb4; 

    private List<InputAction> _rightPushBinds = new();
    private List<InputAction> _leftPushBinds = new();
    private InputAction _joystick1Button1, _joystick1Button2, 
                        _joystick2Button1, _joystick2Button2; 
    private InputAction _rightStick, _leftStick;
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

        SetupRotaries(controls);
        SetupPushButtons(controls);
        SetupJoysticks(controls);

        RightArmRest = new ArmRest(RightJoystick, RightPushButtons, new List<Rotary>() { Rotary1, Rotary2, Rotary3, Rotary4 });
        LeftArmRest = new ArmRest(LeftJoystick, LeftPushButtons);
    }

    private void SetupJoysticks(ControllerSetup.ControlsActions controls)
    {
        RightJoystick = new Joystick("Right Joystick");
        LeftJoystick = new Joystick("Left Joystick");

        _rightStick = controls.Joystick1;
        _leftStick = controls.Joystick2;

        _joystick1Button1 = controls.Joystick1Button1;
        _joystick1Button2 = controls.Joystick1Button2;
        _joystick2Button1 = controls.Joystick2Button1;
        _joystick2Button2 = controls.Joystick2Button2;

        _rightStick.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            RightJoystick.XVal = ConvertJoystickValue(input.x);
            RightJoystick.YVal = ConvertJoystickValue(input.y);
        };
        _leftStick.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            LeftJoystick.XVal = ConvertJoystickValue(input.x);
            LeftJoystick.YVal = ConvertJoystickValue(input.y);
        };

        _joystick1Button1.performed += ctx =>
        {
            "RB1".Print();
            RightJoystick.Button1.SetValue();
        };
        _joystick1Button2.performed += ctx =>
        {
            "RB2".Print();
            RightJoystick.Button2.SetValue();
        };
        _joystick2Button1.performed += ctx =>
        {
            "LB1".Print();
            LeftJoystick.Button1.SetValue();
        };
        _joystick2Button2.performed += ctx =>
        {
            "LB2".Print();
            LeftJoystick.Button2.SetValue();
        };


    }

    private void SetupPushButtons(ControllerSetup.ControlsActions controls)
    {
        RPb1 = controls.RightPushButton1;
        RPb2 = controls.RightPushButton2;
        RPb3 = controls.RightPushButton3;
        RPb4 = controls.RightPushButton4;
        RPb5 = controls.RightPushButton5;
        RPb6 = controls.RightPushButton6;
        RPb7 = controls.RightPushButton7;
        RPb8 = controls.RightPushButton8;

        LPb1 = controls.LeftPushButton1;
        LPb2 = controls.LeftPushButton2;
        LPb3 = controls.LeftPushButton3;
        LPb4 = controls.LeftPushButton4;

        _rightPushBinds = new List<InputAction>
        {
            RPb1, RPb2, RPb3, RPb4, RPb5, RPb6, RPb7, RPb8
        };

        _leftPushBinds = new List<InputAction>
        {
            LPb1, LPb2, LPb3, LPb4
        };

        for(int i = 0; i < _rightPushBinds.Count; i++)
        {
            int index = i;
            PushButton button = new PushButton(index+1, 0);
            RightPushButtons.Add(button);

            _rightPushBinds[index].performed += ctx => 
            {
                Debug.Log("Right Pushed");
                button.Value = 1;
            };
            _rightPushBinds[index].canceled += ctx => 
            {
                Debug.Log("Right Released");
                button.Value = 0;
            };
        }

        for(int i = 0; i < _leftPushBinds.Count; i++)
        {
            int index = i;
            PushButton button = new PushButton(index+1, 0);
            LeftPushButtons.Add(button);

            _leftPushBinds[index].performed += ctx => 
            {
                Debug.Log("Left Button Pushed");
                button.Value = 1;
            };
            _leftPushBinds[index].canceled += ctx => 
            {
                Debug.Log("Left Button Released");
                button.Value = 0;
            };
        }
    }

    private void SetupRotaries(ControllerSetup.ControlsActions controls)
    {
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

    private void Update()
    {
        Debug.Log($"{RightArmRest.GetOutputString()}  {LeftArmRest.GetOutputString()}");
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
