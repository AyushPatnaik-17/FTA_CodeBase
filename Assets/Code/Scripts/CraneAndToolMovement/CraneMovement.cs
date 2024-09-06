using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetKey(KeyCode.A))
        {
            MoveCraneLT(-1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveCraneLT(1);
        }
        if (Input.GetKey(KeyCode.W))
        {
            MoveCraneCT(1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveCraneCT(-1);
        }
    }

    public void MoveCraneLT(float input)
    {
        if (Mathf.Abs(input) < 0.3f)
            return;
        crane.transform.Translate(Vector3.right * ltSpeed * Time.deltaTime * input * multiplier);
    }

    
    public void MoveCraneCT(float input)
    {
        if (Mathf.Abs(input) < 0.3f)
            return;
        cabin.transform.Translate(Vector3.forward * ctSpeed * Time.deltaTime * input * multiplier, Space.Self);
    }

}
