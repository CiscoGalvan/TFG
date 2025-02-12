using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MoveToAPoint))]
public class MoveToAPointEditor : ActuatorEditor
{
	public override void OnInspectorGUI()
	{
		
		MoveToAPoint component = (MoveToAPoint)target;

		DrawDefaultInspector();

		if (component.IsMovementAccelerated())
		{
			component.SetEasingFunction((EasingFunction.Ease)EditorGUILayout.EnumPopup(easingFunctionLabel, component.GetEasingFunctionValue()));
			DrawEasingCurve(component.GetEasingFunctionValue());
		}
		// If GUI changed we must applicate those changes in editor
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}
	}
}
