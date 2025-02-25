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
	private bool _ciclicWaypointAdded;

	private bool _randomPointReached = true;
	private Vector2 _currentRandomPoint;
	[SerializeField]
	private float _timeBetweenRandomPoints;

	[SerializeField]
	private bool _seekPlayer = false;

	private bool _seekingPlayer = false;

	[SerializeField]
	private float _detectionDistance = 0.0f;

	[SerializeField]
	private Transform _playerTransform;

	[SerializeField]
	private WaypointData _reachingPlayerData;

	//private Distance_Sensor _distanceSensor;
	public override void StartActuator()
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
		// Si en un inicio no era un ciclo y luego si, que vuelva a moverse.
		if (!_moving && _isACicle)
		{
			Debug.Log("Muevete");
			_moving = true;
			_currentWaypointIndex = 0;
		}
		if (!_moving)
			return;
		// Si se permite buscar al jugador y aún no se ha activado el modo persecución,
		// se verifica la distancia.
		if (_seekPlayer && _playerTransform != null && !_seekingPlayer)
		{
			float distanceToPlayer = Vector2.Distance(_rb.position, _playerTransform.position);
			if (distanceToPlayer <= _detectionDistance)
			{
				// Activamos la persecución de forma permanente.
				_seekingPlayer = true;
				// Reiniciamos los valores de interpolación para empezar a perseguir
				_startInterpolationPosition = _rb.position;
				_travelElapsedTime = 0f;
				_t = 0f;
			}
		}

		if (_seekingPlayer)
		{
			PursuePlayer();
		}
		else
		{
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
		}

		Vector2 newPosition = Vector2.Lerp(_startInterpolationPosition, targetPos, _t);
		_rb.MovePosition(newPosition);

		if (_t >= 1f && !waypoint.shouldStop)
		{
			AdvanceToNextWaypoint(targetPos);
		}
	}
	private void PursuePlayer()
	{
		if (_playerTransform == null)
			return;

		// Usamos los datos de _reachingPlayerData para configurar el movimiento hacia el jugador.
		float travelTime = _reachingPlayerData.timeToReach;
		_travelElapsedTime += Time.deltaTime;
		_t = _travelElapsedTime / travelTime;

		if (_reachingPlayerData.isAccelerated)
		{
			_t = EasingFunction.GetEasingFunction(_reachingPlayerData.easingFunction)(0, 1, _t);
		}

		// Siempre interpolamos desde la posición de inicio actual hacia la posición actual del jugador.
		Vector2 newPosition = Vector2.Lerp(_startInterpolationPosition, _playerTransform.position, _t);
		_rb.MovePosition(newPosition);

		// Cuando se complete la interpolación, reiniciamos los valores para la siguiente etapa
		if (_t >= 1f)
		{
			_startInterpolationPosition = _rb.position;
			_travelElapsedTime = 0f;
			_t = 0f;
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
				Debug.Log("Movimiento finalizado, todos los waypoints alcanzados.");
			}
		}
	}
	public override void DestroyActuator()
	{
		//_distanceSensor.onEventDetected -= SeekPlayer;
	}
}
