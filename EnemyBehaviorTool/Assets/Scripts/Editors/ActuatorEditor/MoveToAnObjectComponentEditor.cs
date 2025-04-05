using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveToAnObjectActuator))]
public class MoveToAnObjectComponentEditor : ActuatorEditor
{
	SerializedProperty _waypointData;
	//private static readonly GUIContent _shouldStopLabel = new GUIContent("Should Stop", "Indicates whether the enemy should stop upon reaching the waypoint.");
	private static readonly GUIContent _easingFunctionToAPointLabel = new GUIContent("Easing Function", "Easing function that will describe the progress of the position.");
	private static readonly GUIContent _timeToReachLabel = new GUIContent("Time To Reach", "Time it takes to reach the waypoint.");
	private static readonly GUIContent _waypointTransformLabel = new GUIContent("Waypoint Transform", "Reference to the waypoint transform.");
	private static readonly GUIContent _isAcceleratedLabel = new GUIContent("Is Accelerated", "Is the movement towards the waypoint accelerated?");
	//private static readonly GUIContent _stopDurationLabel = new GUIContent("Stop duration", "Time it will take the enemy to start movement to the next waypoint.");

	private void OnEnable()
	{
		_waypointData = serializedObject.FindProperty("_waypointData");
	}
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		var waypointTransform = _waypointData.FindPropertyRelative("waypoint");
		EditorGUILayout.PropertyField(waypointTransform, _waypointTransformLabel);


		var timeToReach = _waypointData.FindPropertyRelative("timeToReach");
		timeToReach.floatValue = Mathf.Max(0, timeToReach.floatValue);
		EditorGUILayout.PropertyField(timeToReach, _timeToReachLabel);

		var isAccelerated = _waypointData.FindPropertyRelative("isAccelerated");
		EditorGUILayout.PropertyField(isAccelerated, _isAcceleratedLabel);

		if (isAccelerated.boolValue)
		{
			EditorGUI.indentLevel++;
			var easingFunctionProp = _waypointData.FindPropertyRelative("easingFunction");
			EditorGUILayout.PropertyField(easingFunctionProp, _easingFunctionToAPointLabel);
			EasingFunction.Ease easingEnum = (EasingFunction.Ease)easingFunctionProp.intValue;
			DrawEasingCurve(easingEnum);
			EditorGUI.indentLevel--;
		}
		serializedObject.ApplyModifiedProperties();
	}
    public override void DrawEasingCurve(EasingFunction.Ease easing)
    {
        // We clean the curve's keyframes before updating them.
        easingCurve.keys = new Keyframe[0];
        float step = 1f / (EASING_GRAPH_NUMBER_OF_POINTS - 1);
        for (int i = 0; i < EASING_GRAPH_NUMBER_OF_POINTS; i++)
        {
            float t = i * step;
            float y = EasingFunction.GetEasingFunction(easing)(0, 1, t);
            easingCurve.AddKey(t, y);
        }

        // Reserve space and draw the easing curve
        Rect curveRect = GUILayoutUtility.GetRect(EASING_GRAPH_WIDTH, EASING_GRAPH_HEIGHT, GUILayout.ExpandWidth(true));
        EditorGUI.CurveField(curveRect, easingCurve);

        // Draw axis labels (small offsets to place them properly)
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 10;

        // Label "X" near bottom right
        Vector2 xLabelPos = new Vector2(curveRect.xMax - 45, curveRect.yMax - 15);
        GUI.Label(new Rect(xLabelPos, new Vector2(40, 20)), "X: Time", labelStyle);

        // Label "Y" near top left
        Vector2 yLabelPos = new Vector2(curveRect.xMin + 15, curveRect.yMin - 2);
        GUI.Label(new Rect(yLabelPos, new Vector2(60, 20)), "Y: Position ", labelStyle);
    }
}
