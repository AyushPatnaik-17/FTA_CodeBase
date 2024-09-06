using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FillingPipe_Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    [Header("First Layer")]
    [SerializeField] private GameObject slider1;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private float maxValueForBlendShape;
    [SerializeField] private float slider1MaxHeight;
    [SerializeField] private float slider1MinHeight;

    private float slider1TotalDistance = 0;

    [Header("Second Layer")]
    [SerializeField] private GameObject slider2;
    [SerializeField] private float slider2MaxHeight;
    [SerializeField] private float slider2MinHeight;

    [Header("Third Layer")]
    [SerializeField] private GameObject slider3;
    [SerializeField] private float slider3MaxHeight;
    [SerializeField] private float slider3MinHeight;

    [Header("Fourth Layer")]
    [SerializeField] private GameObject slider4;
    [SerializeField] private float slider4MaxHeight;
    [SerializeField] private float slider4MinHeight;


    // Start is called before the first frame update
    void Start()
    {
        slider1TotalDistance = slider1MaxHeight - slider1MinHeight;
    }

    private void Update()
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

    public void MoveTool(float joystickValue)
    {
        if (joystickValue > 0)
        {
            MoveFillingPipeUp(Mathf.Abs(joystickValue));
        }
        else
        {
            MoveFillingPipeDown(Mathf.Abs(joystickValue));
        }
    }

    private void MoveFillingPipeDown(float joystickValue)
    {
        float moveDistance = moveSpeed * Time.deltaTime * joystickValue;

        if (slider1.transform.localPosition.y > slider1MinHeight)
        {
            float newYPosition = Mathf.Max(slider1.transform.localPosition.y - moveDistance, slider1MinHeight);
            slider1.transform.localPosition = new Vector3(slider1.transform.localPosition.x, newYPosition, slider1.transform.localPosition.z);

            float blendshapeValue = ((slider1MaxHeight - newYPosition) / slider1TotalDistance) * maxValueForBlendShape;
            skinnedMeshRenderer.SetBlendShapeWeight(0, blendshapeValue);
        }
        else if (slider2.transform.localPosition.y > slider2MinHeight)
        {
            float newYPosition = Mathf.Max(slider2.transform.localPosition.y - moveDistance, slider2MinHeight);
            slider2.transform.localPosition = new Vector3(slider2.transform.localPosition.x, newYPosition, slider2.transform.localPosition.z);
        }
        else if (slider3.transform.localPosition.y > slider3MinHeight)
        {
            float newYPosition = Mathf.Max(slider3.transform.localPosition.y - moveDistance, slider3MinHeight);
            slider3.transform.localPosition = new Vector3(slider3.transform.localPosition.x, newYPosition, slider3.transform.localPosition.z);
        }
        else if (slider4.transform.localPosition.y > slider4MinHeight)
        {
            float newYPosition = Mathf.Max(slider4.transform.localPosition.y - moveDistance, slider4MinHeight);
            slider4.transform.localPosition = new Vector3(slider4.transform.localPosition.x, newYPosition, slider4.transform.localPosition.z);
        }
    }

    private void MoveFillingPipeUp(float joystickValue)
    {
        float moveDistance = moveSpeed * Time.deltaTime * joystickValue;

        if (slider4.transform.localPosition.y < slider4MaxHeight)
        {
            float newYPosition = Mathf.Min(slider4.transform.localPosition.y + moveDistance, slider4MaxHeight);
            slider4.transform.localPosition = new Vector3(slider4.transform.localPosition.x, newYPosition, slider4.transform.localPosition.z);
        }
        else if (slider3.transform.localPosition.y < slider3MaxHeight)
        {
            float newYPosition = Mathf.Min(slider3.transform.localPosition.y + moveDistance, slider3MaxHeight);
            slider3.transform.localPosition = new Vector3(slider3.transform.localPosition.x, newYPosition, slider3.transform.localPosition.z);
        }
        else if (slider2.transform.localPosition.y < slider2MaxHeight)
        {
            float newYPosition = Mathf.Min(slider2.transform.localPosition.y + moveDistance, slider2MaxHeight);
            slider2.transform.localPosition = new Vector3(slider2.transform.localPosition.x, newYPosition, slider2.transform.localPosition.z);
        }
        else if (slider1.transform.localPosition.y < slider1MaxHeight)
        {
            float newYPosition = Mathf.Min(slider1.transform.localPosition.y + moveDistance, slider1MaxHeight);
            slider1.transform.localPosition = new Vector3(slider1.transform.localPosition.x, newYPosition, slider1.transform.localPosition.z);

            float blendshapeValue = ((slider1MaxHeight - newYPosition) / slider1TotalDistance) * maxValueForBlendShape;
            skinnedMeshRenderer.SetBlendShapeWeight(0, blendshapeValue);
        }
    }
}
