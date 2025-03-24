using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(HorizontalActuator))]
public class HorizontalComponentEditor : ActuatorEditor
{

	private static readonly GUIContent _onCollisionReactionLabel = new GUIContent("Reaction After Collision", "What will the object do after collision?\n" +
		"None: The object will not react to the collision.\n" +
		"Bounce: The object will bounce to the opposite direction.\n" +
		"Destroy: The object will be destroyed after the contact.");
	private static readonly GUIContent _directionLabel = new GUIContent("Direction", "Direction of the horizontal movement");
	private bool _showMovementInfo = true;

	#region Accelerated movement
	private static readonly GUIContent goalSpeedLabel = new GUIContent("Goal Speed", "Speed the object will reach");
	private static readonly GUIContent interpolationTimeLabel = new GUIContent("Interpolation Time", "Time it takes to reach Max Speed");
    #endregion

    #region  Non-accelerated movement
    private static readonly GUIContent _throwLabel = new GUIContent("Throw", "The object will be moved only once, when the actuator is activated.");
    private static readonly GUIContent _constantSpeedLabel = new GUIContent("Speed", "The object will move with this constant speed.");
   	#endregion

	private SerializedProperty _directionProperty;
	private SerializedProperty _onCollisionReaction;
	private SerializedProperty _throw;

	private void OnEnable()
	{
		_directionProperty = serializedObject.FindProperty("_direction");
		_onCollisionReaction = serializedObject.FindProperty("_onCollisionReaction");
		_throw = serializedObject.FindProperty("_throw");
	}
	public override void OnInspectorGUI()
	{

		HorizontalActuator component = (HorizontalActuator)target;
		DrawDefaultInspector();


		EditorGUILayout.PropertyField(_onCollisionReaction, _onCollisionReactionLabel);
		EditorGUI.indentLevel++;
		_showMovementInfo = EditorGUILayout.Foldout(_showMovementInfo, "Movement Info", true);
		EditorGUI.indentLevel++;
		if (_showMovementInfo)
		{
			EditorGUILayout.PropertyField(_directionProperty, _directionLabel);
			if (component.IsMovementAccelerated())
			{
				component.SetGoalSpeed(Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(goalSpeedLabel, component.GetGoalSpeed()))));
				component.SetInterpolationTime(Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(interpolationTimeLabel, component.GetInterpolationTime()))));
				component.SetEasingFunction((EasingFunction.Ease)EditorGUILayout.EnumPopup(_easingFunctionLabel, component.GetEasingFunctionValue()));
				EditorGUI.indentLevel++;
				EditorGUILayout.LabelField("Easing Curve", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("X-axis: Time, Y-axis: Speed"); 
				DrawEasingCurve(component.GetEasingFunctionValue());
				EditorGUI.indentLevel--;
			}
			else
			{
				EditorGUILayout.PropertyField(_throw, _throwLabel);
				component.SetSpeed(Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(_constantSpeedLabel, component.GetSpeed()))));
			}
		}
        serializedObject.ApplyModifiedProperties();
        EditorGUI.indentLevel--;
		// If GUI changed we must applicate those changes in editor
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}
	}
}
