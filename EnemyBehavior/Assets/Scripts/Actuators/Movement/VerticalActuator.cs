using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Rigidbody2D))]

public class VerticalActuator : MovementActuator
{
	private enum Direction
	{
		Down = -1,
		Up = 1
	}
	public enum OnCollisionReaction
	{
		None = 0,
		Bounce = 1,
		Destroy = 2
	}

    [SerializeField, HideInInspector]
	private float _speed;
    [SerializeField, HideInInspector]
    private bool _throw; //if this is activated the velocity will be update just ones

    [SerializeField]
	[HideInInspector]
	private float _goalSpeed;


	[SerializeField]
	[HideInInspector]
	private float _interpolationTime = 0;
    private CollisionSensor _collisionSensor;

    private float _initial_speed = 0;
   
    [Tooltip("Movement direction")]
    [SerializeField,HideInInspector]
    private Direction _direction = Direction.Up;

    

    private float _time;
    private Rigidbody2D _rigidbody;

    private EasingFunction.Function _easingFunc;

	[SerializeField, HideInInspector]
	private OnCollisionReaction _onCollisionReaction = OnCollisionReaction.None;

    // public event Action OnBounce; // Evento para notificar el rebote
    //public event Action OnDestroy; // Evento para notificar el rebote
    AnimatorManager _animatorManager;
    public override void StartActuator()
    {
        _animatorManager = this.gameObject.GetComponent<AnimatorManager>();
        _rigidbody = this.GetComponent<Rigidbody2D>();
        _easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
        _collisionSensor = this.GameObject().GetComponent<CollisionSensor>();
		if (_onCollisionReaction == OnCollisionReaction.Bounce || _onCollisionReaction == OnCollisionReaction.Destroy)
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
            _speed = GetComponent<Rigidbody2D>().velocity.x;
        }
        _initial_speed = _speed;
        if (_throw) ApllyForce();
        if (_animatorManager != null)
        {
            _animatorManager.ChangeSpeedY(_initial_speed);
            if (_direction == Direction.Up)
                _animatorManager.UpDirection();
            else
                _animatorManager.DownDirection();
        }
    }
    public override void DestroyActuator()
    {
        if (_collisionSensor != null)
        {
            _collisionSensor.onEventDetected += CollisionEvent;
        }
    }

    public override void UpdateActuator()
    {
       
        if(!_throw) ApllyForce();

    }
    private void ApllyForce()
    {
        _time += Time.deltaTime;
        int dirValue = (int)_direction;
        if (!_isAccelerated)
        {
            //MRU
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _speed * dirValue);
          
        }
        else
        {
            //MRUA
            float t = (_time / _interpolationTime);
            float easedSpeed = _easingFunc(_initial_speed, _goalSpeed, t);
			if (t >= 1.0f)
            {
                Debug.Break();
                _speed = _goalSpeed;
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _goalSpeed * dirValue);
            }
            else
            {
				_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, easedSpeed * dirValue);
				_speed = easedSpeed;
            }
            if (_animatorManager != null) _animatorManager.ChangeSpeedY(_rigidbody.velocity.y);
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

        if (Mathf.Abs(normal.x) < Mathf.Abs(normal.y))
        {

			if (_onCollisionReaction == OnCollisionReaction.Bounce)
			{
                bool correctCollision = (_direction == Direction.Up && normal.y < 0) || (_direction == Direction.Down && normal.y > 0);
                if (correctCollision) {
                    _direction = _direction == Direction.Up ? Direction.Down : Direction.Up;
                    if (_animatorManager != null)
                    {
                        _animatorManager.RotateSpriteY();
                        if (_direction == Direction.Up)
                            _animatorManager.UpDirection();
                        else
                            _animatorManager.DownDirection();
                    }
                }
                
               
                //OnBounce?.Invoke();
            }
			else if (_onCollisionReaction == OnCollisionReaction.Destroy)
			{
                // Destroy(this.gameObject);
                if (_animatorManager != null) _animatorManager.Destroy();
				else
					Destroy(this.gameObject);
                //OnDestroy?.Invoke();
            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled || !_debugActuator) return;

        Gizmos.color = Color.green;
        Vector3 position = transform.position;

        Vector3 dir = new Vector3(0, (int)_direction, 0);

        // Draw direction arrow
        Gizmos.DrawLine(position, position + dir);
        Vector3 arrowTip = position + dir;
        Vector3 right = Quaternion.Euler(0, 0, 135) * dir * 0.25f;
        Vector3 left = Quaternion.Euler(0, 0, -135) * dir * 0.25f;
        Gizmos.DrawLine(arrowTip, arrowTip + right);
        Gizmos.DrawLine(arrowTip, arrowTip + left);
    }

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
}
