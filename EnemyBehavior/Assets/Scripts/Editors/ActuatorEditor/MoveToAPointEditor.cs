using UnityEditor;
using UnityEngine;
using static MoveToAPoint_Actuator;

[CustomEditor(typeof(MoveToAPoint_Actuator))]
public class MoveToAPoint_ActuatorEditor : ActuatorEditor
{
	private SerializedProperty waypointsData;
	private SerializedProperty isACicle;
	private SerializedProperty usageWay;
	private SerializedProperty randomArea;
	private SerializedProperty timeBetweenRandomPoints;
	private SerializedProperty seekPlayer;
	private SerializedProperty playerTransform;
	private SerializedProperty detectionDistance;
	private SerializedProperty reachingPlayerData;


	//Los GUIContent deben ir aqui y con su correspondiente descripcion
	private void OnEnable()
	{
		waypointsData = serializedObject.FindProperty("_waypointsData");
		isACicle = serializedObject.FindProperty("_isACicle");
		usageWay = serializedObject.FindProperty("_usageWay");
		randomArea = serializedObject.FindProperty("_randomArea");
		seekPlayer = serializedObject.FindProperty("_seekPlayer");
		detectionDistance = serializedObject.FindProperty("_detectionDistance");
		playerTransform = serializedObject.FindProperty("_playerTransform");
		reachingPlayerData = serializedObject.FindProperty("_reachingPlayerData");
		timeBetweenRandomPoints = serializedObject.FindProperty("_timeBetweenRandomPoints");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(usageWay, new GUIContent("Usage Way"));
		if (usageWay.intValue == 1)
		{
			EditorGUILayout.PropertyField(randomArea, new GUIContent("Random Area"));
			timeBetweenRandomPoints.floatValue = Mathf.Max(0, timeBetweenRandomPoints.floatValue);
			EditorGUILayout.PropertyField(timeBetweenRandomPoints, new GUIContent("Time between random points"));
		}
		else
		{
			EditorGUILayout.PropertyField(isACicle);
			EditorGUILayout.PropertyField(waypointsData, new GUIContent("Waypoints Data"), false);
			if (waypointsData.isExpanded)
			{
				EditorGUI.indentLevel++;
				waypointsData.arraySize = EditorGUILayout.IntField("Size", waypointsData.arraySize);

				for (int i = 0; i < waypointsData.arraySize; i++)
				{
					var waypoint = waypointsData.GetArrayElementAtIndex(i);
					EditorGUILayout.PropertyField(waypoint, new GUIContent("Waypoint " + i), false);
					if (waypoint.isExpanded)
					{
						EditorGUI.indentLevel++;

						var waypointTransform = waypoint.FindPropertyRelative("waypoint");
						EditorGUILayout.PropertyField(waypointTransform);


						var timeToReach = waypoint.FindPropertyRelative("timeToReach");
						timeToReach.floatValue = Mathf.Max(0, timeToReach.floatValue);
						EditorGUILayout.PropertyField(timeToReach);

						var isAccelerated = waypoint.FindPropertyRelative("isAccelerated");
						EditorGUILayout.PropertyField(isAccelerated);

						if (isAccelerated.boolValue)
						{
							var easingFunctionProp = waypoint.FindPropertyRelative("easingFunction");
							EditorGUILayout.PropertyField(easingFunctionProp, new GUIContent("Easing Function"));
							EasingFunction.Ease easingEnum = (EasingFunction.Ease)easingFunctionProp.intValue;
							DrawEasingCurve(easingEnum);
						}

						var shouldStop = waypoint.FindPropertyRelative("shouldStop");
						EditorGUILayout.PropertyField(shouldStop);
						if (shouldStop.boolValue)
						{
							var stopDuration = waypoint.FindPropertyRelative("stopDuration");
							stopDuration.floatValue = Mathf.Max(0f, EditorGUILayout.FloatField(new GUIContent("Stop Duration"), stopDuration.floatValue));
						}
						EditorGUI.indentLevel--;
					}
				}
				EditorGUI.indentLevel--;
			}
		}
		EditorGUILayout.PropertyField(seekPlayer);
		if (seekPlayer.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(reachingPlayerData, new GUIContent("Seeking Player Data"), false);
			if (reachingPlayerData.isExpanded)
			{
				EditorGUI.indentLevel++;
				detectionDistance.floatValue = Mathf.Max(0f, EditorGUILayout.FloatField(new GUIContent("Detection Distance"), detectionDistance.floatValue));
				EditorGUILayout.PropertyField(playerTransform, new GUIContent("Player Transform"));

				var timeToReach = reachingPlayerData.FindPropertyRelative("timeToReach");
				timeToReach.floatValue = Mathf.Max(0, timeToReach.floatValue);
				EditorGUILayout.PropertyField(timeToReach);

				var isAccelerated = reachingPlayerData.FindPropertyRelative("isAccelerated");
				EditorGUILayout.PropertyField(isAccelerated);

				if (isAccelerated.boolValue)
				{
					EditorGUI.indentLevel++;
					var easingFunctionProp = reachingPlayerData.FindPropertyRelative("easingFunction");
					EditorGUILayout.PropertyField(easingFunctionProp, new GUIContent("Easing Function"));
					EasingFunction.Ease easingEnum = (EasingFunction.Ease)easingFunctionProp.intValue;
					DrawEasingCurve(easingEnum);
				}

				var shouldStop = reachingPlayerData.FindPropertyRelative("shouldStop");
				EditorGUILayout.PropertyField(shouldStop);
				if (shouldStop.boolValue)
				{
					EditorGUI.indentLevel++;
					var stopDuration = reachingPlayerData.FindPropertyRelative("stopDuration");
					stopDuration.floatValue = Mathf.Max(0f, EditorGUILayout.FloatField(new GUIContent("Stop Duration"), stopDuration.floatValue));
				}
			}
		}
		serializedObject.ApplyModifiedProperties();
	}
}
