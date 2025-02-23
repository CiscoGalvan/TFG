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
	[SerializeField,HideInInspector]
	private bool _bounceAfterCollision = false;
	[SerializeField]
	[HideInInspector]
    private float m_speed;

	[SerializeField]
	[HideInInspector]
	private float m_goalSpeed;

	[SerializeField]
	[HideInInspector]
	private float m_interpolationTime = 0;

	private Collision_Sensor collisionSensor;

	private float m_initial_speed = 0;

    //[Tooltip("Object acceleration in units per second squared. Set to 0 for uniform motion")]
    
    //private float m_acceleration = 0f;

    [Tooltip("Movement direction")]
    [SerializeField]
    private Direction m_direction = Direction.Left;

	private enum Direction
    {
        Left = -1,
        Right = 1
    }

    private float m_time;
    Rigidbody2D m_rigidbody;
    private EasingFunction.Function easingFunc;

	[SerializeField]
	private bool _destroyAfterCollision = false;
	public override void StartActuator()
    {
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        easingFunc = EasingFunction.GetEasingFunction(m_easingFunction);
        collisionSensor = this.GameObject().GetComponent<Collision_Sensor>();
		if(collisionSensor != null)
		{
            collisionSensor.onEventDetected += CollisionEvent;
        }
        m_time = 0;
		if (m_isAccelerated)
		{
			m_speed = m_rigidbody.velocity.x;
		}
		m_initial_speed = m_speed;
		
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
		m_time += Time.deltaTime;
		int dirValue = (int)m_direction;
		if (!m_isAccelerated)
		{
			//MRU
			m_rigidbody.velocity = new Vector2(m_speed * dirValue, m_rigidbody.velocity.y);
		}
		else
		{
			//MRUA
			float t = (m_time / m_interpolationTime);
			float easedSpeed = easingFunc(m_initial_speed, m_goalSpeed, t);
			m_rigidbody.velocity = new Vector2(easedSpeed * dirValue, m_rigidbody.velocity.y);
			
			if (t >= 1.0f)
			{
				m_speed = m_goalSpeed;
				m_rigidbody.velocity = new Vector2(m_goalSpeed * dirValue, m_rigidbody.velocity.y);
			}
			else
			{
				m_speed = easedSpeed;
			}
		}
	}

	void CollisionEvent(Sensors s)
    {
		if (_bounceAfterCollision)
		{
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
				m_direction = m_direction == Direction.Left ? Direction.Right : Direction.Left;
			}

		}
		else if (_destroyAfterCollision)
		{
			Destroy(this.gameObject);
		}
	}

    private void OnDrawGizmosSelected()
    {
        if (! this.isActiveAndEnabled) return;

        Gizmos.color = Color.green;
        Vector3 position = transform.position;

        Vector3 direction = new Vector3((int)m_direction, 0, 0);

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
        m_speed = newValue;
    }
    public float GetSpeed()
    {
        return m_speed;
    }
	public void SetGoalSpeed(float newValue)
	{
		m_goalSpeed = newValue;
	}
	public float GetGoalSpeed()
	{
		return m_goalSpeed;
	}
	public void SetInterpolationTime(float newValue)
	{
		m_interpolationTime = newValue;
	}
	public float GetInterpolationTime()
	{
		return m_interpolationTime;
	}

	public void SetBouncesAfterCollision(bool newValue)
	{
		_bounceAfterCollision = newValue;
	}
	public bool GetBouncesAfterCollision() => _bounceAfterCollision;
	#endregion
}
