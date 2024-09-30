using System.Collections.Generic;
using AlligatorUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("ArmRests")]
    public ArmRest RightArmRest;
    public ArmRest LeftArmRest;

    [Header("Joysticks")]
     public Joystick RightJoystick = new Joystick("Right Joystick");
    public Joystick LeftJoystick = new Joystick("Left Joystick");
    
    [Header("Rotaries")]
    public Rotary Rotary1 = new Rotary
                            (
                                value: 0,
                                maxValue: 6,
                                minValue: 0,
                                name: "One",
                                new string[]
                                {
                                    "none",
                                    "selection anode handling",
                                    "filling",
                                    "suction",
                                    "dust discharge",
                                    "aux.hoist",
                                    "flue wall hoist"
                                }
                            );
    public Rotary Rotary2 = new Rotary
                            (
                                value: 0, 
                                maxValue: 2, 
                                minValue: 0, 
                                name: "Two",
                                new string[]
                                {
                                    "none",
                                    "selection all",
                                    "AH + flue wall hoist"
                                }
                            ); 
    public Rotary Rotary3 = new Rotary
                            (
                                value: 0, 
                                maxValue: 3, 
                                minValue: 0, 
                                name: "Three",
                                new string[]
                                {
                                    "none",
                                    "FPH 1",
                                    "FPH 2",
                                    "FPH 1+2"
                                }
                            ); 
    public Rotary Rotary4 = new Rotary
                            (
                                value: 0, 
                                maxValue: 3, 
                                minValue: 0, 
                                name: "Four",
                                new string[]
                                {
                                    "none",
                                    "selection straightener",
                                    "demolitioner",
                                    "hook"
                                }
                            ); 
    public Rotary ActiveRotary;

    [Header("Push Buttons")]
    public List<PushButton> RightPushButtons = new();
    public List<PushButton> LeftPushButtons = new();

    #region Input
    public ControllerSetup ControllerSetup;
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

    private void Awake()
    {
        ControllerSetup = new ControllerSetup();
        ControllerSetup.ControlsActions controls = ControllerSetup.Controls;

        SetupRotaries(controls);
        SetupPushButtons(controls);
        SetupJoysticks(controls);

        RightArmRest = new ArmRest(RightJoystick, RightPushButtons, new List<Rotary>() { Rotary1, Rotary2, Rotary3, Rotary4 });
        LeftArmRest = new ArmRest(LeftJoystick, LeftPushButtons);
    }
    
    private void Update()
    {
        string text = $"{RightArmRest.GetOutputString()}  {LeftArmRest.GetOutputString()}";
        UIHandler.Instance.DisplayInputText(text);
    }
    public void OnEnable() => ControllerSetup.Controls.Enable();
    public void OnDisable() => ControllerSetup.Controls.Disable();
    private int ConvertJoystickValue(float axisValue)
    {
        return axisValue switch
        {
            >= -1f and < -0.75f => 1,
            >= -0.75f and < -0.5f => 2,
            >= -0.5f and < -0.25f => 3,
            >= -0.25f and < 0f => 4,
            0f => 5,
            > 0f and <= 0.25f => 6,
            > 0.25f and <= 0.5f => 7,
            > 0.5f and <= 0.75f => 8,
            _ => 9
        };
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
           UIHandler.Instance.ChangeRotaryCountText
           (
            GetActiveRotaryNumber(),
            ActiveRotary.Value,
            ActiveRotary.SettingDetails[ActiveRotary.Value]
        );
        };

        _downshift.performed += ctx => 
        {
            ActiveRotary.RotateDown();
            UIHandler.Instance.ChangeRotaryCountText
            (
                GetActiveRotaryNumber(),
                ActiveRotary.Value,
                ActiveRotary.SettingDetails[ActiveRotary.Value]
            );
        };
    }
    private void SetActiveRotary(Rotary rotary)
    {
        ActiveRotary = rotary;
        UIHandler.Instance.SetRotaryAsActive(GetActiveRotaryNumber());
        UIHandler.Instance.ChangeRotaryCountText
        (
            GetActiveRotaryNumber(),
            ActiveRotary.Value,
            ActiveRotary.SettingDetails[ActiveRotary.Value]
        );
    }
    private int GetActiveRotaryNumber()
    {
        if (ActiveRotary == Rotary1) return 0;
        if (ActiveRotary == Rotary2) return 1;
        if (ActiveRotary == Rotary3) return 2;
        if (ActiveRotary == Rotary4) return 3;
        return -1;
    }
    
}
