using UnityEditor;
using UnityEngine;
using static Distance_Sensor;

[CustomEditor(typeof(Distance_Sensor))]
public class DistanceSensorEditor : Editor
{
    private static readonly GUIContent _distanceTypeLabel = new GUIContent("Distance Type", "Select the type of distance measurement.");
    private static readonly GUIContent _axisLabel = new GUIContent("Axis", "Choose the axis along which distance is measured.");
    private static readonly GUIContent _partOfAxisLabel = new GUIContent("Part of Axis", "Defines whether detection is on one side or the other.");
    private static readonly GUIContent _detectionSideLabel = new GUIContent("Detection Sides", "Defines whether detection is on one side or both sides of the axis.");
    private static readonly GUIContent _detectionDistanceLabel = new GUIContent("Detection Distance", "The threshold distance for detection.");
    private static readonly GUIContent _targetLabel = new GUIContent("Target", "The object to measure distance from.");
    private static readonly GUIContent _areaTriggerLabel = new GUIContent("Area Trigger", "External trigger used for area-based detection.");

    private SerializedProperty _distanceType;
    private SerializedProperty _axis;
    private SerializedProperty _partOfAxis;
    private SerializedProperty _detectionSide;
    private SerializedProperty _detectionDistance;
    private SerializedProperty _target;
    private SerializedProperty _areaTrigger;

    private void OnEnable()
    {
        _distanceType = serializedObject.FindProperty("_distanceType");
        _axis = serializedObject.FindProperty("_axis");
        _partOfAxis = serializedObject.FindProperty("_partOfAxis");
        _detectionSide = serializedObject.FindProperty("_detectionSide");
        _detectionDistance = serializedObject.FindProperty("_detectionDistance");
        _target = serializedObject.FindProperty("_target");
        _areaTrigger = serializedObject.FindProperty("_areaTrigger");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_distanceType, _distanceTypeLabel);
        EditorGUILayout.PropertyField(_target, _targetLabel);
        

        Distance_Sensor sensor = (Distance_Sensor)target;
        if (sensor.GetDistanceType() == Distance_Sensor.TypeOfDistance.SingleAxis)
        {   
            EditorGUILayout.PropertyField(_detectionDistance, _detectionDistanceLabel);
            EditorGUILayout.PropertyField(_axis, _axisLabel);
            EditorGUILayout.PropertyField(_detectionSide, _detectionSideLabel);
            if (sensor.GetDetectionSide() != Distance_Sensor.DetectionSide.Both)
            {
                EditorGUILayout.PropertyField(_partOfAxis, _partOfAxisLabel);
            }

        }
        else if (sensor.GetDistanceType() == Distance_Sensor.TypeOfDistance.Area)
        {
            EditorGUILayout.PropertyField(_areaTrigger, _areaTriggerLabel);

        }
        else if (sensor.GetDistanceType() == Distance_Sensor.TypeOfDistance.Magnitude)
        {
            EditorGUILayout.PropertyField(_detectionDistance, _detectionDistanceLabel);
        }

       serializedObject.ApplyModifiedProperties();
    }
}
