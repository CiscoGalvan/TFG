using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Rigidbody2D))]

public class Vertical : Actuator
{

	[SerializeField]
	[HideInInspector]
	private float m_speed;


	[SerializeField]
	[HideInInspector]
	private float m_goalSpeed;


	[SerializeField]
	[HideInInspector]
	private float m_interpolationTime = 0;


	[Tooltip("Movement direction")]
    [SerializeField]
    private Direction m_direction = Direction.Down;


	private float m_initial_speed = 0;

	private enum Direction
    {
        Down = -1,
        Up = 1
    }

    private float m_time = 0;
    private EasingFunction.Function easingFunc;
    private Rigidbody2D m_rigidbody;
    public override void StartActuator()
    {
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        easingFunc = EasingFunction.GetEasingFunction(m_easingFunction);
		m_time = 0;
		if (m_isAccelerated)
		{
			m_speed = m_rigidbody.velocity.x;
		}
		m_initial_speed = m_speed;
		//Collision.OnCollisionSensor += ReceiveMessage;
	}
    public override void DestroyActuator()
    {
        //Collision.OnCollisionSensor -= ReceiveMessage;
    }

    public override void UpdateActuator()
    {
		m_time += Time.deltaTime;
		int dirValue = (int)m_direction;
		if (!m_isAccelerated)
		{
			//MRU
			m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x,m_speed * dirValue);
		}
		else
		{
			//MRUA
			float t = (m_time / m_interpolationTime);
			float easedSpeed = easingFunc(m_initial_speed, m_goalSpeed, t);
			m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, easedSpeed * dirValue);

			if (t >= 1.0f)
			{
				m_speed = m_goalSpeed;
				m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_goalSpeed * dirValue);
			}
			else
			{
				m_speed = easedSpeed;
			}
		}
	}
    //void ReceiveMessage(Collision2D mensaje)
    //{

    //    m_direction = m_direction == Direction.Down ? Direction.Up: Direction.Down;
    //}
    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled) return;

        Gizmos.color = Color.green;
        Vector3 position = transform.position;

        Vector3 direction = new Vector3(0, (int)m_direction, 0);

        // Draw direction arrow
        Gizmos.DrawLine(position, position + direction);
        Vector3 arrowTip = position + direction;
        Vector3 right = Quaternion.Euler(0, 0, 135) * direction * 0.25f;
        Vector3 left = Quaternion.Euler(0, 0, -135) * direction * 0.25f;
        Gizmos.DrawLine(arrowTip, arrowTip + right);
        Gizmos.DrawLine(arrowTip, arrowTip + left);
    }

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
}
