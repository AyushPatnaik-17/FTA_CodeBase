using System.Collections;
using System.Collections.Generic;
using AlligatorUtils;
using UnityEngine;

public class HoistAssitant : MonoBehaviour
{
    public Transform Hoist;
    public Transform AttachPoint;
    public float DetectionRadius = 10f;
    public float ThresholdDistance = 0.5f;

    public float Distance = 0f;
    private void Update()
    {
        Vector3 relativePosition = Hoist.position - transform.position;
        Distance = Vector3.Distance(Hoist.position,AttachPoint.position);
        if (relativePosition.magnitude <= DetectionRadius)
        {
            var hoistScript = Hoist.GetComponent<HoistBehaviour>();
            //relativePosition.magnitude.Print("relative distance:", "red");
            if(Distance <= ThresholdDistance)
            {
                hoistScript.DirectionToMove = Direction.None;
                return;
            }

            if (hoistScript != null)
            {
                hoistScript.DirectionToMove = GetProminentDirection(relativePosition);
            }
        }
    }

    private Direction GetProminentDirection(Vector3 relativePos)
    {
        if (Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.y) && Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.z) && Mathf.Abs(relativePos.x) > ThresholdDistance)
        {
            return relativePos.x > 0 ? Direction.Left : Direction.Right;
        }
        else if (Mathf.Abs(relativePos.y) > Mathf.Abs(relativePos.x) && Mathf.Abs(relativePos.y) > Mathf.Abs(relativePos.z) && Mathf.Abs(relativePos.x) > ThresholdDistance)
        {
            return relativePos.y > 0 ? Direction.Down : Direction.Up;
        }
        else if(Mathf.Abs(relativePos.z) > Mathf.Abs(relativePos.x) && Mathf.Abs(relativePos.z) > Mathf.Abs(relativePos.y) && Mathf.Abs(relativePos.y) > ThresholdDistance)
        {
            return relativePos.z > 0 ? Direction.Backward : Direction.Forward;
        }
        else
        {
            return Direction.None;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}
