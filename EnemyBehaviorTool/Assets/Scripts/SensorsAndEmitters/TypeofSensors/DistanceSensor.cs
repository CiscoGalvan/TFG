using System;
using UnityEngine;
using UnityEditor;

// Observation: The distance between the two objects is measured from their center.
public class DistanceSensor : Sensor
{
    // Different modes for distance calculation
    public enum TypeOfDistance
    {
        Magnitude = 0,  // Measure the entire distance (magnitude)
        SingleAxis = 1  // Measure distance along a single axis (X or Y)
    };

    // Conditions for detection
    public enum DetectionCondition
    {
        InsideMagnitude = 0, // Trigger detection when inside the detection range
        OutsideMagnitude = 1 // Trigger detection when outside the detection range
    };

    // Axes for single-axis distance calculation
    public enum Axis
    {
        Y = 0, // Measure distance along the Y-axis
        X = 1  // Measure distance along the X-axis
    };

    // Part of the axis for single-axis detection
    public enum PartOfAxis
    {
        UpOrLeft = 0,   // Detect targets above or to the left
        DownOrRight = 1 // Detect targets below or to the right
    };

    // Detection side configuration
    public enum DetectionSide
    {
        Both = 0,  // Detect on both sides of the axis
        Single = 1 // Detect on one side of the axis
    };

    [SerializeField] private TypeOfDistance _distanceType = TypeOfDistance.Magnitude; // Default distance type
    [SerializeField] private Axis _axis = Axis.X; // Default axis for single-axis detection
    [SerializeField] private PartOfAxis _partOfAxis = PartOfAxis.UpOrLeft; // Default part of axis
    [SerializeField] private DetectionSide _detectionSide = DetectionSide.Both; // Default detection side

    [SerializeField]
    private GameObject _target; // Target object to measure distance against

    [SerializeField, Min(0)]
    private float _detectionDistance = 5f; // Threshold distance for detection

    [SerializeField, Tooltip("External trigger used for area detection.")]
    private Collider2D _areaTrigger; // External trigger for area detection

    [SerializeField]
    private DetectionCondition _detectionCondition = DetectionCondition.InsideMagnitude; // Default detection condition

    // Initializes the sensor settings
    public override void StartSensor()
    {
        base.StartSensor();
        if (_target == null)
        {
            Debug.LogError($"No target set in Distance Sensor in object {name}");
        }
    }

    // Determines if the sensor should transition based on distance
    private void Update()
    {
        base.UpdateSensor();

        // Exit early if the target is null or the timer has not finished
        if (_target == null || !_timerFinished)
            return;

        Vector2 selfPos = transform.position; // Position of the sensor
        Vector2 targetPos = _target.transform.position; // Position of the target
        bool detected = false; // Tracks whether detection occurred

        switch (_distanceType)
        {
            case TypeOfDistance.Magnitude:
                // Check if the distance is within the threshold
                bool isWithinMagnitude = Vector2.Distance(selfPos, targetPos) <= _detectionDistance;
                detected = (_detectionCondition == DetectionCondition.InsideMagnitude) ? isWithinMagnitude : !isWithinMagnitude;
                break;

            case TypeOfDistance.SingleAxis:
                // Check distance along a single axis
                float distance = (_axis == Axis.X)
                    ? Mathf.Abs(selfPos.x - targetPos.x)
                    : Mathf.Abs(selfPos.y - targetPos.y);

                // Verify if the target is on the correct side
                bool correctSide = _detectionSide == DetectionSide.Both ||
                   (_axis == Axis.X
                       ? (_partOfAxis == PartOfAxis.UpOrLeft ? targetPos.x < selfPos.x : targetPos.x > selfPos.x)
                       : (_partOfAxis == PartOfAxis.UpOrLeft ? targetPos.y > selfPos.y : targetPos.y < selfPos.y));

                bool isWithinSingleAxis = distance <= _detectionDistance && correctSide;
                detected = (_detectionCondition == DetectionCondition.InsideMagnitude) ? isWithinSingleAxis : !isWithinSingleAxis;
                break;
        }

        // Trigger the detection event if the condition is met
        if (detected)
        {
            EventDetected();
        }
    }

    // Draws the detection range in the scene view
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!_debugSensor) return; // Exit if debug mode is not enabled
        Gizmos.color = new Color(0, 0, 1, 0.3f); // Set gizmo color to blue

        switch (_distanceType)
        {
            case TypeOfDistance.Magnitude:
                // Draw a circular detection range
                Handles.color = new Color(0, 0, 1, 0.3f);
                Handles.DrawSolidDisc(transform.position, Vector3.forward, _detectionDistance);
                break;

            case TypeOfDistance.SingleAxis:
                Vector3 size;
                Vector3 positionOffset = Vector3.zero;

                if (_axis == Axis.X)
                {
                    if (_detectionSide == DetectionSide.Both)
                    {
                        size = new Vector3(_detectionDistance * 2, 10, 1); // Full width on both sides
                    }
                    else
                    {
                        size = new Vector3(_detectionDistance, 10, 1); // Half width on one side
                        positionOffset = new Vector3(
                            _partOfAxis == PartOfAxis.UpOrLeft ? -_detectionDistance / 2 : _detectionDistance / 2,
                            0, 0);
                    }
                }
                else // Axis.Y
                {
                    if (_detectionSide == DetectionSide.Both)
                    {
                        size = new Vector3(10, _detectionDistance * 2, 1); // Full height on both sides
                    }
                    else
                    {
                        size = new Vector3(10, _detectionDistance, 1); // Half height on one side
                        positionOffset = new Vector3(
                            0, _partOfAxis == PartOfAxis.UpOrLeft ? _detectionDistance / 2 : -_detectionDistance / 2,
                            0);
                    }
                }

                // Draw the detection range as a rectangle
                Gizmos.DrawCube(transform.position + positionOffset, size);
                break;
        }
#endif
    }

    // Set a new detection distance
    public void SetDetectionDistance(float newValue)
    {
        _detectionDistance = newValue;
    }

    // Set a new target for the sensor
    public void SetTarget(GameObject g)
    {
        _target = g;
    }
}