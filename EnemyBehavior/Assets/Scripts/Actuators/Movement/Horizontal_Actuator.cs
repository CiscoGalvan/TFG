using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Horizontal_Actuator : Movement_Actuator
{

	// ¿Qué pasa si el disenhador setea esto a true y en ejecución lo cambia a false?
	[SerializeField, HideInInspector]
	private bool _bounceAfterCollision = false;
    [SerializeField, HideInInspector]
    private bool _destroyAfterCollision = false;
    [SerializeField,HideInInspector]
    private float _speed;
	[SerializeField,HideInInspector]
	private float _goalSpeed;

	[SerializeField, HideInInspector]
	private float _interpolationTime = 0;

	private Collision_Sensor _collisionSensor;

	private float _initial_speed = 0;

    [Tooltip("Movement direction")]
    [SerializeField]
    private Direction _direction = Direction.Left;

	private enum Direction
    {
        Left = -1,
        Right = 1
    }

    private float _time;
    private Rigidbody2D _rigidbody;
    private EasingFunction.Function _easingFunc;


	public override void StartActuator()
    {
        _rigidbody = this.GetComponent<Rigidbody2D>();
		_easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
		if (_bounceAfterCollision || _destroyAfterCollision)
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
		Debug.Log("bounce: " + _bounceAfterCollision + "destroy:"+ _destroyAfterCollision);
		
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
			if (_bounceAfterCollision)
			{
            _direction = _direction == Direction.Left ? Direction.Right : Direction.Left;
			}
			else if (_destroyAfterCollision)
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

	public void SetBouncesAfterCollision(bool newValue)
	{
		_bounceAfterCollision = newValue;
	}
	public bool GetBouncesAfterCollision() => _bounceAfterCollision;
    public void SetDestroyAfterCollision(bool newValue)
    {
        _destroyAfterCollision = newValue;
    }
    public bool GetDestroyAfterCollision() => _destroyAfterCollision;
    #endregion
}
