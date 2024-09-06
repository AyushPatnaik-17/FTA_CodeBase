//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MainGrab_Movement : MonoBehaviour
//{
//    [SerializeField] private float moveSpeed;

//    [Header("Slider 1")]
//    [SerializeField] private GameObject slider1;
//    [SerializeField] private float slider1MinHeight;
//    [SerializeField] private float slider1MaxHeight;

//    [Header("Slider 2")]
//    [SerializeField] private GameObject slider2;
//    [SerializeField] private float slider2MinHeight;
//    [SerializeField] private float slider2MaxHeight;


//    private MainGrab_Assembly_Movement mainGrabAssemblyMovement;

//    private bool isGrabbing, isLocked = false;


//    private void Awake()
//    {
//        mainGrabAssemblyMovement = GetComponentInChildren<MainGrab_Assembly_Movement>();
//    }

//    void Start()
//    {

//    }

//    private void Update()
//    {
//        if (Input.GetKey(KeyCode.UpArrow))
//        {
//            MoveTool(1);
//        }
//        if (Input.GetKey(KeyCode.DownArrow))
//        {
//            MoveTool(-1);
//        }
//    }


//    public void MoveTool(float joytstickValue)
//    {
//        if (joytstickValue > 0)
//        {
//            MoveMainGrabUp(Mathf.Abs(joytstickValue));
//        }
//        else
//        {
//            MoveMainGrabDown(Mathf.Abs(joytstickValue));
//        }
//    }

//    public void MoveMainGrabDown(float joystickValue)
//    {
//        float moveDistance = moveSpeed * Time.deltaTime * joystickValue * mainGrabAssemblyMovement.speedModifier;

//        if (slider1.transform.localPosition.y > slider1MinHeight)
//        {
//            float newYPosition = Mathf.Max(slider1.transform.localPosition.y - moveDistance, slider1MinHeight);
//            slider1.transform.localPosition = new Vector3(slider1.transform.localPosition.x, newYPosition, slider1.transform.localPosition.z);
//        }
//        else if (slider2.transform.localPosition.y > slider2MinHeight)
//        {
//            float newYPosition = Mathf.Max(slider2.transform.localPosition.y - moveDistance, slider2MinHeight);
//            slider2.transform.localPosition = new Vector3(slider2.transform.localPosition.x, newYPosition, slider2.transform.localPosition.z);
//        }
//    }


//    public void MoveMainGrabUp(float joystickValue)
//    {
//        float moveDistance = 0f;
//        if(mainGrabAssemblyMovement.speedModifier != 1)
//        {
//            moveDistance = moveSpeed * Time.deltaTime * joystickValue * (0.1f);     //Alternating speed while lifting 
//        }
//        else
//        {
//            moveDistance = moveSpeed * Time.deltaTime * joystickValue * mainGrabAssemblyMovement.speedModifier;
//        }
//        if (slider2.transform.localPosition.y < slider2MaxHeight)
//        {
//            float newYPosition = Mathf.Min(slider2.transform.localPosition.y + moveDistance, slider2MaxHeight);
//            slider2.transform.localPosition = new Vector3(slider2.transform.localPosition.x, newYPosition, slider2.transform.localPosition.z);
//        }
//        else if (slider1.transform.localPosition.y < slider1MaxHeight)
//        {
//            float newYPosition = Mathf.Min(slider1.transform.localPosition.y + moveDistance, slider1MaxHeight);
//            slider1.transform.localPosition = new Vector3(slider1.transform.localPosition.x, newYPosition, slider1.transform.localPosition.z);
//        }
//    }
//}
 