using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static MoveToAPoint_Actuator;

[CustomEditor(typeof(DamageEmitter))]
public class DamageEmitterComponentEditor : Editor
{
	private static readonly GUIContent amountOfDamage = new GUIContent("Damage Amount", "Amount of damage the enemy will deal the player");
	private static readonly GUIContent damageCooldown = new GUIContent("Damage Cooldown", "Amount of seconds it will take the player to receive damage again");
	#region Instant Damage Labels
	private static readonly GUIContent instaKill = new GUIContent("Instant Kill", "Will the damage eliminate the player ignoring it's current life?");
	#endregion
	#region Residual Damage Labels
	private static readonly GUIContent residualDamageLabel = new GUIContent("Residual Damage Amount", "Damage taken by the player after the initial collision");
	private static readonly GUIContent numberOfTicks = new GUIContent("Number of Applications", "Times the residual damage will be applied");
	private static readonly GUIContent instantDamageAmount = new GUIContent("Instant Damage Amount", "Amount of damage the enemy will deal the player when they collide");
	#endregion


	private bool _showDamageInfo = true;


	public override void OnInspectorGUI()
	{
		DamageEmitter component = (DamageEmitter)target;
		DrawDefaultInspector();

		EditorGUI.indentLevel++;
		_showDamageInfo = EditorGUILayout.Foldout(_showDamageInfo, "Damage Info", true);
		if (_showDamageInfo)
		{
			switch (component.GetDamageType())
			{
				case DamageEmitter.DamageType.Instant:
					component.SetInstaKill(EditorGUILayout.Toggle(instaKill, component.GetInstaKill()));
					if (!component.GetInstaKill())
					{
						component.SetAmountOfDamage(EditorGUILayout.FloatField(amountOfDamage, component.GetAmountOfDamage()));
					}
					break;
				case DamageEmitter.DamageType.Persistent:
					component.SetAmountOfDamage(EditorGUILayout.FloatField(amountOfDamage, component.GetAmountOfDamage()));
					component.SetDamageCooldown(EditorGUILayout.FloatField(damageCooldown, component.GetDamageCooldown()));
					break;
				case DamageEmitter.DamageType.Residual:
					component.SetAmountOfDamage(EditorGUILayout.FloatField(instantDamageAmount, component.GetAmountOfDamage()));
					component.SetResidualDamageAmount(EditorGUILayout.FloatField(residualDamageLabel, component.GetResidualDamageAmount()));
					component.SetDamageCooldown(EditorGUILayout.FloatField(damageCooldown, component.GetDamageCooldown()));
					component.SetNumberOfResidualApplication(EditorGUILayout.IntField(numberOfTicks, component.GetNumberOfResidualApplication()));
					break;
			}
			EditorGUI.indentLevel--; 
		}

		
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}


	}
}
