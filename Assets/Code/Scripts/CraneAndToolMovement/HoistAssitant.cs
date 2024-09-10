using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoistAssitant : MonoBehaviour
{
    public Transform Hoist;
    public float DetectionRadius = 10f;
    public float ThresholdDistance = 0.5f;
    private void Update()
    {
        Vector3 relativePosition = Hoist.position - transform.position;

        if (relativePosition.magnitude <= DetectionRadius)
        {
            var hoistScript = Hoist.GetComponent<HoistBehaviour>();

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
