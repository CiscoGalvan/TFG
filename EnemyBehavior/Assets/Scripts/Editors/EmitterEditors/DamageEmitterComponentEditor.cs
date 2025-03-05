using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static MoveToAPoint_Actuator;

[CustomEditor(typeof(DamageEmitter))]
public class DamageEmitterComponentEditor : Editor
{
	private SerializedProperty _damageType;

	private static readonly GUIContent _amountOfDamage = new GUIContent("Damage Amount", "Amount of damage the enemy will deal the player.");
	private static readonly GUIContent _damageCooldown = new GUIContent("Damage Cooldown", "Amount of seconds it will take the player to receive damage again.");
	private static readonly GUIContent _damageTypeLabel = new GUIContent("Damage Type", "How will the damage be dealt?\n" +
		"Instant: The damage will be applied instantly.\n" +
		"Persistent: The damage will be applied while the enemy is in contact with the player.\n" +
		"Residual: There will be a part of the damage applied instantly and another part will be in delivered \"applications\".");
	#region Instant Damage Labels
	private static readonly GUIContent instaKill = new GUIContent("Instant Kill", "Will the damage eliminate the player ignoring it's current life?");
	#endregion
	#region Residual Damage Labels
	private static readonly GUIContent residualDamageLabel = new GUIContent("Residual Damage Amount", "Damage taken by the player after the initial collision");
	private static readonly GUIContent numberOfTicks = new GUIContent("Number Of Applications", "Times the residual damage will be applied");
	private static readonly GUIContent instantDamageAmount = new GUIContent("Instant Damage Amount", "Amount of damage the enemy will deal the player when they collide");
	#endregion

	private bool _showDamageInfo = true;

	private void OnEnable()
	{
		_damageType = serializedObject.FindProperty("_damageType");
	}
	public override void OnInspectorGUI()
	{
		DamageEmitter component = (DamageEmitter)target;
		DrawDefaultInspector();
		serializedObject.Update();

		EditorGUILayout.PropertyField(_damageType, _damageTypeLabel);
		EditorGUI.indentLevel++;
		_showDamageInfo = EditorGUILayout.Foldout(_showDamageInfo, "Damage Info", true);
		if (_showDamageInfo)
		{
			EditorGUI.indentLevel++;
			switch (component.GetDamageType())
			{
				case DamageEmitter.DamageType.Instant:
					component.SetInstaKill(EditorGUILayout.Toggle(instaKill, component.GetInstaKill()));
					if (!component.GetInstaKill())
					{
						component.SetAmountOfDamage(EditorGUILayout.FloatField(_amountOfDamage, component.GetAmountOfDamage()));
					}
					break;
				case DamageEmitter.DamageType.Persistent:
					component.SetAmountOfDamage(EditorGUILayout.FloatField(_amountOfDamage, component.GetAmountOfDamage()));
					component.SetDamageCooldown(EditorGUILayout.FloatField(_damageCooldown, component.GetDamageCooldown()));
					break;
				case DamageEmitter.DamageType.Residual:
					component.SetAmountOfDamage(EditorGUILayout.FloatField(instantDamageAmount, component.GetAmountOfDamage()));
					component.SetResidualDamageAmount(EditorGUILayout.FloatField(residualDamageLabel, component.GetResidualDamageAmount()));
					component.SetDamageCooldown(EditorGUILayout.FloatField(_damageCooldown, component.GetDamageCooldown()));
					component.SetNumberOfResidualApplication(EditorGUILayout.IntField(numberOfTicks, component.GetNumberOfResidualApplication()));
					break;
			}
			EditorGUI.indentLevel--; 
		}

		
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}
		serializedObject.ApplyModifiedProperties();

	}
}
