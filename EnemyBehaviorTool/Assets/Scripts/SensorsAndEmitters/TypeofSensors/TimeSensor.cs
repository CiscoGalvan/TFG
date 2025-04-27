using UnityEditor;
using UnityEngine;

public class TimeSensor : Sensor
{
    // Time required for detection to trigger
    [SerializeField, Min(0)]
    private float _detectionTime = 5f;

    // Instance of Timer
    private Timer _ownTimer;

    // Override the StartSensor method to initialize the timer
    public override void StartSensor()
    {
        base.StartSensor(); // Call the base class StartSensor
        _ownTimer = new Timer(_detectionTime); // Create a new Timer with the specified detection time
        _ownTimer.Start(); // Start the timer
    }

    // Override the UpdateSensor method to handle the timer logic
    public override void UpdateSensor()
    {
        base.UpdateSensor(); // Call the base class UpdateSensor
        

        // If the sensor is not active or the base timer has not finished, exit
        if (!_sensorActive || !_timerFinished) return;


        _ownTimer.Update(Time.deltaTime); // Update the timer with the elapsed time

        // If the timer has reached the detection time, trigger the event
        if (_ownTimer.GetTimeRemaining() <= 0)
        {
            EventDetected(); // Trigger the event
            _ownTimer.Reset(); // Reset the timer after detection
        }
    }

    // Displays the remaining time in the scene view (editor only)
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        // If the sensor is not active, do not display the label
        if (!_sensorActive) return;

        Gizmos.color = Color.blue; // Set the gizmo color to blue

        // Get the remaining time from the timer or use the default detection time
        float timeRemaining = _ownTimer != null ? _ownTimer.GetTimeRemaining() : _detectionTime;

        // Display the remaining time as a label above the GameObject
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, $"Time Remaining: {timeRemaining:0.00}s");
#endif
    }
}