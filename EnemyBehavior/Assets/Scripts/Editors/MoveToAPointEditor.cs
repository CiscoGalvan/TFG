using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MoveToAPoint))]
public class MoveToAPointEditor : Editor
{
	//Poner en ingles y revisar lo escrito que no lo he mirado muy bien todo.
	private static readonly GUIContent accelerationLabel = new GUIContent("Acceleration Value", "Define la aceleración del objeto en movimiento.");
	private static readonly GUIContent easingFunctionLabel = new GUIContent("Easing Function", "Funcion de easing");

	public override void OnInspectorGUI()
	{

		MoveToAPoint component = (MoveToAPoint)target;

		DrawDefaultInspector();

		if (component.IsMovementAccelerated())
		{
			component.SetEasingFunction((EasingFunction.Ease)EditorGUILayout.EnumPopup(easingFunctionLabel, component.GetEasingFunctionValue()));
		}
		// If GUI changed we must applicate those changes in editor
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}
	}
}
