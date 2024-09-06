using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneLocationManager : MonoBehaviour
{
    [SerializeField] private GameObject craneLT, craneCT;

    private int currentLTPos;
    private int currentCTPos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosLT();
        CheckPosCT();
    }

    private void CheckPosCT()
    {
        
    }

    private void CheckPosLT()
    {
        currentLTPos = ((int)(craneLT.transform.localPosition.x / 5.56f) * -1) + 1;
        Debug.Log(currentLTPos);
    }
}
