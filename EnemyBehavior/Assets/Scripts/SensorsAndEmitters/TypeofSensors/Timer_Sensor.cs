using System;
using UnityEditor;
using UnityEngine;

public class Timer_Sensor : Sensors
{
    // Time required for detection to trigger
    [SerializeField, Min(0)]
    private float _detectionTime = 5f;

    // Tracks the elapsed time
    private float _timer = 0f;

    // Indicates whether the timer is active
    private bool _startTimer;


    private void Update()
    {
        // If the timer is active, increment time
        if (_startTimer)
        {
            _timer += UnityEngine.Time.deltaTime;

            // Check if the timer has reached the detection time
            if (_timer >= _detectionTime)
            {
                EventDetected(); // Trigger event
                _timer = 0f; // Reset timer
            }
        }
    }

    // Activates the sensor. Is the firts method call
    public override void StartSensor()
    {
        _timer = 0f;
        _startTimer = true;
    }

    // Displays the remaining time in the scene view (editor only)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float timeRemaining = Mathf.Max(0, _detectionTime - _timer);
        Handles.Label(transform.position + Vector3.up * 1.5f, $"Time Remaining: {timeRemaining:0.00}s");
    }
}
