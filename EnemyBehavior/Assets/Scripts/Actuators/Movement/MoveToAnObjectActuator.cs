using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using static MoveToAPointActuator;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Rigidbody2D))]
[ExecuteInEditMode]
public class MoveToAnObjectActuator : MovementActuator
{
	const float ALMOST_REACHED_ONE = 0.999f;

	[System.Serializable]
	public struct WaypointData
	{
		public Transform waypoint;
		public float timeToReach;
		public bool isAccelerated;
		public bool shouldStop;      // Indica si se debe detener en este waypoint
		//[HideInInspector]
		//public float stopDuration;   // Tiempo de parada en segundos
		[SerializeField, HideInInspector]
		public EasingFunction.Ease easingFunction;
	}

	[SerializeField, Tooltip("Configura el waypoint con su tiempo, aceleración y función de easing")]
	private WaypointData _waypointData;

	private Rigidbody2D _rb;
	private bool _moving;

	private float _travelElapsedTime;
	private float _stopElapsedTime;
	private float _t;

	private Vector2 _startInterpolationPosition;

	public override void StartActuator()
	{
		_actuatorActive = true;
		_rb = GetComponent<Rigidbody2D>();
		_travelElapsedTime = 0f;
		_stopElapsedTime = 0f;
		_t = 0f;
		_moving = true;

		if (_waypointData.waypoint == null)
		{
			Debug.LogError($"MoveToAnObject error in {name}: No se ha asignado ningún waypoint.");
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
		else
		{
			_startInterpolationPosition = _rb.position;
		}
        AnimatorManager _animatorManager = this.gameObject.GetComponent<AnimatorManager>();
        if (_animatorManager != null) _animatorManager.Follow();
    }

	public override void UpdateActuator()
	{
		if (!_moving)
			return;

		MoveTowardsTarget(_waypointData, _waypointData.waypoint.position);
	}
	private void MoveTowardsTarget(WaypointData waypoint, Vector2 targetPos)
	{
		//// Si ya se llegó y se debe detener, acumula el tiempo de parada
		//if (_t >= 1f && waypoint.shouldStop)
		//{
		//	_stopElapsedTime += Time.deltaTime;
		//	if (_stopElapsedTime >= waypoint.stopDuration)
		//	{
		//		_moving = false;
		//	}
		//	return;
		//}

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

		// Una vez alcanzado el objetivo sin requerir parada, detenemos el movimiento
		if ((_t >= 1f && !waypoint.shouldStop) || waypoint.waypoint == null)
		{
			_moving = false;
		}
	}

	
	public override void DestroyActuator()
	{
		_actuatorActive = false;
	}
	private void OnDrawGizmos()
	{
		if (!_debugActuator) return;
		if (_waypointData.waypoint != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(_waypointData.waypoint.position, 0.2f);
		}
	}
}