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
    private static readonly GUIContent _followPlayerLabel = new GUIContent("Follow Player", "Direction of the horizontal movement will be the nearest to the player");
    private static readonly GUIContent _layersToCollideLabel = new GUIContent("Layers To Collide", "Layers that will activate the Reaction After Collision event in case there is a collision.");
    private bool _showMovementInfo = true;

    #region Accelerated movement
    private static readonly GUIContent goalSpeedLabel = new GUIContent("Goal Speed", "Speed the object will reach");
    private static readonly GUIContent interpolationTimeLabel = new GUIContent("Interpolation Time", "Time it takes to reach Max Speed");
    #endregion

    #region  Non-accelerated movement
    private static readonly GUIContent _throwLabel = new GUIContent("Throw", "The object will be moved only once, when the actuator is activated.");
    private static readonly GUIContent _constantSpeedLabel = new GUIContent("Speed", "The object will move with this constant speed.");
    #endregion

    private SerializedProperty _followPlayerProperty;
    private SerializedProperty _directionProperty;
    private SerializedProperty _onCollisionReaction;
    private SerializedProperty _throw;
    private SerializedProperty _layersToCollide;

    private void OnEnable()
    {
        _followPlayerProperty = serializedObject.FindProperty("_followPlayer");
        _directionProperty = serializedObject.FindProperty("_direction");
        _onCollisionReaction = serializedObject.FindProperty("_onCollisionReaction");
        _throw = serializedObject.FindProperty("_throw");
        _layersToCollide = serializedObject.FindProperty("_layersToCollide");
    }

    public override void OnInspectorGUI()
    {
        HorizontalActuator component = (HorizontalActuator)target;
        DrawDefaultInspector();

        EditorGUILayout.PropertyField(_onCollisionReaction, _onCollisionReactionLabel);
        if (_onCollisionReaction.intValue != 0)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_layersToCollide, _layersToCollideLabel);
            EditorGUI.indentLevel--;
        }

        _showMovementInfo = EditorGUILayout.Foldout(_showMovementInfo, "Movement Info", true);
        EditorGUI.indentLevel++;
        if (_showMovementInfo)
        {
            EditorGUILayout.PropertyField(_followPlayerProperty, _followPlayerLabel);
            if (!_followPlayerProperty.boolValue)
            {
                EditorGUILayout.PropertyField(_directionProperty, _directionLabel);
            }


            if (_followPlayerProperty.boolValue && _onCollisionReaction.intValue == 1) // 1 = Bounce
            {
                EditorGUILayout.HelpBox("The object won't bounce off collisions while following the player.", MessageType.Warning);
            }

            if (component.IsMovementAccelerated())
            {
                component.SetGoalSpeed(Mathf.Max(0, EditorGUILayout.FloatField(goalSpeedLabel, component.GetGoalSpeed())));
                component.SetInterpolationTime(Mathf.Max(0, EditorGUILayout.FloatField(interpolationTimeLabel, component.GetInterpolationTime())));
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Easing Curve", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("X-axis: Time, Y-axis: Speed");
                DrawEasingCurve(component.GetEasingFunctionValue());
                EditorGUI.indentLevel--;
            }
            else
            {
                EditorGUILayout.PropertyField(_throw, _throwLabel);
                component.SetSpeed(Mathf.Max(0, EditorGUILayout.FloatField(_constantSpeedLabel, component.GetSpeed())));
            }
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(component);
        }
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
