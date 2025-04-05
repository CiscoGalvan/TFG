using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CustomEditor(typeof(Directional_Actuator))]
public class DirectionalComponentEditor : ActuatorEditor
{
	private static readonly GUIContent _onCollisionReactionLabel = new GUIContent("Reaction After Collision", "What will the object do after collision?\n" +
		"None: The object will not react to the collision.\n" +
		"Bounce: The object will bounce to the opposite direction.\n" +
		"Destroy: The object will be destroyed after the contact.");

	private static readonly GUIContent _speedLabel = new GUIContent("Speed", "The object will move with this constant speed.");
	private static readonly GUIContent _goalSpeedLabel = new GUIContent("Goal Speed", "Speed the object will reach after the acceleration.");
	private static readonly GUIContent _interpolationTimeLabel = new GUIContent("Interpolation Time", "Time needed to reach the goal speed.");
	private static readonly GUIContent _angleLabel = new GUIContent("Angle", "Launch angle [0,360]");
	private static readonly GUIContent _isAcceleratedLabel = new GUIContent("Is Accelerated", "Is the object movement accelerated?");
	private static readonly GUIContent _throwLabel = new GUIContent("Throw", "The object will be moved only once, when the actuator is activated.");
	private static readonly GUIContent _aimPlayerLabel = new GUIContent("Aim Player", "The object will move towards player direction.");
    private static readonly GUIContent _layerMaskLabel = new GUIContent("Collision LayerMask", "Select the layers that the object will interact with");

    private SerializedProperty _onCollisionReaction;
	private SerializedProperty _speed;
	private SerializedProperty _isAccelerated;
	private SerializedProperty _goalSpeed;
	private SerializedProperty _interpolationTime;
	private SerializedProperty _angle;
	private SerializedProperty _throw;
	private SerializedProperty _easingFunction;
	private SerializedProperty _aimPlayer;
    private SerializedProperty _layerMask;

	private bool _showMovementInfo = true;
	private void OnEnable()
	{
		_onCollisionReaction = serializedObject.FindProperty("_onCollisionReaction");
		_speed = serializedObject.FindProperty("_speed");
		_isAccelerated = serializedObject.FindProperty("_isAccelerated");
		_goalSpeed = serializedObject.FindProperty("_goalSpeed");
		_interpolationTime = serializedObject.FindProperty("_interpolationTime");
		_angle = serializedObject.FindProperty("_angle");
		_throw = serializedObject.FindProperty("_throw");
		_easingFunction = serializedObject.FindProperty("_easingFunction");
		_aimPlayer = serializedObject.FindProperty("_aimPlayer");
        _layerMask = serializedObject.FindProperty("_layersToCollide");
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.PropertyField(_layerMask, _layerMaskLabel);
		EditorGUILayout.PropertyField(_onCollisionReaction, _onCollisionReactionLabel);
		EditorGUI.indentLevel++;
		_showMovementInfo = EditorGUILayout.Foldout(_showMovementInfo, "Movement Info", true);
		EditorGUI.indentLevel++;
		if (_showMovementInfo)
		{
			EditorGUILayout.PropertyField(_isAccelerated, _isAcceleratedLabel);
			EditorGUILayout.PropertyField(_aimPlayer, _aimPlayerLabel);
			if (!_aimPlayer.boolValue)
			{
				EditorGUI.indentLevel++;
				_angle.floatValue = Mathf.Clamp(EditorGUILayout.FloatField(_angleLabel, _angle.floatValue), 0, 360);
				EditorGUI.indentLevel--;
			}
			if (_isAccelerated.boolValue)
			{
				_goalSpeed.floatValue = Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(_goalSpeedLabel, _goalSpeed.floatValue)));
				_interpolationTime.floatValue = Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(_interpolationTimeLabel, _interpolationTime.floatValue)));
				EditorGUILayout.PropertyField(_easingFunction, _easingFunctionLabel);
				EasingFunction.Ease easingEnum = (EasingFunction.Ease)_easingFunction.intValue;
				DrawEasingCurve(easingEnum);
			}
			else
			{
				EditorGUILayout.PropertyField(_throw, _throwLabel);
				_speed.floatValue = Mathf.Max(0, Mathf.Max(0, EditorGUILayout.FloatField(_speedLabel, _speed.floatValue)));
			}



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
        Vector2 yLabelPos = new Vector2(curveRect.xMin + 30, curveRect.yMin - 2);
        GUI.Label(new Rect(yLabelPos, new Vector2(60, 20)), "Y: Speed ", labelStyle);
    }
}
