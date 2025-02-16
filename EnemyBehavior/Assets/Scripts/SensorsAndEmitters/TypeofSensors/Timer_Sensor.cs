using System;
using UnityEditor;
using UnityEngine;

public class Timer_Sensor : Sensors
{
    // Time required for detection to trigger
    [SerializeField, Min(0)]
    private float detectionTime = 5f;

    // Tracks the elapsed time
    private float timer = 0f;

    // Indicates whether the timer is active
    private bool startTimer;


    private void Update()
    {
        // If the timer is active, increment time
        if (startTimer)
        {
            timer += UnityEngine.Time.deltaTime;

            // Check if the timer has reached the detection time
            if (timer >= detectionTime)
            {
                EventDetected(); // Trigger event
                timer = 0f; // Reset timer
            }
        }
    }

    // Activates the sensor. Is the firts method call
    public override void StartSensor()
    {
        timer = 0f;
        startTimer = true;
    }

    // Determines if the sensor whants to change the state
    public override bool CanTransition()
    {
        return timer >= detectionTime;
    }

    // Displays the remaining time in the scene view (editor only)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float timeRemaining = Mathf.Max(0, detectionTime - timer);
        Handles.Label(transform.position + Vector3.up * 1.5f, $"Time Remaining: {timeRemaining:0.00}s");
    }
}
