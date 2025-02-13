using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Horizontal : Actuator
{
	//Tenemos que serializar y esconder en el inspector toda variable que querramos que sea cambiada con un editor pero que esta no aparezca desde un principio en el inspector.

	[SerializeField]
	[HideInInspector]
    private float m_speed;

	[SerializeField]
	[HideInInspector]
	private float m_goalSpeed;

	[SerializeField]
	[HideInInspector]
	private float m_interpolationTime = 0;



	private float m_initial_speed = 0;

    //[Tooltip("Object acceleration in units per second squared. Set to 0 for uniform motion")]
    
    //private float m_acceleration = 0f;

    [Tooltip("Movement direction")]
    [SerializeField]
    private Direction m_direction = Direction.Left;

    [SerializeField]
    private List<Sensors> m_eventsToReact;

	private enum Direction
    {
        Left = -1,
        Right = 1
    }

    private float m_time;
    Rigidbody2D m_rigidbody;
    private EasingFunction.Function easingFunc;

	public override void StartActuator()
    {
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        easingFunc = EasingFunction.GetEasingFunction(m_easingFunction);
        //Collision.OnCollisionSensor += CollisionEvent;
        m_time = 0;
		if (m_isAccelerated)
		{
			m_speed = m_rigidbody.velocity.x;
		}
		m_initial_speed = m_speed;
		foreach (var sensor in m_eventsToReact)
		{
            sensor.onEventDetected += CollisionEvent;
		}
	}
    public override void DestroyActuator()
    {
		//Collision.OnCollisionSensor -= CollisionEvent;
		foreach (var sensor in m_eventsToReact)
		{
			sensor.onEventDetected -= CollisionEvent;
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

	void CollisionEvent()
    {
        m_direction = m_direction == Direction.Left ? Direction.Right : Direction.Left;
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

	public List<Sensors> GetSensors()
	{
		return m_eventsToReact;
	}
	#endregion
}
