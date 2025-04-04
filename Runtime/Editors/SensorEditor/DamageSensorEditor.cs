using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[CustomEditor(typeof(DamageSensor))]
public class DamageSensorEditor : Editor
{
	//private static readonly GUIContent damageCooldown = new GUIContent("Damage Cooldown", "Amount of seconds it will take the player to receive damage again");
	//private static readonly GUIContent instaKill = new GUIContent("Instant Kill", "Will the damage eliminate the player ignoring it's current life?");
	//private static readonly GUIContent amountOfDamage= new GUIContent("Amount Of Damage","Amount of damage the enemy will deal the player");
	//public override void OnInspectorGUI()
	//{


	//	Damage_Sensor component = (Damage_Sensor)target;
	//	DrawDefaultInspector();

	//	switch (component.GetDamageType())
	//	{
	//		case Damage_Sensor.DamageType.Instant:
	//			component.SetInstaKill(EditorGUILayout.Toggle(instaKill, component.GetInstaKill()));
	//			if (!component.GetInstaKill())
	//			{
	//				component.SetAmountOfDamage(EditorGUILayout.FloatField(amountOfDamage, component.GetAmountOfDamage()));
	//			}
	//			break;
	//		case Damage_Sensor.DamageType.Persistent:
	//			component.SetAmountOfDamage(EditorGUILayout.FloatField(amountOfDamage, component.GetAmountOfDamage()));
	//			component.SetDamageCooldown(EditorGUILayout.FloatField(damageCooldown, component.GetDamageCooldown()));
	//			break;
	//		case Damage_Sensor.DamageType.Residual:
	//			// Diferenciar entre daño por contacto y daño residual.
	//			break;
	//	}

	//	// If GUI changed we must applicate those changes in editor
	//	if (GUI.changed)
	//	{
	//		EditorUtility.SetDirty(component);
	//	}


	//}
}
