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

    [SerializeField]
    [Tooltip("if this is active the message is send on Distance enter otherway it is send when its far")]
    private bool _onDistanceEnter= true; // Target object whose distance is measured
    //private bool _isNear;
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
            if (distance <= _detectionDistance && _onDistanceEnter)
            {
                
                EventDetected(); // Trigger the event
                 
            }
            else if (!_onDistanceEnter)
            {
                
                EventDetected(); // Trigger the event
            }
        }
       
    }
    // Draws the detection range in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.3f); // Azul semitransparente
        if (_useMagnitude)
        {
            Gizmos.DrawSphere(transform.position, _detectionDistance);
        }
        else
        {
            Vector3 start = transform.position;
            Vector3 size = _checkXAxis
                ? new Vector3(_detectionDistance * 2, 1, 1)
                : new Vector3(1, _detectionDistance * 2, 1);

            Gizmos.DrawCube(start, size);
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
