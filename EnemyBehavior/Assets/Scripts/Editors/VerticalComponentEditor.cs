using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Vertical_Actuator))]
public class VerticalComponentEditor : ActuatorEditor
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

        Vertical_Actuator component = (Vertical_Actuator)target;
		DrawDefaultInspector();
		//creo que este es el lineal y el else el acelerado.
		if (component.IsMovementAccelerated())
		{
			component.SetGoalSpeed(Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(goalSpeedLabel, component.GetGoalSpeed()))));
			component.SetInterpolationTime(Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(interpolationTimeLabel, component.GetInterpolationTime()))));
			component.SetEasingFunction((EasingFunction.Ease)EditorGUILayout.EnumPopup(easingFunctionLabel, component.GetEasingFunctionValue()));
			DrawEasingCurve(component.GetEasingFunctionValue());
		}
		else
		{
			component.SetSpeed(Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(constantSpeedLabel, component.GetSpeed()))));
		}

		// If GUI changed we must applicate those changes in editor
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}
	}
}
