//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class FillingPipe_FillingManager : MonoBehaviour
//{
//    [SerializeField] private Transform rayOrigin;
//    [SerializeField] private float raycastDistance = 10f;
//    [SerializeField] private LayerMask layerMask;

//    private Vector3 rayDirection = Vector3.down;

//    private IPitInteractable previousInteractable = null;
//    private IPitInteractable currentInteractable = null;

//    void Update()
//    {
//        //Condition for dispersiing coke material
//        if (Input.GetKey(KeyCode.Return))
//        {
//            //Condition for checking if any gameobject is hit with raycast or not
//            if (Physics.Raycast(rayOrigin.position, rayDirection, out RaycastHit hit, raycastDistance, layerMask))
//            {
//#if UNITY_EDITOR
//                Debug.DrawRay(rayOrigin.position, rayDirection * raycastDistance, Color.green);
//                //Debug.Log($"Hit: {hit.collider.name}    Hit point: {hit.point}     Hit normal:  {hit.normal}   Hit distance:  {hit.distance} ");
//#endif
//                currentInteractable = hit.collider.gameObject.GetComponent<IPitInteractable>();

//                //Condition to check if hit object is pit interactable or not 
//                if (!currentInteractable.IsUnityNull())
//                {
//                    //Condition to check if pit interactable was changed, if yes we'd want to reset the status of the last pit interactable
//                    if(currentInteractable == previousInteractable)
//                    {
//                        //Condition to check if filling is happening correctly or not according to the pit interactable
//                        if (currentInteractable.Fill(hit))
//                        {
//#if UNITY_EDITOR
//                            //Debug.Log("Filling correctly");
//#endif
//                        }
//                        else
//                        {
//#if UNITY_EDITOR
//                            Debug.LogError("Filling incorrectly!");
//#endif
//                        }
//                    }
//                    else
//                    {
//                        if(!previousInteractable.IsUnityNull())
//                            previousInteractable.ResetStatus();
//                        previousInteractable = currentInteractable;
//                    }
//                }
//                else
//                {
//#if UNITY_EDITOR
//                    Debug.DrawRay(rayOrigin.position, rayDirection * raycastDistance, Color.red);
//                    Debug.LogError("No pit interactable detected!");
//#endif
//                }
//            }
//            else
//            {
//#if UNITY_EDITOR
//                Debug.DrawRay(rayOrigin.position, rayDirection * raycastDistance, Color.red);
//                Debug.LogError("Incorrect release of coke!");
//#endif
//            }
//        }
//        else
//        {
//#if UNITY_EDITOR
//            Debug.DrawRay(rayOrigin.position, rayDirection * raycastDistance, Color.blue);
//#endif
//        }
//    }
//}
