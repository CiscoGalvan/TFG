using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(State))]
public class StateComponentEditor : Editor
{
    private Editor[] actuatorEditors;

    public override void OnInspectorGUI()
    {
        State component = (State)target;

        DrawDefaultInspector();

        if (component.actuatorList != null)
        {
            // Asegurarse de que el array de editores tiene el tamaño correcto
            if (actuatorEditors == null || actuatorEditors.Length != component.actuatorList.Count)
            {
                actuatorEditors = new Editor[component.actuatorList.Count];
            }

            for (int i = 0; i < component.actuatorList.Count; i++)
            {
                if (component.actuatorList[i] == null)
                    continue;

                // Crear un editor si aún no existe
                if (actuatorEditors[i] == null)
                {
                    actuatorEditors[i] = Editor.CreateEditor(component.actuatorList[i]);
                }

                // Dibujar el editor del actuador
                if (actuatorEditors[i] != null)
                {
                    EditorGUILayout.LabelField($"Actuator {i + 1}", EditorStyles.boldLabel);
                    actuatorEditors[i].OnInspectorGUI();
                    EditorGUILayout.Space();
                }
            }
        }
    }
}
