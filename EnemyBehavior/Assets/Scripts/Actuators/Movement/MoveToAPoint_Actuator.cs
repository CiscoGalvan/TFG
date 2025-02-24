using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[ExecuteInEditMode] 

public class MoveToAPoint_Actuator : Movement_Actuator
{


	[Tooltip("Time until the object speed reaches the position it moves to")]
	[SerializeField]
	private float _timeUntilReachingPosition = 0f;

	[Tooltip("The position the object moves towards")]
	[SerializeField]
	private Transform _objectivePosition;
	private Rigidbody2D _rb;
	private bool _moving;
	private float _elapsedTime;
	private Vector2 _startPos;
	private float _t;
	public override void StartActuator()
	{
		_rb = GetComponent<Rigidbody2D>();
		_elapsedTime = 0f;
		_moving = false;
		if (_objectivePosition != null)
		{
			_startPos = _rb.position;
			_moving = true;
		}
	
	}

	// Update is called once per frame
	public override void UpdateActuator()
	{
		if (!_moving || _objectivePosition == null) return;

		#region Movement by time
		Vector2 targetPos = _objectivePosition.position;
		_elapsedTime += Time.deltaTime;
		_t = _elapsedTime / _timeUntilReachingPosition;
		if (_isAccelerated)
		{
			// Aplicamos una función de easing
			_t = EasingFunction.GetEasingFunction(_easingFunction)(0, 1, _t);
		}
		Vector2 newPosition = Vector2.Lerp(_startPos, targetPos, _t);
		_rb.MovePosition(newPosition);

		// Si llegamos al destino, detenemos el movimiento
		if (_t >= 1f)
		{
			_moving = false;
			_rb.velocity = Vector2.zero;
		}
		#endregion
		#region Movement by velocities
		#endregion
	}
	private void Start()
	{
		if (_objectivePosition == null)
		{
			Debug.LogError($"There was an error in GameObject{name}, script MoveToAPoint: You have to give 'm_objectivePos' a value");
			UnityEditor.EditorApplication.isPlaying = false;
		}
	}
	public override void DestroyActuator()
	{
		Debug.Log(_t);
	}
	#region Setters and Getters
	#endregion

	//Dibuja la misma flecha que otros comportamientos 
	//private void OnDrawGizmosSelected()
	//{
	//	if (!this.isActiveAndEnabled) return;

	//	Gizmos.color = Color.green;
	//	Vector2 position = new Vector2(transform.position.x,transform.position.y);
	//	Vector2 objectivePos = new Vector2(m_objectivePosition.position.x, m_objectivePosition.position.y);

	//	Vector2 direction = (objectivePos - position).normalized;

	//	// Draw direction arrow
	//	Gizmos.DrawLine(position, position + direction);
	//	Vector3 arrowTip = position + direction;
	//	Vector3 right = Quaternion.Euler(0, 0, 135) * direction * 0.25f;
	//	Vector3 left = Quaternion.Euler(0, 0, -135) * direction * 0.25f;
	//	Gizmos.DrawLine(arrowTip, arrowTip + right);
	//	Gizmos.DrawLine(arrowTip, arrowTip + left);
	//}
}
