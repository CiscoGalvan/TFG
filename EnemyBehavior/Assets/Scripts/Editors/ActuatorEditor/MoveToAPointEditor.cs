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

	private bool _showWaypointsData= true;


	private static readonly GUIContent _usageWayLabel = new GUIContent("Usage Way", "How will the waypoints be set?\n" +
		"Random Area: A collider will be given and the waypoints will be generated inside it.\n" +
		"Waypoint: A sequence of waypoints will be given from start.\n");
	private static readonly GUIContent _randomAreaLabel = new GUIContent("Random Area", "Area that will describe where the next waypoints will be generated.");
	private static readonly GUIContent _timeBetweenWaypointsLabel = new GUIContent("Time between random points", "Time that will take to go from one point to another.");
	private static readonly GUIContent _seekPlayerLabel = new GUIContent("Seek Player", "Determines whether the enemy will chase the player if it is close enough.");
	private static readonly GUIContent _seekingPlayerDataLabel = new GUIContent("Seeking Player Data", "Group of values that will describe how the enemy behaves in case it seeks the player.");
	private static readonly GUIContent _detectionDistanceLabel = new GUIContent("Detection Distance", "Distance needed to trigger the seeking behaviour.");
	private static readonly GUIContent _playerTransformLabel = new GUIContent("Player Transform", "Reference to the player transform.");
	private static readonly GUIContent _timeToReachPlayerLabel = new GUIContent("Time To Reach The Player", "Time it takes to reach the player.");
	private static readonly GUIContent _isSeekingAcceleratedLabel = new GUIContent("Is Accelerated", "Is the movement towards the enemy accelerated?");
	private static readonly GUIContent _seekingEasingFunctionLabel = new GUIContent("Easing Function", "Function that defines how the enemy moves towards the player.");
	private static readonly GUIContent _shouldStopAfterSeekingLabel = new GUIContent("Should Stop", "Indicates whether the enemy should stop upon reaching the player.");
	private static readonly GUIContent _stopAfterSeekingDurationLabel = new GUIContent("Stop Duration", "Time it will take the enemy to start seeking the player again.");


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
		EditorGUILayout.PropertyField(usageWay, _usageWayLabel);
		if (usageWay.intValue == 1)
		{
			EditorGUILayout.PropertyField(randomArea, _randomAreaLabel);
			timeBetweenRandomPoints.floatValue = Mathf.Max(0, timeBetweenRandomPoints.floatValue);
			EditorGUILayout.PropertyField(timeBetweenRandomPoints, _timeBetweenWaypointsLabel);
		}
		else
		{
			EditorGUILayout.PropertyField(isACicle);
			_showWaypointsData = EditorGUILayout.Foldout(_showWaypointsData, "Waypoint Data", true);
			if (_showWaypointsData)
			{
				EditorGUI.indentLevel++;
				waypointsData.arraySize = EditorGUILayout.IntField("Size", waypointsData.arraySize);
				EditorGUI.indentLevel++;
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
		EditorGUILayout.PropertyField(seekPlayer, _seekPlayerLabel, false);
		if (seekPlayer.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(reachingPlayerData, _seekingPlayerDataLabel, false);
			if (reachingPlayerData.isExpanded)
			{
				EditorGUI.indentLevel++;
				detectionDistance.floatValue = Mathf.Max(0f, EditorGUILayout.FloatField(_detectionDistanceLabel, detectionDistance.floatValue));
				EditorGUILayout.PropertyField(playerTransform, _playerTransformLabel);

				var timeToReach = reachingPlayerData.FindPropertyRelative("timeToReach");
				timeToReach.floatValue = Mathf.Max(0, timeToReach.floatValue);
				EditorGUILayout.PropertyField(timeToReach,_timeToReachPlayerLabel);

				var isAccelerated = reachingPlayerData.FindPropertyRelative("isAccelerated");
				EditorGUILayout.PropertyField(isAccelerated, _isSeekingAcceleratedLabel);

				if (isAccelerated.boolValue)
				{
					EditorGUI.indentLevel++;
					var easingFunctionProp = reachingPlayerData.FindPropertyRelative("easingFunction");
					EditorGUILayout.PropertyField(easingFunctionProp, _seekingEasingFunctionLabel);
					EasingFunction.Ease easingEnum = (EasingFunction.Ease)easingFunctionProp.intValue;
					DrawEasingCurve(easingEnum);
				}

				var shouldStop = reachingPlayerData.FindPropertyRelative("shouldStop");
				EditorGUILayout.PropertyField(shouldStop, _shouldStopAfterSeekingLabel);
				if (shouldStop.boolValue)
				{
					EditorGUI.indentLevel++;
					var stopDuration = reachingPlayerData.FindPropertyRelative("stopDuration");
					stopDuration.floatValue = Mathf.Max(0f, EditorGUILayout.FloatField(_stopAfterSeekingDurationLabel, stopDuration.floatValue));
				}
			}
		}
		serializedObject.ApplyModifiedProperties();
	}
}
