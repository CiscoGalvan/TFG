using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HorizontalActuator : MovementActuator
{

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

    [SerializeField,HideInInspector]
    private float _speed;
	[SerializeField,HideInInspector]
	private float _goalSpeed;

	[SerializeField, HideInInspector]
	private float _interpolationTime = 0;

	private CollisionSensor _collisionSensor;
    [SerializeField, HideInInspector]
    private bool _throw; //if this is activated the velocity will be update just ones


    private float _initialSpeed = 0;

    [Tooltip("Movement direction")]
    [SerializeField,HideInInspector]
    private Direction _direction = Direction.Left;


	[SerializeField, HideInInspector]
	private OnCollisionReaction _onCollisionReaction = OnCollisionReaction.None;



	private float _time;
    private Rigidbody2D _rigidbody;
    private EasingFunction.Function _easingFunc;
    AnimatorManager _animatorManager;

    public override void StartActuator()
    {
		_actuatorActive = true;
        _animatorManager = this.gameObject.GetComponent<AnimatorManager>();
        _rigidbody = this.GetComponent<Rigidbody2D>();
		_easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
		if (_onCollisionReaction == OnCollisionReaction.Bounce ||_onCollisionReaction == OnCollisionReaction.Destroy)
		{
            _collisionSensor = this.GameObject().GetComponent<CollisionSensor>();
            if (_collisionSensor == null) //si no esta creado lo crea
            {
                _collisionSensor = this.gameObject.AddComponent<CollisionSensor>();
            }
            _collisionSensor.onEventDetected += CollisionEvent;
			sensors.Add(_collisionSensor);
        }
		
        _time = 0;
		if (_isAccelerated)
		{
			_speed = _rigidbody.velocity.x;
		}
		_initialSpeed = _speed;
        if (_throw) ApplyForce();
        if (_animatorManager != null)
        {
            _animatorManager.ChangeSpeedX(_speed * (int)_direction);
            if (_direction == Direction.Left)
                _animatorManager.LeftDirection();
            else
                _animatorManager.RightDirection();
        }
    }
    public override void DestroyActuator()
    {
		_actuatorActive = false;
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
			float easedSpeed = _easingFunc(_initialSpeed, _goalSpeed, t);

			if (t >= 1.0f)
			{
				_speed = _goalSpeed;
				_rigidbody.velocity = new Vector2(_goalSpeed * dirValue, _rigidbody.velocity.y);
			}
			else
			{
				_rigidbody.velocity = new Vector2(easedSpeed * dirValue, _rigidbody.velocity.y);
				_speed = easedSpeed;
			}
           _animatorManager?.ChangeSpeedX(_rigidbody.velocity.x);
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
                bool hitFromCorrectSide = (_direction == Direction.Left && normal.x > 0) || (_direction == Direction.Right && normal.x < 0);
                if (hitFromCorrectSide)
                {
                    _direction = _direction == Direction.Left ? Direction.Right : Direction.Left;
					 
					if (_animatorManager.enabled)
					{
						_animatorManager?.RotateSpriteX();
						if (_direction == Direction.Left)
							_animatorManager?.LeftDirection();
						else
							_animatorManager?.RightDirection();
					}
                }
              
            }
			else if (_onCollisionReaction == OnCollisionReaction.Destroy)
			{
				if (_animatorManager != null || !_animatorManager.enabled) _animatorManager.Destroy();
				else Destroy(this.gameObject);

            }
		}
		
	}
    
    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled || !_debugActuator) return;

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
    public bool GetBouncing()
    {
        return _onCollisionReaction == OnCollisionReaction.Bounce;
    }

    public bool GetDestroying()
    {
        return _onCollisionReaction == OnCollisionReaction.Destroy;
    }
    #endregion
}
