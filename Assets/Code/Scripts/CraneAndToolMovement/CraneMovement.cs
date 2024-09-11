using System.Collections;
using System.Collections.Generic;
using AlligatorUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

public class CraneMovement : MonoBehaviour
{
    [SerializeField] private GameObject crane;
    [SerializeField] private GameObject cabin;

    [SerializeField] private float ctSpeed;
    [SerializeField] private float ltSpeed;

    private float multiplier = 1f;

    void Start()
    {
        
    }

    private void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");
        //horInput.Print("Current Hor input: ");
        if(horInput > 0)
            MoveCraneLT(horInput);
        if(verInput >  0)
            MoveCraneCT(verInput);
        // if (Input.GetKey(KeyCode.A))
        // {
        //     MoveCraneLT(-1);
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     MoveCraneLT(1);
        // }
        // if (Input.GetKey(KeyCode.W))
        // {
        //     MoveCraneCT(1);
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     MoveCraneCT(-1);
        // }
    }

    private Vector3 _currentVelocity;
    public void MoveCraneLT(float input)
    {
        // if (Mathf.Abs(input) < 0.3f)
        //     return;
        "MoveCraneLT called".Print();
        input.Print("lt input: ");
        // crane.transform.Translate(Vector3.right * ltSpeed * Time.deltaTime * input * multiplier;
        Vector3 targetPos = crane.transform.position + (Vector3.right * ltSpeed * Time.deltaTime * input);
        crane.transform.position = Vector3.SmoothDamp
        (
            crane.transform.position,
            new Vector3(targetPos.x, crane.transform.position.y, crane.transform.position.z),
            ref _currentVelocity,
            0.5f
        );
    }

    
    public void MoveCraneCT(float input)
    {
        if (Mathf.Abs(input) < 0.3f)
            return;
        cabin.transform.Translate(Vector3.forward * ctSpeed * Time.deltaTime * input * multiplier, Space.Self);
    }

}
