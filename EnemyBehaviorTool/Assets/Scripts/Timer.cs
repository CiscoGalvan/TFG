using System;
using UnityEditor;
using UnityEngine;

public class Timer
{
    // The required time for detection to activate
    private float _detectionTime;
    private float _timer;
    private bool _isRunning;

    // Constructor to initialize the timer with a specified detection time
    public Timer(float detectionTime)
    {
        // Ensure the detection time is not negative
        _detectionTime = Mathf.Max(0, detectionTime);
        _isRunning = false;
    }

    // Starts the timer, must be called from an external Update method
    public void Start()
    {
        _timer = 0f;
        _isRunning = true;
    }

    // Resets the timer
    public void Reset()
    {
        _timer = 0f;
        _isRunning = false;
    }

    // Updates the timer, must be called from an external Update method
    public void Update(float deltaTime)
    {
        // If the timer is not running, exit the method
        if (!_isRunning)
            return;

        // Increment the timer by the elapsed time
        _timer += deltaTime;

        // If the timer exceeds or equals the detection time, stop the timer
        if (_timer >= _detectionTime)
        {
            _isRunning = false;

        }
    }

    // Gets the remaining time
    public float GetTimeRemaining()
    {
        // Return the remaining time, ensuring it is not negative
        return Mathf.Max(0, _detectionTime - _timer);
    }
}