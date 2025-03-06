using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Rigidbody2D))]

public class Vertical_Actuator : Movement_Actuator
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
	//[SerializeField, HideInInspector]
 //   private bool _bounceAfterCollision = false;
 //   [SerializeField, HideInInspector]
 //   private bool _destroyAfterCollision = false;

    [SerializeField, HideInInspector]
	private float _speed;
    [SerializeField]
    private bool _throw; //if this is activated the velocity will be update just ones

    [SerializeField]
	[HideInInspector]
	private float _goalSpeed;


	[SerializeField]
	[HideInInspector]
	private float _interpolationTime = 0;
    private Collision_Sensor _collisionSensor;

    private float _initial_speed = 0;
   
    [Tooltip("Movement direction")]
    [SerializeField,HideInInspector]
    private Direction _direction = Direction.Up;

    

    private float _time;
    private Rigidbody2D _rigidbody;

    private EasingFunction.Function _easingFunc;

	[SerializeField, HideInInspector]
	private OnCollisionReaction _onCollisionReaction = OnCollisionReaction.None;
	public override void StartActuator()
    {
        _rigidbody = this.GetComponent<Rigidbody2D>();
        _easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
        _collisionSensor = this.GameObject().GetComponent<Collision_Sensor>();
		if (_onCollisionReaction == OnCollisionReaction.Bounce || _onCollisionReaction == OnCollisionReaction.Destroy)
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
            _speed = GetComponent<Rigidbody2D>().velocity.x;
        }
        _initial_speed = _speed;
        if (_throw) ApllyForce();
      //  UpdateActuator2();

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
                _direction = _direction == Direction.Up ? Direction.Down : Direction.Up;
            }
			else if (_onCollisionReaction == OnCollisionReaction.Destroy)
			{
                Destroy(this.gameObject);
            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled) return;

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
}
