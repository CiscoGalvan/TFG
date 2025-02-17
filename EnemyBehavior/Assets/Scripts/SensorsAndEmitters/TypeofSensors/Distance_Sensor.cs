using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// Observation: The distance between the two objects is measured from their center.
public class Distance_Sensor : Sensors
{
    [SerializeField]
    private GameObject m_target; // Target object whose distance is measured

    [SerializeField, Min(0)]
    public float detectionDistance = 5f; // Threshold distance to trigger the event

    public bool useMagnitude = true; // If true, measures full magnitude; otherwise, measures a single axis

    public bool checkXAxis = true; // If true, measures along the X-axis; otherwise, measures along the Y-axis

    private float m_maxDistance;
    int layerMask;
    // Initializes the sensor settings
    public override void StartSensor()
    {
        
        useMagnitude = true;
        checkXAxis = true;
        if (m_target != null)
        {
          Vector3 outerpoint = m_target.GetComponent<Collider2D>().bounds.max;
            Vector2 outerpoint2D = new Vector2 (outerpoint.x, outerpoint.y);
            m_maxDistance = Vector2.Distance(outerpoint2D, m_target.transform.position);
            layerMask = ~LayerMask.GetMask("Enemies");
        }
    }

    // Determines if the sensor should transition based on distance
    public override bool CanTransition()
    {
        // If there is no target, return false
        if (m_target == null) return false;

        // Calculate distance based on the selected method
        float distance = useMagnitude
            ? Vector2.Distance(transform.position, m_target.transform.position) // Full magnitude distance
            : checkXAxis
                ? Mathf.Abs(transform.position.x - m_target.transform.position.x) // Distance along X-axis
                : Mathf.Abs(transform.position.y - m_target.transform.position.y); // Distance along Y-axis

       // Debug.Log(distance + "distance" + detectionDistance +" _"+  m_maxDistance + "other ");    
        // Check if the distance is within the threshold
        if (distance <= detectionDistance + m_maxDistance)
        {
            // Direction from current position to target
            Vector2 direction = (m_target.transform.position - transform.position).normalized;

            // Perform the Raycast

           
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance, layerMask);
            Debug.DrawRay(transform.position, direction * detectionDistance, Color.red, 0.1f);
            
            // Check if the Raycast hits something before the target
            if (hit.collider != null && hit.collider.gameObject == m_target)
            {
                EventDetected(); // Trigger the event
                return true;
            }
            else return false;
        }
        else return false;
    }
    // Draws the detection range in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (useMagnitude)
        {
            // Draw a circle representing the detection radius
            Gizmos.DrawWireSphere(transform.position, detectionDistance);
        }
        else
        {
            // Draw a line representing the detection range along a single axis
            Vector3 start = transform.position;
            Vector3 end = checkXAxis
                ? new Vector3(transform.position.x + detectionDistance, transform.position.y, transform.position.z)
                : new Vector3(transform.position.x, transform.position.y + detectionDistance, transform.position.z);

            Gizmos.DrawLine(start, end);
            Gizmos.DrawLine(start, start - (end - start)); // Draw opposite side for better visualization
        }
    }
}
