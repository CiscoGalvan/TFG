using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Distance_Sensor))]
public class DistanteSensorEditor : Editor
{



    private static readonly GUIContent goalSpeedLabel = new GUIContent("X-axis", "If true, measures along the X-axis; otherwise, measures along the Y-axis");
    public override void OnInspectorGUI()
    {


        Distance_Sensor component = (Distance_Sensor)target;
        DrawDefaultInspector();

        if (!component.GetUseMagnitude()) //if magnitude is false then chek axis
        {
            component.SetCheckXAxis(EditorGUILayout.Toggle(goalSpeedLabel, component.GetCheckXAxis()));
           
        }
       
        // If GUI changed we must applicate those changes in editor
        if (GUI.changed)
        {
            EditorUtility.SetDirty(component);
        }


    }
}
