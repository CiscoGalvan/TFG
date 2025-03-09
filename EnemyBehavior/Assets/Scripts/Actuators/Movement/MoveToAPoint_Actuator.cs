using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MoveToAPoint_Actuator;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Rigidbody2D))]
[ExecuteInEditMode]
public class MoveToAPoint_Actuator : Movement_Actuator
{
	const float ALMOST_REACHED_ONE = 0.999f;
	[System.Serializable]
	public struct WaypointData
	{
		public Transform waypoint;
		public float timeToReach;
		public bool isAccelerated;
		public bool shouldStop;      // Indica si se debe detener en este waypoint
		[HideInInspector]
		public float stopDuration;   // Tiempo de parada en segundos
		[SerializeField, HideInInspector]
		public EasingFunction.Ease easingFunction;
	}
	public enum UsageWay
	{
		Waypoint = 0,
		RandomArea = 1
	};

	#region Values whether every Waypoint have same behaviour
	[SerializeField, HideInInspector]
	private float _timeToReachForAllWaypoints;
	[SerializeField,HideInInspector]
	private bool _areAccelerated;
	[SerializeField, HideInInspector]
	private bool _shouldThemStop;
	[SerializeField, HideInInspector]
	private float _stopTime;
	[SerializeField, HideInInspector]
	private EasingFunction.Ease _easingFunctionForAllWaypoints;
	#endregion
	[SerializeField]
	private UsageWay _usageWay;
	[Tooltip("Configure each waypoint with its time, acceleration, and easing function")]
	[SerializeField] private List<WaypointData> _waypointsData = new List<WaypointData>();
	[SerializeField] private Collider2D _randomArea;

	private Rigidbody2D _rb;
	private bool _moving;


	private float _travelElapsedTime;
	private float _stopElapsedTime;



	private Vector2 _startInterpolationPosition;
	private float _t;
	private int _currentWaypointIndex;


	[SerializeField]
	private bool _isACicle = false;
	[SerializeField,HideInInspector]
	private bool _allWaypointsHaveTheSameData = false;

	[SerializeField, HideInInspector]
	private bool _ciclicWaypointAdded;

	private bool _randomPointReached = true;
	private Vector2 _currentRandomPoint;
	[SerializeField]
	private float _timeBetweenRandomPoints;

	

	[SerializeField]
	private float _detectionDistance = 0.0f;

	public override void StartActuator(Animator _animator)
	{
		_rb = GetComponent<Rigidbody2D>();
		_travelElapsedTime = 0f;
		_stopElapsedTime = 0f;
		_t = 0f;
		_currentWaypointIndex = 0;

		if (_usageWay == UsageWay.Waypoint && (_waypointsData == null || _waypointsData.Count == 0))
		{
			Debug.LogError($"MoveToAPoint_Actuator error in {name}: No waypoints were set.");
			UnityEditor.EditorApplication.isPlaying = false;
		}
		else
		{
			_startInterpolationPosition = _rb.position;
			_moving = true;
		}

		if (_usageWay == UsageWay.RandomArea)
		{
			_randomArea.isTrigger = true;
		}

		
		//if (_seekPlayer)
		//{
		//	_distanceSensor = this.GameObject().GetComponent<Distance_Sensor>();
		//	if (_distanceSensor == null) //si no esta creado lo crea
		//	{
		//		_distanceSensor = this.gameObject.AddComponent<Distance_Sensor>();
		//	}
		//	_distanceSensor.SetDetectionDistance(_detectionDistance);
		//	_distanceSensor.SetTarget(_playerTransform.gameObject);
		//	_distanceSensor.onEventDetected += SeekPlayer;
		//	sensors.Add(_distanceSensor);
		//}
	}

	public override void UpdateActuator()
	{
		
		if (!_moving && _isACicle)
		{
			_moving = true;
			_currentWaypointIndex = 0;
		}
		if (!_moving)
			return;
		switch (_usageWay)
		{
			case UsageWay.Waypoint:
				MoveAlongWaypoints();
				break;
			case UsageWay.RandomArea:
				MoveToRandomPoint();
				break;
		}
	}
	private void MoveToRandomPoint()
	{
		if (_randomArea == null)
		{
			Debug.LogError("Random area not set!");
			return;
		}

		if (_randomPointReached)
		{
			_currentRandomPoint = new Vector2(
				Random.Range(_randomArea.bounds.min.x, _randomArea.bounds.max.x),
				Random.Range(_randomArea.bounds.min.y, _randomArea.bounds.max.y)
			);
			_randomPointReached = false;
		}

		//revisar estos valores
		WaypointData randomWaypoint = new WaypointData
		{
			waypoint = null,
			timeToReach = _timeBetweenRandomPoints,
			isAccelerated = false,
			shouldStop = false,
			stopDuration = Random.Range(0.5f, 2f),
			easingFunction = EasingFunction.Ease.EaseInOutQuad
		};

		MoveTowardsTarget(randomWaypoint, _currentRandomPoint);
	}
	private void MoveAlongWaypoints()
	{
		if (_currentWaypointIndex >= _waypointsData.Count)
			return;

		WaypointData currentWaypoint = _waypointsData[_currentWaypointIndex];
		Vector2 targetPos = currentWaypoint.waypoint.position;
		MoveTowardsTarget(currentWaypoint, targetPos);
	}
	private void MoveTowardsTarget(WaypointData waypoint, Vector2 targetPos)
	{
		if (_t >= 1f && waypoint.shouldStop)
		{
			_stopElapsedTime += Time.deltaTime;
			if (_stopElapsedTime >= waypoint.stopDuration)
			{
				AdvanceToNextWaypoint(targetPos);
			}
			return;
		}

		_travelElapsedTime += Time.deltaTime;
		_t = _travelElapsedTime / waypoint.timeToReach;

	

		if (waypoint.isAccelerated)
		{
			_t = EasingFunction.GetEasingFunction(waypoint.easingFunction)(0, 1, _t);
			if (_t >= ALMOST_REACHED_ONE)
				_t = 1f;
		}

		Vector2 newPosition = Vector2.Lerp(_startInterpolationPosition, targetPos, _t);
		_rb.MovePosition(newPosition);

		if (_t >= 1f && !waypoint.shouldStop)
		{
			AdvanceToNextWaypoint(targetPos);
		}
	}

	private void AdvanceToNextWaypoint(Vector2 reachedPos)
	{
		
		_travelElapsedTime = 0f;
		_stopElapsedTime = 0f;
		_t = 0f;
		_startInterpolationPosition = reachedPos;
		_currentWaypointIndex++;

		if (_usageWay == UsageWay.RandomArea)
		{
			_randomPointReached = true;
		}
		else if (_currentWaypointIndex >= _waypointsData.Count)
		{
			if (_isACicle)
			{
				_currentWaypointIndex = 0;
			}
			else
			{
				_moving = false;
				_rb.velocity = Vector2.zero;
			}
		}
	}
	public override void DestroyActuator()
	{
		//_distanceSensor.onEventDetected -= SeekPlayer;
	}
	private void OnDrawGizmos()
	{
		switch (_usageWay)
		{
			case UsageWay.RandomArea:
				
				
					Gizmos.color = Color.blue;

					Gizmos.DrawSphere(_currentRandomPoint, 0.2f);
				
				break;
			case UsageWay.Waypoint:
				if (_waypointsData.Count > 0 && _currentWaypointIndex < _waypointsData.Count)
				{
					Transform currentWaypoint = _waypointsData[_currentWaypointIndex].waypoint;

					if (currentWaypoint != null)
					{
						Gizmos.color = Color.blue;

						Gizmos.DrawSphere(currentWaypoint.position, 0.2f);
					}
				}
				break;
		}
		
	}
}
