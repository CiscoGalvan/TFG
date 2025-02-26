using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// Observation: The distance between the two objects is measured from their center.
public class Distance_Sensor : Sensors
{
    [SerializeField]
    private GameObject _target; // Target object whose distance is measured

    [SerializeField, Min(0)]
    private float _detectionDistance = 5f; // Threshold distance to trigger the event
   
    [SerializeField]
    private bool _useMagnitude = true; // If true, measures full magnitude; otherwise, measures a single axis
   
    [SerializeField]
    [HideInInspector]
    private bool _checkXAxis = false; // If true, measures along the X-axis; otherwise, measures along the Y-axis

    // Indicates whether the timer is active
    private bool _startDistance;
    // Initializes the sensor settings
    public override void StartSensor()
    {
        
        _useMagnitude = true;
        _startDistance = true;

    }

    // Determines if the sensor should transition based on distance
    private void Update()
    {
        // If there is no target, return false
        if (_target != null && _startDistance)
        {
            // Calculate distance based on the selected method
            float distance = _useMagnitude
                ? Vector2.Distance(transform.position, _target.transform.position) // Full magnitude distance
                : _checkXAxis
                    ? Mathf.Abs(transform.position.x - _target.transform.position.x) // Distance along X-axis
                    : Mathf.Abs(transform.position.y - _target.transform.position.y); // Distance along Y-axis

     
            // Check if the distance is within the threshold
            if (distance <= _detectionDistance )
            {
            
                EventDetected(); // Trigger the event
                 
            }
        }
       
    }
    // Draws the detection range in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (_useMagnitude)
        {
            // Draw a circle representing the detection radius
            Gizmos.DrawWireSphere(transform.position, _detectionDistance);
        }
        else
        {
            // Draw a line representing the detection range along a single axis
            Vector3 start = transform.position;
            Vector3 end = _checkXAxis
                ? new Vector3(transform.position.x + _detectionDistance, transform.position.y, transform.position.z)
                : new Vector3(transform.position.x, transform.position.y + _detectionDistance, transform.position.z);

            Gizmos.DrawLine(start, end);
            Gizmos.DrawLine(start, start - (end - start)); // Draw opposite side for better visualization
        }
    }
    
    public bool GetUseMagnitude()
    {
        return _useMagnitude;
    }
    public bool GetCheckXAxis()
    {
        return _checkXAxis;
    }

    public void SetCheckXAxis(bool b)
    {
         _checkXAxis = b;
    }
    public void SetDetectionDistance(float newValue)
    {
        _detectionDistance = newValue;
    }
    public void SetTarget(GameObject g)
    {
        _target = g;
    }
}
