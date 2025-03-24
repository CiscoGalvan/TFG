using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static MoveToAPointActuator;

[CustomEditor(typeof(DamageEmitter))]
public class DamageEmitterComponentEditor : Editor
{
	private SerializedProperty _damageType;
	private SerializedProperty _damageEmitterCollider;
	private SerializedProperty _moreThanOneCollider;
	private SerializedProperty _destroyAfterDoingDamage;

	private static readonly GUIContent _amountOfDamageLabel = new GUIContent("Damage Amount", "Amount of damage the enemy will deal the player.");
	private static readonly GUIContent _moreThanOneColliderLabel = new GUIContent("More Than One Collider?", "If the object has more than one collider, it is needed to specify which one will be the damage emitter.\n" +
		"If it has more than one collider and this value is not set to true, the behaviour may not be accurate.");
	private static readonly GUIContent _destroyAfterDoingDamageLabel = new GUIContent("Destroy After Doing Damage", "Will the object destroy after doing damage?");
	private static readonly GUIContent _damageEmitterColliderLabel = new GUIContent("Damage Zone", "The collider that will deal damage to the player in case they make contact");
	private static readonly GUIContent _damageCooldownLabel = new GUIContent("Damage Cooldown", "Amount of seconds it will take the player to receive damage again.");
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
		_damageEmitterCollider = serializedObject.FindProperty("_damageEmitterCollider");
		_moreThanOneCollider = serializedObject.FindProperty("_moreThanOneCollider");
		_destroyAfterDoingDamage = serializedObject.FindProperty("_destroyAfterDoingDamage");
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
			EditorGUILayout.PropertyField(_moreThanOneCollider, _moreThanOneColliderLabel);
			
			if (_moreThanOneCollider.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(_damageEmitterCollider, _damageEmitterColliderLabel);
				EditorGUI.indentLevel--;
			}
			switch (component.GetDamageType())
			{
				case DamageEmitter.DamageType.Instant:
                    EditorGUILayout.PropertyField(_destroyAfterDoingDamage, _destroyAfterDoingDamageLabel);
                    component.SetInstaKill(EditorGUILayout.Toggle(instaKill, component.GetInstaKill()));
					if (!component.GetInstaKill())
					{
						EditorGUI.indentLevel++;
						component.SetAmountOfDamage(EditorGUILayout.FloatField(_amountOfDamageLabel, component.GetAmountOfDamage()));
						EditorGUI.indentLevel--;
					}
					break;
				case DamageEmitter.DamageType.Persistent:
					component.SetAmountOfDamage(EditorGUILayout.FloatField(_amountOfDamageLabel, component.GetAmountOfDamage()));
					component.SetDamageCooldown(EditorGUILayout.FloatField(_damageCooldownLabel, component.GetDamageCooldown()));
					break;
				case DamageEmitter.DamageType.Residual:
                    EditorGUILayout.PropertyField(_destroyAfterDoingDamage, _destroyAfterDoingDamageLabel);
                    component.SetAmountOfDamage(EditorGUILayout.FloatField(instantDamageAmount, component.GetAmountOfDamage()));
					component.SetResidualDamageAmount(EditorGUILayout.FloatField(residualDamageLabel, component.GetResidualDamageAmount()));
					component.SetDamageCooldown(EditorGUILayout.FloatField(_damageCooldownLabel, component.GetDamageCooldown()));
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
