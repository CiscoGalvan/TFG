using UnityEditor;
using UnityEngine;
using TMPro;

[CustomEditor(typeof(Life))]
public class LifeEditor : Editor
{
   
    public override void OnInspectorGUI()
    {
        Life life = (Life)target;

        EditorGUILayout.LabelField("Life Settings", EditorStyles.boldLabel);
        life.SetInitialLife(EditorGUILayout.FloatField("Initial Life", life.GetInitialLife()));

        life.SetEntityType((Life.EntityType)EditorGUILayout.EnumPopup("Entity Type", life.GetEntityType()));

        if (life.GetEntityType() == Life.EntityType.Player)
        {
            EditorGUILayout.LabelField("UI Settings", EditorStyles.boldLabel);
            life.SetTextName(EditorGUILayout.TextField("Text Name", life.GetTextName()));
            life.SetLifeText((TextMeshProUGUI)EditorGUILayout.ObjectField("Life Text", life.GetLifeText(), typeof(TextMeshProUGUI), true));

        }

    }
}
