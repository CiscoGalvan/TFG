using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MoveToAPointActuator;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Rigidbody2D))]
[ExecuteInEditMode]
public class MoveToAPointActuator : MovementActuator
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
	[SerializeField, HideInInspector]
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
	[SerializeField, HideInInspector]
	private bool _allWaypointsHaveTheSameData = false;

	[SerializeField, HideInInspector]
	private bool _ciclicWaypointAdded;

	private bool _randomPointReached = true;
	private Vector2 _currentRandomPoint;
	[SerializeField]
	private float _timeBetweenRandomPoints;
	private List<Vector2> _cachedWaypointPositions = new List<Vector2>();
	AnimatorManager _animatorManager;
	Vector2 _previousPosition;

    public override void StartActuator()
	{
		_actuatorActive = true;
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
			
			if (_usageWay == UsageWay.Waypoint)
			{
				foreach (var waypoint in _waypointsData)
				{
					if (waypoint.waypoint != null)
						_cachedWaypointPositions.Add(waypoint.waypoint.position);
					else
						_cachedWaypointPositions.Add(Vector2.zero);
				}
			}
			_startInterpolationPosition = _rb.position;
			_moving = true;
		}

		if (_usageWay == UsageWay.RandomArea)
		{
			_randomArea.isTrigger = true;
		}
        _animatorManager = this.GetComponent<AnimatorManager>();
        _previousPosition = _rb.position;
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
		if (_currentWaypointIndex >= _cachedWaypointPositions.Count)
			return;

		Vector2 targetPos = _cachedWaypointPositions[_currentWaypointIndex];
		WaypointData currentWaypoint = _waypointsData[_currentWaypointIndex];
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

        // Comparar posiciones para determinar la dirección del movimiento
        if (newPosition.x < _previousPosition.x)
        {
            _animatorManager.XLeftChangeAndFlip();
        }
        else if (newPosition.x > _previousPosition.x)
        {
            _animatorManager.XRightChangeAndFlip();
        }

        // Guardar la posición actual para la próxima comparación
        _previousPosition = newPosition;

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
		_actuatorActive = false;
	}
	private void OnDrawGizmos()
	{
		if (!_debugActuator) return;
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
