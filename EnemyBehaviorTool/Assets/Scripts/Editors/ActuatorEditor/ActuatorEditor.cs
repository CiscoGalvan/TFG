using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class ActuatorEditor : Editor
{
	protected const int EASING_GRAPH_WIDTH = 100;
	protected const int EASING_GRAPH_HEIGHT = 250;
	protected const int EASING_GRAPH_NUMBER_OF_POINTS = 20;

	protected static readonly GUIContent _easingFunctionLabel = new GUIContent("Easing Function", "Easing function that will describe the progress of the velocity");
	protected AnimationCurve easingCurve = new AnimationCurve();
	protected void DrawEasingCurve(EasingFunction.Ease easing)
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

		// Then we draw the easing curve.
		Rect curveRect = GUILayoutUtility.GetRect(EASING_GRAPH_WIDTH, EASING_GRAPH_HEIGHT, GUILayout.ExpandWidth(true));
		EditorGUI.CurveField(curveRect, easingCurve);
	}
}
