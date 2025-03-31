using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(State))]
public class StateComponentEditor : Editor
{
	public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
	}
}
