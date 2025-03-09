using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(State))]
public class StateComponentEditor : Editor
{
	private SerializedProperty _sensorTransitions;
	private bool _showSensorTransitions = true;

	private void OnEnable()
	{
		_sensorTransitions = serializedObject.FindProperty("_sensorTransitions");
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		serializedObject.Update();

		// Mostrar lista de transiciones de sensores
		_showSensorTransitions = EditorGUILayout.Foldout(_showSensorTransitions, "Sensor Transitions", true);
		if (_showSensorTransitions)
		{
			EditorGUI.indentLevel++;
			_sensorTransitions.arraySize = EditorGUILayout.IntField("Number of Transitions", _sensorTransitions.arraySize);

			// Iterar sobre las transiciones de sensores
			for (int i = 0; i < _sensorTransitions.arraySize; i++)
			{
				var transition = _sensorTransitions.GetArrayElementAtIndex(i);

				// Agregar un foldout para cada transición de sensor
				bool isExpanded = EditorGUILayout.Foldout(true, "Transition " + i);
				if (isExpanded)
				{
					EditorGUI.indentLevel++;

					// Sensor
					var sensorProp = transition.FindPropertyRelative("sensor");
					EditorGUILayout.PropertyField(sensorProp, new GUIContent("Sensor"));

					if (sensorProp.objectReferenceValue != null)
					{
						var sensor = sensorProp.objectReferenceValue as Sensors;
						if (sensor != null)
						{
							//switch

							if (sensor is Collision_Sensor collisionSensor)
							{
								// Usamos SerializedObject para la clase derivada Collision_Sensor
								SerializedObject sensorSerializedObject = new SerializedObject(collisionSensor);
								var subscriberTypeProp = sensorSerializedObject.FindProperty("_subscriberType");

								if (subscriberTypeProp != null)
								{
									EditorGUILayout.PropertyField(subscriberTypeProp, new GUIContent("Event Type"));
									sensorSerializedObject.ApplyModifiedProperties();
								}
								//// Transition event
								//var transitionEventProp = transition.FindPropertyRelative("transitionEvent");
								//EditorGUILayout.PropertyField(transitionEventProp, new GUIContent("Transition Event"));
							}
							// Agregar más comprobaciones si tienes otras clases derivadas de Sensors
							// Ejemplo: else if (sensor is OtroSensor otroSensor) { ... }
						}
					}
					// Target state
					var targetStateProp = transition.FindPropertyRelative("targetState");
					EditorGUILayout.PropertyField(targetStateProp, new GUIContent("Target State"));

					EditorGUI.indentLevel--;
				}
			}
			EditorGUI.indentLevel--;
		}

		serializedObject.ApplyModifiedProperties();
	}
}
