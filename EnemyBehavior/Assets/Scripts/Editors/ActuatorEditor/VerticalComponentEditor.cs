using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static MoveToAPoint_Actuator;

[CustomEditor(typeof(Vertical_Actuator))]
public class VerticalComponentEditor : ActuatorEditor
{
    private static readonly GUIContent bouncingLabel = new GUIContent("Bounce Object", "Does the object bounce after collision?");
    private static readonly GUIContent destroyLabel = new GUIContent("Destroy Object", "Does the object self-destruct after the collision?");
	private bool _showMovementInfo = true;

	#region Accelerated movement
	private static readonly GUIContent goalSpeedLabel = new GUIContent("Goal Speed", "Speed the object will reach");
	private static readonly GUIContent interpolationTimeLabel = new GUIContent("Interpolation Time", "Time it takes to reach Max Speed");
	#endregion

	#region  Non-accelerated movement
	private static readonly GUIContent constantSpeedLabel = new GUIContent("Speed", "The object will move with this constant speed.");
	#endregion

	private SerializedProperty directionProperty;

	private void OnEnable()
	{
		directionProperty = serializedObject.FindProperty("_direction");
	}

	public override void OnInspectorGUI()
	{

        Vertical_Actuator component = (Vertical_Actuator)target;
		DrawDefaultInspector();
        // Variables auxiliares para evitar selección simultánea
        bool bounces = component.GetBouncesAfterCollision();
        bool destroys = component.GetDestroyAfterCollision();

        bounces = EditorGUILayout.Toggle(bouncingLabel, bounces);
        if (bounces) destroys = false; // Si se selecciona rebotar, desactiva destruir

        destroys = EditorGUILayout.Toggle(destroyLabel, destroys);
        if (destroys) bounces = false; // Si se selecciona destruir, desactiva rebotar

        component.SetBouncesAfterCollision(bounces);
        component.SetDestroyAfterCollision(destroys);
		EditorGUI.indentLevel++;
		_showMovementInfo = EditorGUILayout.Foldout(_showMovementInfo, "Movement Info", true);
		if (_showMovementInfo)
		{
			EditorGUILayout.PropertyField(directionProperty, new GUIContent("Direction"));
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
		}
        serializedObject.ApplyModifiedProperties();
        // If GUI changed we must applicate those changes in editor
        if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}
	}
}
