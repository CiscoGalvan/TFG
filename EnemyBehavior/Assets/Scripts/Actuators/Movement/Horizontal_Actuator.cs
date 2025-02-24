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
	private bool bounceAfterCollision = false;
    [SerializeField, HideInInspector]
    private bool destroyAfterCollision = false;
    [SerializeField]
	[HideInInspector]
    private float speed;

	[SerializeField]
	[HideInInspector]
	private float _goalSpeed;

	[SerializeField]
	[HideInInspector]
	private float _interpolationTime = 0;

	private Collision_Sensor collisionSensor;

	private float _initial_speed = 0;

    //[Tooltip("Object acceleration in units per second squared. Set to 0 for uniform motion")]
    
    //private float m_acceleration = 0f;

    [Tooltip("Movement direction")]
    [SerializeField]
    private Direction _direction = Direction.Left;

	private enum Direction
    {
        Left = -1,
        Right = 1
    }

    private float _time;
    Rigidbody2D _rigidbody;
    private EasingFunction.Function easingFunc;


	public override void StartActuator()
    {
        _rigidbody = this.GetComponent<Rigidbody2D>();
		easingFunc = EasingFunction.GetEasingFunction(m_easingFunction);
		if (bounceAfterCollision || destroyAfterCollision)
		{
            collisionSensor = this.GameObject().GetComponent<Collision_Sensor>();
            if (collisionSensor == null) //si no esta creado lo crea
            {
                collisionSensor = this.gameObject.AddComponent<Collision_Sensor>();
            }
            collisionSensor.onEventDetected += CollisionEvent;
			sensors.Add(collisionSensor);
        }
		
        _time = 0;
		if (m_isAccelerated)
		{
			speed = _rigidbody.velocity.x;
		}
		_initial_speed = speed;
		
	}
    public override void DestroyActuator()
    {
        if (collisionSensor != null)
        {
            collisionSensor.onEventDetected -= CollisionEvent;
        }
    }
	public override void UpdateActuator()
	{
		_time += Time.deltaTime;
		int dirValue = (int)_direction;
		if (!m_isAccelerated)
		{
			//MRU
			_rigidbody.velocity = new Vector2(speed * dirValue, _rigidbody.velocity.y);
		}
		else
		{
			//MRUA
			float t = (_time /_interpolationTime);
			float easedSpeed = easingFunc(_initial_speed, _goalSpeed, t);
			_rigidbody.velocity = new Vector2(easedSpeed * dirValue, _rigidbody.velocity.y);
			
			if (t >= 1.0f)
			{
				speed = _goalSpeed;
				_rigidbody.velocity = new Vector2(_goalSpeed * dirValue, _rigidbody.velocity.y);
			}
			else
			{
				speed = easedSpeed;
			}
		}
	}

	void CollisionEvent(Sensors s)
    {
		Debug.Log("bounce: " + bounceAfterCollision + "destroy:"+ destroyAfterCollision);
		
			Collision2D col = collisionSensor.GetCollidedObject();

			if (col == null) return;
			//comprobacion  de:
			// choque enemigo con mundo 
			//choque por izquierda o derecha
			if (col.gameObject.layer != LayerMask.NameToLayer("World")) return;
			ContactPoint2D contact = col.contacts[0];
			Vector2 normal = contact.normal;

			if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
			{
				if (bounceAfterCollision)
				{
                _direction = _direction == Direction.Left ? Direction.Right : Direction.Left;
				}
				else if (destroyAfterCollision)
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
        speed = newValue;
    }
    public float GetSpeed()
    {
        return speed;
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
		bounceAfterCollision = newValue;
	}
	public bool GetBouncesAfterCollision() => bounceAfterCollision;
    public void SetDestroyAfterCollision(bool newValue)
    {
        destroyAfterCollision = newValue;
    }
    public bool GetDestroyAfterCollision() => destroyAfterCollision;
    #endregion
}
