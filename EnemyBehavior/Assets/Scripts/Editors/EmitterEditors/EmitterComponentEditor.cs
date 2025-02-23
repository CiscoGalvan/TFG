using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(DamageEmitter))]
public class EmitterComponentEditor : Editor
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


	public override void OnInspectorGUI()
	{


		DamageEmitter component = (DamageEmitter)target;
		DrawDefaultInspector();

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
				// Diferenciar entre da�o por contacto y da�o residual.
				component.SetAmountOfDamage(EditorGUILayout.FloatField(instantDamageAmount, component.GetAmountOfDamage()));
				component.SetResidualDamageAmount(EditorGUILayout.FloatField(residualDamageLabel, component.GetResidualDamageAmount()));
				component.SetDamageCooldown(EditorGUILayout.FloatField(damageCooldown, component.GetDamageCooldown()));
				component.SetNumberOfResidualApplication(EditorGUILayout.IntField(numberOfTicks, component.GetNumberOfResidualApplication()));
				break;
		}

		// If GUI changed we must applicate those changes in editor
		if (GUI.changed)
		{
			EditorUtility.SetDirty(component);
		}


	}
}
