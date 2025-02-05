using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MoveToAPoint))]
public class MoveToAPointEditor : Editor
{
	const int EASING_GRAPH_WIDTH = 100;
	const int EASING_GRAPH_HEIGHT = 250;
	const int EASING_GRAPH_NUMBER_OF_POINTS= 20;
	//Poner en ingles y revisar lo escrito que no lo he mirado muy bien todo.
	private static readonly GUIContent accelerationLabel = new GUIContent("Acceleration Value", "Define la aceleración del objeto en movimiento.");
	private static readonly GUIContent easingFunctionLabel = new GUIContent("Easing Function", "Funcion de easing");
	private AnimationCurve easingCurve = new AnimationCurve(); // Curva de easing

	public override void OnInspectorGUI()
	{
		
		MoveToAPoint component = (MoveToAPoint)target;

		DrawDefaultInspector();

		if (component.IsMovementAccelerated())
		{
			component.SetEasingFunction((EasingFunction.Ease)EditorGUILayout.EnumPopup(easingFunctionLabel, component.GetEasingFunctionValue()));
			UpdateEasingCurve(component.GetEasingFunctionValue());
			Rect curveRect = GUILayoutUtility.GetRect(EASING_GRAPH_WIDTH, EASING_GRAPH_HEIGHT, GUILayout.ExpandWidth(true)); 
			EditorGUI.CurveField(curveRect, easingCurve);

		}
		// If GUI changed we must applicate those changes in editor
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}
	}
	private void UpdateEasingCurve(EasingFunction.Ease easing)
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
	}
}
