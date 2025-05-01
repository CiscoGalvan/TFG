using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimatorManager))]
public class AnimatorManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("All sprites must be facing right.", MessageType.Info);
        DrawDefaultInspector();
    }
}