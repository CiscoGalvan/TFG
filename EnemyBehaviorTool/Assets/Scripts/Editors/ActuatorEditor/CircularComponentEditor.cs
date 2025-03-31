using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CircularActuator))]
public class CircularComponentEditor : ActuatorEditor
{
	#region Accelerated movement
	private static readonly GUIContent goalSpeedLabel = new GUIContent("Goal Speed", "Speed the object will reach");
	private static readonly GUIContent interpolationTimeLabel = new GUIContent("Interpolation Time", "Time it takes to reach Max Speed");
	#endregion

	#region  Non-accelerated movement
	private static readonly GUIContent constantSpeedLabel = new GUIContent("Speed", "The object will move with this constant speed.");
	#endregion
	public override void OnInspectorGUI()
	{

		CircularActuator component = (CircularActuator)target;
		DrawDefaultInspector();

		if (component.IsMovementAccelerated())
		{
			component.SetGoalAngularSpeed(Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(goalSpeedLabel, component.GetGoalAngularSpeed()))));
			component.SetInterpolationTime(Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(interpolationTimeLabel, component.GetInterpolationTime()))));
			component.SetEasingFunction((EasingFunction.Ease)EditorGUILayout.EnumPopup(_easingFunctionLabel, component.GetEasingFunctionValue()));
			DrawEasingCurve(component.GetEasingFunctionValue());
		}
		else
		{
			component.SetAngularSpeed(Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(constantSpeedLabel, component.GetAngularSpeed()))));
		}
		
		// If GUI changed we must applicate those changes in editor
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}
	}
}
