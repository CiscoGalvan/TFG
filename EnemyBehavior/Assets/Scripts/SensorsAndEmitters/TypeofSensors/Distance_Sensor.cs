using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// Observation: The distance between the two objects is measured from their center.
public class Distance_Sensor : Sensors
{
    [SerializeField]
    private Transform m_target; // Target object whose distance is measured

    [SerializeField, Min(0)]
    public float detectionDistance = 5f; // Threshold distance to trigger the event

    public bool useMagnitude = true; // If true, measures full magnitude; otherwise, measures a single axis

    public bool checkXAxis = true; // If true, measures along the X-axis; otherwise, measures along the Y-axis

    // Initializes the sensor settings
    public override void StartSensor()
    {
        useMagnitude = true;
        checkXAxis = true;
    }

    // Determines if the sensor should transition based on distance
    public override bool CanTransition()
    {
        // If there is no target, return false
        if (m_target == null) return false;

        // Calculate distance based on the selected method
        float distance = useMagnitude
            ? Vector2.Distance(transform.position, m_target.position) // Full magnitude distance
            : checkXAxis
                ? Mathf.Abs(transform.position.x - m_target.position.x) // Distance along X-axis
                : Mathf.Abs(transform.position.y - m_target.position.y); // Distance along Y-axis

        // Check if the distance is within the threshold
        if (distance <= detectionDistance)
        {
            EventDetected(); // Trigger the event
            return true;
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
