using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DistanceSensor;

[RequireComponent(typeof(Collider2D))]
public class AreaSensor : Sensor
{
    [SerializeField]
    private GameObject _target; // The target object to detect within the area

    private Collider2D _detectionZone; // The Collider2D that represents the detection area

    [SerializeField]
    private DetectionCondition _detectionCondition = DetectionCondition.InsideMagnitude; // Condition for detection (inside or outside area)

    // Enum to define detection conditions
    public enum DetectionCondition
    {
        InsideMagnitude = 0, // Trigger when the target is inside the area
        OutsideMagnitude = 1 // Trigger when the target is outside the area
    };

    // Initializes the sensor and sets up the detection zone
    public override void StartSensor()
    {
        base.StartSensor();
        _detectionZone = gameObject.GetComponent<Collider2D>(); // Get the Collider2D component
        _detectionZone.isTrigger = true; // Ensure the collider acts as a trigger
    }

    // Called when another object enters the detection zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only process the event if the sensor is active and the timer has finished
        if (!_sensorActive || !_timerFinished) return;

        // Trigger the event if the condition is InsideMagnitude and the target matches
        if (_detectionCondition == DetectionCondition.InsideMagnitude && collision.gameObject == _target)
        {
            EventDetected(); // Notify listeners of the detection event
        }
    }

    // Called when another object remains inside the detection zone
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Only process the event if the sensor is active and the timer has finished
        if (!_sensorActive || !_timerFinished) return;

        // Trigger the event if the condition is InsideMagnitude and the target matches
        if (_detectionCondition == DetectionCondition.InsideMagnitude && collision.gameObject == _target)
        {
            EventDetected(); // Notify listeners of the detection event
        }
    }

    // Called when another object exits the detection zone
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Only process the event if the sensor is active and the timer has finished
        if (!_sensorActive || !_timerFinished) return;

        // Trigger the event if the condition is OutsideMagnitude and the target matches
        if (_detectionCondition == DetectionCondition.OutsideMagnitude && collision.gameObject == _target)
        {
            EventDetected(); // Notify listeners of the detection event
        }
    }
}