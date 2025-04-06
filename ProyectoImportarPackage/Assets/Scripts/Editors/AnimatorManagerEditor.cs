using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimatorManager))]
public class AnimatorManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AnimatorManager animatorManager = (AnimatorManager)target;

       
        EditorGUILayout.HelpBox("Todos los sprites deben estar orientados hacia la derecha.", MessageType.Info);
        DrawDefaultInspector();
    }
}