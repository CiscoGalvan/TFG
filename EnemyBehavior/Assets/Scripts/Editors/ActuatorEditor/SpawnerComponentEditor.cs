using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Spawner_Actuator))]
public class SpawnerComponentEditor : Editor
{
	private static readonly GUIContent numEnemiesLabel = new GUIContent("Amount Of Enemies", "Amount of enemies the spawner will spawn");
	public override void OnInspectorGUI()
	{

		Spawner_Actuator component = (Spawner_Actuator)target;

		DrawDefaultInspector();

		if (!component.GetInfiniteEnemies())
		{
			component.SetNumberOfEnemiesToSpawn(Mathf.Max(0, Mathf.Max(0, EditorGUILayout.IntField(numEnemiesLabel, component.GetNumberOfEnemiesToSpawn()))));
		}
		// If GUI changed we must applicate those changes in editor
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}
	}
}
