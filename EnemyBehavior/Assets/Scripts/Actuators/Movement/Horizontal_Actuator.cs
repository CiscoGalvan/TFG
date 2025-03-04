using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Horizontal_Actuator : Movement_Actuator
{
	//private static readonly GUIContent bouncingLabel = new GUIContent("Bounce Object", "Does the object bounce after collision?");
	//private static readonly GUIContent destroyLabel = new GUIContent("Destroy Object", "Does the object self-destruct after the collision?");

	private enum Direction
	{
		Left = -1,
		Right = 1
	}
	public enum OnCollisionReaction
	{
		None = 0,
		Bounce = 1,
		Destroy = 2
	}
	//private enum MovementType
	//{
	//	Force = 0,
	//	Lineal = 1,
	//	Accelerated = 2
	//}
	// ¿Qué pasa si el disenhador setea esto a true y en ejecución lo cambia a false?
	//[SerializeField, HideInInspector]
	//private bool _bounceAfterCollision = false;
    //[SerializeField, HideInInspector]
    //private bool _destroyAfterCollision = false;
    [SerializeField,HideInInspector]
    private float _speed;
	[SerializeField,HideInInspector]
	private float _goalSpeed;

	[SerializeField, HideInInspector]
	private float _interpolationTime = 0;

	private Collision_Sensor _collisionSensor;
    [SerializeField]
    private bool _throw; //if this is activated the velocity will be update just ones


    private float _initial_speed = 0;

    [Tooltip("Movement direction")]
    [SerializeField,HideInInspector]
    private Direction _direction = Direction.Left;


	[SerializeField, HideInInspector]
	private OnCollisionReaction _onCollisionReaction = OnCollisionReaction.None;



	private float _time;
    private Rigidbody2D _rigidbody;
    private EasingFunction.Function _easingFunc;


	public override void StartActuator()
    {
        _rigidbody = this.GetComponent<Rigidbody2D>();
		_easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
		if (_onCollisionReaction == OnCollisionReaction.Bounce ||_onCollisionReaction == OnCollisionReaction.Destroy)
		{
            _collisionSensor = this.GameObject().GetComponent<Collision_Sensor>();
            if (_collisionSensor == null) //si no esta creado lo crea
            {
                _collisionSensor = this.gameObject.AddComponent<Collision_Sensor>();
            }
            _collisionSensor.onEventDetected += CollisionEvent;
			sensors.Add(_collisionSensor);
        }
		
        _time = 0;
		if (_isAccelerated)
		{
			_speed = _rigidbody.velocity.x;
		}
		_initial_speed = _speed;
        if (_throw) ApplyForce();

    }
    public override void DestroyActuator()
    {
        if (_collisionSensor != null)
        {
            _collisionSensor.onEventDetected -= CollisionEvent;
        }
    }
	public override void UpdateActuator()
	{
        if (!_throw) ApplyForce();
	}
	private void ApplyForce()
	{ 
		_time += Time.deltaTime;
		int dirValue = (int)_direction;
		if (!_isAccelerated)
		{
			//MRU
			_rigidbody.velocity = new Vector2(_speed * dirValue, _rigidbody.velocity.y);
		}
		else
		{
			//MRUA
			float t = (_time /_interpolationTime);
			float easedSpeed = _easingFunc(_initial_speed, _goalSpeed, t);
			_rigidbody.velocity = new Vector2(easedSpeed * dirValue, _rigidbody.velocity.y);
			
			if (t >= 1.0f)
			{
				_speed = _goalSpeed;
				_rigidbody.velocity = new Vector2(_goalSpeed * dirValue, _rigidbody.velocity.y);
			}
			else
			{
				_speed = easedSpeed;
			}
		}
	}
	void CollisionEvent(Sensors s)
    {
		Collision2D col = _collisionSensor.GetCollidedObject();

		if (col == null) return;
		//comprobacion  de:
		// choque enemigo con mundo 
		//choque por izquierda o derecha
		if (col.gameObject.layer != LayerMask.NameToLayer("World") && col.gameObject.layer != LayerMask.NameToLayer("Player")) return;
		ContactPoint2D contact = col.contacts[0];
		Vector2 normal = contact.normal;

		if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
		{
			if (_onCollisionReaction == OnCollisionReaction.Bounce)
			{
				_direction = _direction == Direction.Left ? Direction.Right : Direction.Left;
			}
			else if (_onCollisionReaction == OnCollisionReaction.Destroy)
			{
				Destroy(this.gameObject);
			}
		}
		
	}

    private void OnDrawGizmosSelected()
    {
        if (! this.isActiveAndEnabled) return;

        Gizmos.color = Color.green;
        Vector3 position = transform.position;

        Vector3 direction = new Vector3((int)_direction, 0, 0);

        // Draw direction arrow
        Gizmos.DrawLine(position, position + direction );
        Vector3 arrowTip = position + direction;
        Vector3 right = Quaternion.Euler(0, 0, 135) * direction * 0.25f;
        Vector3 left = Quaternion.Euler(0, 0, -135) * direction * 0.25f;
        Gizmos.DrawLine(arrowTip, arrowTip + right);
        Gizmos.DrawLine(arrowTip, arrowTip + left);
       
    }
  

	#region Setters and Getters 
    public void SetSpeed(float newValue)
    {
        _speed = newValue;
    }
    public float GetSpeed()
    {
        return _speed;
    }
	public void SetGoalSpeed(float newValue)
	{
		_goalSpeed = newValue;
	}
	public float GetGoalSpeed()
	{
		return _goalSpeed;
	}
	public void SetInterpolationTime(float newValue)
	{
		_interpolationTime = newValue;
	}
	public float GetInterpolationTime()
	{
		return _interpolationTime;
	}
    #endregion
}
