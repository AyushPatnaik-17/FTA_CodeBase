//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class MainGrab_Assembly_Movement : MonoBehaviour
//{
//    [SerializeField] private float minHeight, minHeightAttached, maxHeight;

//    [SerializeField] private float currentMinHeight;
//    [SerializeField] private Animator animator;

//    public bool isGrabbable = false, isTracking = false;


//    private Vector3 trackPosition = Vector3.zero;


//    //private float mainAssemblyHeightLevel = 1f;     //in percentage

//    public float speedModifier;

//    public bool reachedBottom = false, reachedTop = true, isGrabbing = false;

//    private Transform grabbableObject;


//    void Start()
//    {
//        currentMinHeight = minHeight;
//    }


//    void Update()
//    {
//        //Debug.Log($"isGrabbable = {isGrabbable}         trackPosition = {trackPosition}");
//        if (isGrabbable)
//        {
//            TrackPosition();
//        }
//        isGrabbable = CheckIfGrabbable();
//    }

//    [Header("Debug Raycast")]
//    public Transform rayOrigin;
//    public float rayDistance = 100f;
//    public LayerMask layerMask;

//    private bool CheckIfGrabbable()
//    {
//        Vector3 rayDirection = rayOrigin.up * -1;

//        if (Physics.Raycast(rayOrigin.position, rayDirection, out RaycastHit hit, rayDistance, layerMask))
//        {
//#if UNITY_EDITOR
//            //Debug.Log("Hit object: " + hit.collider.name);
//            Debug.DrawLine(rayOrigin.position, hit.point, Color.red);
//#endif

//            //Checking if grabbable object is grounded 
//            //Used to see if grabbable object can be attached or detached 
//            IGrabInteractable grabInteractable = hit.collider.transform.GetComponent<IGrabInteractable>();
//            if (grabInteractable != null)
//            {
//                if (grabInteractable.IsGrounded())
//                {
//                    if (trackPosition == Vector3.zero)
//                        trackPosition = transform.position;
//                    grabbableObject = hit.collider.transform;
//                    return true;
//                }
//                else
//                {
//                    trackPosition = Vector3.zero;
//                    return false;
//                }
//            }
//            else
//            {
//                trackPosition = Vector3.zero;
//                return false;
//            }
//        }
//        else
//        {
//#if UNITY_EDITOR
//            Debug.DrawLine(rayOrigin.position, rayOrigin.position + rayDirection * rayDistance, Color.green);
//#endif
//            trackPosition = Vector3.zero;
//            return false;
//        }
//    }

//    private void TrackPosition()
//    {
//        Vector3 localLastPosition = transform.parent.InverseTransformPoint(trackPosition);
//        localLastPosition.y = Mathf.Clamp(localLastPosition.y, currentMinHeight, maxHeight);

//        SetSpeedModifierValue(localLastPosition.y);
//        transform.localPosition = new Vector3(transform.localPosition.x, localLastPosition.y, transform.localPosition.z);
//    }

//    private void SetSpeedModifierValue(float pos)
//    {
//        speedModifier = (pos - currentMinHeight) / (maxHeight - currentMinHeight);
//        speedModifier = 1 - speedModifier;
//        CheckForInteraction();
//    }

//    private void CheckForInteraction()
//    {
//        if (speedModifier < 0.9f)
//        {
//            CheckForDetach();
//        }

//        if (speedModifier > 0.9f)
//        {
//            reachedTop = true;
//        }

//        if (speedModifier > 0.2f)
//            CheckForAttach();



//        if (speedModifier < 0.05)
//        {
//            if (reachedTop)
//            {
//                if (isGrabbing)
//                {
//                    isGrabbing = false;
//                    currentMinHeight = minHeight;
//                }
//                else
//                {
//                    isGrabbing = true;
//                    currentMinHeight = minHeightAttached;
//                }
//                reachedTop = false;
//            }
//        }
//    }

//    private void CheckForAttach()
//    {
//        if (isGrabbing && !isTracking)
//        {
//            StartCoroutine(CheckForAttachAsync());
//        }
//    }

//    private IEnumerator CheckForAttachAsync()
//    {
//        isTracking = true;
//        animator.Play("AttachAnim");
//        // Initial local position in world space
//        Vector3 initialLocalPosition = transform.InverseTransformPoint(grabbableObject.position);

//        // Target local position with x set to 0
//        Vector3 targetLocalPosition = initialLocalPosition;
//        Debug.LogError(targetLocalPosition);
//        targetLocalPosition.x = 0;

//        while (speedModifier != 1f)
//        {
//            // Calculate the interpolation factor based on the speedModifier
//            float t = Time.deltaTime * speedModifier;

//            // Gradually move the x coordinate of the local position to the target x
//            initialLocalPosition.x = Mathf.Lerp(initialLocalPosition.x, targetLocalPosition.x, t);

//            // Convert the current local position back to world space and set it
//            Vector3 currentWorldPosition = transform.TransformPoint(initialLocalPosition);
//            //grabbableObject.position = currentWorldPosition;

//            yield return null;
//        }
//        grabbableObject.SetParent(transform);
//        Vector3 pos = grabbableObject.localPosition;
//        pos.y = -0.4786f;
//        pos.x = 0;
//        grabbableObject.localPosition = pos;
//        Debug.LogError("Attached");
//        isGrabbable = false;
//        trackPosition = Vector3.zero;
//    }

//    private void CheckForDetach()
//    {
//        if (isTracking && reachedTop)
//        {
//            Debug.LogError("Called detach");
//            StartCoroutine(CheckForDetachAsync());
//        }
//    }

//    private IEnumerator CheckForDetachAsync()
//    {
//        animator.Play("DetachAnim");
//        while (speedModifier > 0.1f && isGrabbing)
//        //while (isGrabbing)
//        {
//            yield return null;
//        }
//        grabbableObject.SetParent(null);
//        isTracking = false;
//    }



//    /*    private void OnDrawGizmos()
//        {
//            // Set the gizmo color
//            Gizmos.color = Color.red;

//            // Draw a wire sphere at the transform's position with the specified radius
//            Gizmos.DrawWireSphere(lastPosition, 0.1f);

//            //Gizmos.DrawWireSphere(lastPosition, 0.1f);
//        }*/
//}