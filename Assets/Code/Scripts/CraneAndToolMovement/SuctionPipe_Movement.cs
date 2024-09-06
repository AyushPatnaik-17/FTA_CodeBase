using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuctionPipe_Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [Header("Slider 1")]
    [SerializeField] private GameObject slider1;
    [SerializeField] private float slider1MinHeight;
    [SerializeField] private float slider1MaxHeight;

    [Header("Slider 2")]
    [SerializeField] private GameObject slider2;
    [SerializeField] private float slider2MinHeight;
    [SerializeField] private float slider2MaxHeight;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveTool(1);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveTool(-1);
        }
    }

    public void MoveTool(float joytstickValue)
    {
        if (joytstickValue > 0)
        {
            MoveSuctionPipeUp(Mathf.Abs(joytstickValue));
        }
        else
        {
            MoveSuctionPipeDown(Mathf.Abs(joytstickValue));
        }
    }

    private void MoveSuctionPipeUp(float joystickValue)
    {
        float moveDistance = moveSpeed * Time.deltaTime * joystickValue;
        if (slider2.transform.localPosition.y < slider2MaxHeight)
        {
            float newYPosition = Mathf.Min(slider2.transform.localPosition.y + moveDistance, slider2MaxHeight);
            slider2.transform.localPosition = new Vector3(slider2.transform.localPosition.x, newYPosition, slider2.transform.localPosition.z);
        }
        else if (slider1.transform.localPosition.y < slider1MaxHeight)
        {
            float newYPosition = Mathf.Min(slider1.transform.localPosition.y + moveDistance, slider1MaxHeight);
            slider1.transform.localPosition = new Vector3(slider1.transform.localPosition.x, newYPosition, slider1.transform.localPosition.z);
        }
    }

    private void MoveSuctionPipeDown(float joystickValue)
    {
        float moveDistance = moveSpeed * Time.deltaTime * joystickValue;

        if (slider1.transform.localPosition.y > slider1MinHeight)
        {
            float newYPosition = Mathf.Max(slider1.transform.localPosition.y - moveDistance, slider1MinHeight);
            slider1.transform.localPosition = new Vector3(slider1.transform.localPosition.x, newYPosition, slider1.transform.localPosition.z);
        }
        else if (slider2.transform.localPosition.y > slider2MinHeight)
        {
            float newYPosition = Mathf.Max(slider2.transform.localPosition.y - moveDistance, slider2MinHeight);
            slider2.transform.localPosition = new Vector3(slider2.transform.localPosition.x, newYPosition, slider2.transform.localPosition.z);
        }
    }
}
