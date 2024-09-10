using UnityEngine;

public class HoistAssist : MonoBehaviour
{
    public Transform targetObject;
    public float detectionRadius = 5f;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, targetObject.position);
        
        if (distance <= detectionRadius)
        {
            Vector3 direction = (targetObject.position - transform.position).normalized;
            DetermineToDirection(direction);
            //MultidimensionalCheck(direction);
        }
    }

    void DetermineToDirection(Vector3 direction)
    {
        
    }

    private void MultidimensionalCheck(Vector3 direction)
    {
        if (direction.x > 0)
        {
            Debug.Log("Move Right");
        }
        else if (direction.x < 0)
        {
            Debug.Log("Move Left");
        }

        if (direction.y > 0)
        {
            Debug.Log("Move Up");
        }
        else if (direction.y < 0)
        {
            Debug.Log("Move Down");
        }

        if (direction.z > 0)
        {
            Debug.Log("Move Forward");
        }
        else if (direction.z < 0)
        {
            Debug.Log("Move Backward");
        }
    }
}