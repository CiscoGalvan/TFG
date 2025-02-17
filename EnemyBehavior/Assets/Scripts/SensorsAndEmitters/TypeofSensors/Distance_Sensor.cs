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
    private float m_detectionDistance = 5f; // Threshold distance to trigger the event
   
    [SerializeField]
    private bool m_useMagnitude = true; // If true, measures full magnitude; otherwise, measures a single axis
   
    [SerializeField]
    [HideInInspector]
    private bool m_checkXAxis = true; // If true, measures along the X-axis; otherwise, measures along the Y-axis

    private float m_maxDistance;
    private int m_layerMask; //no collision mask
    // Initializes the sensor settings
    public override void StartSensor()
    {
        
        m_useMagnitude = true;
        m_checkXAxis = true;
        if (m_target != null)
        {
          Vector3 outerpoint = m_target.GetComponent<Collider2D>().bounds.max;
            Vector2 outerpoint2D = new Vector2 (outerpoint.x, outerpoint.y);
            m_maxDistance = Vector2.Distance(outerpoint2D, m_target.transform.position);
            m_layerMask = ~LayerMask.GetMask("Enemies");
        }
    }

    // Determines if the sensor should transition based on distance
    public override bool CanTransition()
    {
        // If there is no target, return false
        if (m_target == null) return false;

        // Calculate distance based on the selected method
        float distance = m_useMagnitude
            ? Vector2.Distance(transform.position, m_target.transform.position) // Full magnitude distance
            : m_checkXAxis
                ? Mathf.Abs(transform.position.x - m_target.transform.position.x) // Distance along X-axis
                : Mathf.Abs(transform.position.y - m_target.transform.position.y); // Distance along Y-axis

       // Debug.Log(distance + "distance" + detectionDistance +" _"+  m_maxDistance + "other ");    
        // Check if the distance is within the threshold
        if (distance <= m_detectionDistance + m_maxDistance)
        {
            // Direction from current position to target
            Vector2 direction = (m_target.transform.position - transform.position).normalized;

            // Perform the Raycast

           
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, m_detectionDistance, m_layerMask);
            Debug.DrawRay(transform.position, direction * m_detectionDistance, Color.red, 0.1f);
            
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
        if (m_useMagnitude)
        {
            // Draw a circle representing the detection radius
            Gizmos.DrawWireSphere(transform.position, m_detectionDistance);
        }
        else
        {
            // Draw a line representing the detection range along a single axis
            Vector3 start = transform.position;
            Vector3 end = m_checkXAxis
                ? new Vector3(transform.position.x + m_detectionDistance, transform.position.y, transform.position.z)
                : new Vector3(transform.position.x, transform.position.y + m_detectionDistance, transform.position.z);

            Gizmos.DrawLine(start, end);
            Gizmos.DrawLine(start, start - (end - start)); // Draw opposite side for better visualization
        }
    }
    
    public bool GetUseMagnitude()
    {
        return m_useMagnitude;
    }
    public bool GetCheckXAxis()
    {
        return m_checkXAxis;
    }

    public void SetCheckXAxis(bool b)
    {
         m_checkXAxis = b;
    }
}
