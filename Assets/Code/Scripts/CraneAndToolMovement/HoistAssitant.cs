using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoistAssitant : MonoBehaviour
{
    public Transform Hoist;
    public float DetectionRadius = 10f;

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
        if (Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.y) && Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.z))
        {
            return relativePos.x > 0 ? Direction.Right : Direction.Left;
        }
        else if (Mathf.Abs(relativePos.y) > Mathf.Abs(relativePos.x) && Mathf.Abs(relativePos.y) > Mathf.Abs(relativePos.z))
        {
            return relativePos.y > 0 ? Direction.Up : Direction.Down;
        }
        else
        {
            return relativePos.z > 0 ? Direction.Forward : Direction.Backward;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the radius for visual aid
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}
