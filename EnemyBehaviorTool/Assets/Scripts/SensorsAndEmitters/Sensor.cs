using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public abstract class Sensor : MonoBehaviour
{
    // Internal event for actions when the sensor detects an event
    private Action<Sensor> _onEventDetectedInternal;

    // Debugging flag to display sensor-related information
    protected bool _debugSensor;

    // Counter to track the number of subscribers to the event
    private int _subscriberCount = 0;

    // Indicates whether the sensor is currently active
    protected bool _sensorActive;

    [SerializeField, Min(0)]
    [Tooltip("Initial time the sensor will need to be active")]
    protected float _startDetectingTime = 0f; // Initial delay before the sensor starts detecting
    protected Timer _timer; // Timer to manage the delay
    protected bool _timerFinished = false; // Tracks whether the timer has completed

    // Event to notify when the sensor detects something, with custom add/remove logic
    public event Action<Sensor> onEventDetected
    {
        add
        {
            // Add a subscriber and increment the counter
            _onEventDetectedInternal += value;
            _subscriberCount++;
        }
        remove
        {
            // Remove a subscriber and decrement the counter
            _onEventDetectedInternal -= value;
            if (_subscriberCount <= 0)
            {
                Debug.LogError("Attempted to remove a subscriber when there are none.");
                return;
            }
            _subscriberCount--;
        }
    }

    // Method to trigger the event when the sensor detects something
    public void EventDetected()
    {
        // Invoke the event if there are any subscribers
        _onEventDetectedInternal?.Invoke(this);
    }

    // Starts the sensor and initializes the timer if necessary
    public virtual void StartSensor()
    {
        _sensorActive = true; // Activate the sensor
        _timer = new Timer(_startDetectingTime); // Create a timer with the specified delay

        if (_startDetectingTime > 0)
        {
            _timer.Start(); // Start the timer
            _timerFinished = false; // Timer is not yet finished
        }
        else
        {
            _timerFinished = true; // No delay, timer is considered finished
        }
    }

    // Updates the sensor logic, typically called every frame
    public virtual void UpdateSensor()
    {
        // If the sensor is not active, skip the update
        if (!_sensorActive) return;

        // Handle the timer logic if it hasn't finished yet
        if (!_timerFinished)
        {
            _timer.Update(Time.deltaTime); // Update the timer with the elapsed time
            if (_timer.GetTimeRemaining() <= 0)
            {
                _timerFinished = true; // Mark the timer as finished
            }
            else
            {
                return; // Exit early if the timer is still running
            }
        }
    }

    // Stops the sensor from being active
    public virtual void StopSensor()
    {
        _sensorActive = false; // Deactivate the sensor
    }

    // Enables or disables debug mode
    public void SetDebug(bool debug)
    {
        _debugSensor = debug; // Set the debug flag
    }

    // Unity's Update method, calls the sensor's update logic
    private void Update()
    {
        UpdateSensor(); // Delegate to the sensor's update logic
    }
}