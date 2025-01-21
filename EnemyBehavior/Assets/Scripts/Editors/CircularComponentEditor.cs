using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Circular))]
public class CircularComponentEditor : Editor
{
	public override void OnInspectorGUI()
	{
		
		Circular component = (Circular)target;

		DrawDefaultInspector();

		if (component.GetRotationPoint() == null || !component.RotationPointAssigned())
		{
			component.SetRadius(EditorGUILayout.Slider("Radio", component.GetRadius(), 0, float.MaxValue));
		}

		// If GUI changed we must applicate those changes in editor
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}
	}
}
