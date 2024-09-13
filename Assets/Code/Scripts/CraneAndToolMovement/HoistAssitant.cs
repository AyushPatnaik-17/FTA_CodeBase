using System;
using System.Collections;
using System.Collections.Generic;
using AlligatorUtils;
using UnityEngine;

public class HoistAssitant : MonoBehaviour
{
    public Transform Hoist, AttachPoint;
    public float DetectionRadius = 10f, ThresholdDistance = 0.5f;
    public HingeJoint Joint;
    public event Action OnHoistAttached;
    public event Action<Direction> OnDirectionChanged;

    [SerializeField]
    private float _attachTime = 0f;
    private Coroutine _proximityCoroutine;
    private bool _isMonitoring = false;
    private HoistBehaviour _hoistBehaviour;

    private void Start()
    {
        OnHoistAttached += HandleHoistAttach;
        OnDirectionChanged += HandleDirectionChange;

        _hoistBehaviour = Hoist.GetComponent<HoistBehaviour>();
        _hoistBehaviour.OnHoistMoved += StartProximityCheck;
    }

    private void StartProximityCheck()
    {
        if (!_isMonitoring)
        {
            _proximityCoroutine = StartCoroutine(MonitorProximity());
            _isMonitoring = true;
        }
    }

    private IEnumerator MonitorProximity()
    {
        while (true)
        {
            Vector3 relativePosition = Hoist.position - transform.position;
            float distance = Vector3.Distance(Hoist.position, AttachPoint.position);

            if (relativePosition.magnitude <= DetectionRadius)
            {
                if (distance <= ThresholdDistance && Joint.connectedBody == null)
                {
                    OnHoistAttached?.Invoke();
                }
                else
                {
                    Direction newDirection = GetProminentDirection(relativePosition);
                    OnDirectionChanged?.Invoke(newDirection);
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    // private void Update()
    // {
    //     Vector3 relativePosition = Hoist.position - transform.position;
    //     _distance = Vector3.Distance(Hoist.position,AttachPoint.position);
    //     if (relativePosition.magnitude <= DetectionRadius)
    //     {
    //         var hoistScript = Hoist.GetComponent<HoistBehaviour>();
    //         if(_distance <= ThresholdDistance)
    //         {
    //             if(Hoist != null && Joint.connectedBody == null)
    //             {
    //                 "Joint Attached".Print("","red");
    //                 Hoist.position = AttachPoint.position;
    //                 Joint.connectedBody = Hoist.GetComponent<Rigidbody>();
    //             }
    //             hoistScript.DirectionToMove = Direction.None;
    //             return;
    //         }

    //         if (hoistScript != null)
    //         {
    //             hoistScript.DirectionToMove = GetProminentDirection(relativePosition);
    //         }
    //     }
    // }

    private Direction GetProminentDirection(Vector3 relativePos)
    {
        if (Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.y) && Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.z) && Mathf.Abs(relativePos.x) > ThresholdDistance)
        {
            return relativePos.x > 0 ? Direction.Left : Direction.Right;
        }
        else if (Mathf.Abs(relativePos.y) > Mathf.Abs(relativePos.x) && Mathf.Abs(relativePos.y) > Mathf.Abs(relativePos.z) && Mathf.Abs(relativePos.y) > ThresholdDistance)
        {
            return relativePos.y > 0 ? Direction.Down : Direction.Up;
        }
        else if(Mathf.Abs(relativePos.z) > Mathf.Abs(relativePos.x) && Mathf.Abs(relativePos.z) > Mathf.Abs(relativePos.y) && Mathf.Abs(relativePos.z) > ThresholdDistance)
        {
            return relativePos.z > 0 ? Direction.Backward : Direction.Forward;
        }
        else
        {
            return Direction.None;
        }
    }

     private void HandleHoistAttach()
    {
        "Joint Attaching".Print("", "red");
        //Hoist.position = AttachPoint.position;
        StartCoroutine(AttachHoist());
    }

    private IEnumerator AttachHoist()
    {
        float elapsedTime = 0f;
        Vector3 startPos = Hoist.position;
        while (elapsedTime < _attachTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime/_attachTime;
            Hoist.position = Vector3.Lerp(startPos, AttachPoint.position, lerpFactor);
            yield return null;
        }
        Hoist.position = AttachPoint.position;
        Joint.connectedBody = Hoist.GetComponent<Rigidbody>();
        "Joint Attached".Print("", "red");

    }

    private void HandleDirectionChange(Direction newDirection)
    {
        var hoistScript = Hoist.GetComponent<HoistBehaviour>();
        if (hoistScript != null)
        {
            hoistScript.DirectionToMove = newDirection;
        }
    }

    private void OnDestroy()
    {
        if (_proximityCoroutine != null)
        {
            StopCoroutine(_proximityCoroutine);
        }

        OnHoistAttached -= HandleHoistAttach;
        OnDirectionChanged -= HandleDirectionChange;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}
