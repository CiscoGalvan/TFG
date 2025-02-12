using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Rigidbody2D))]

public class Vertical : Actuator
{
    [Tooltip("Initial speed of the object in units per second")]
    [SerializeField]
    float m_speed = 1f;

    [Tooltip("Maximum speed of the object in units per second")]
    [SerializeField]
    float m_maxspeed = 10f;

    //[Tooltip("Object acceleration in units per second squared. Set to 0 for uniform motion")]
    //[SerializeField]
    //float m_acceleration = 0f;

    [Tooltip("Movement direction")]
    [SerializeField]
    private Direction m_direction = Direction.Down;

    //[Tooltip("Easing function for acceleration")]
    //[SerializeField]
    //private EasingFunction.Ease m_easingFunction = EasingFunction.Ease.Linear;
    public enum Direction
    {
        Down = -1,
        Up = 1
    }

    float m_time =0;

    private EasingFunction.Function easingFunc;
    Rigidbody2D m_rigidbody;
    public override void StartActuator()
    {
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        easingFunc = EasingFunction.GetEasingFunction(m_easingFunction);
        //Collision.OnCollisionSensor += ReceiveMessage;
    }
    public override void DestroyActuator()
    {
        //Collision.OnCollisionSensor -= ReceiveMessage;
    }

    public override void UpdateActuator()
    {
        Debug.Log("Vertical");
        m_time += Time.deltaTime;
        int dirValue = (int)m_direction;
        // MRU: x = x0 + v*t
        // MRUA: x = x0 + v0*t + 1/2 * a * t^2
        float desp;
        if (m_accelerationValue == 0)
        {
            // MRU
            desp = m_speed * Time.deltaTime * dirValue;
        }
        else
        {
            float easedAcceleration = m_accelerationValue > 0 ? easingFunc(0, m_accelerationValue, Mathf.Clamp01(m_time)) : 0;

            // MRUA
            desp = m_speed * Time.deltaTime * dirValue + 0.5f * easedAcceleration * Mathf.Pow(Time.deltaTime, 2) * dirValue;
            m_speed += easedAcceleration * Time.deltaTime; // Update speed in accelerated motion
            if (m_speed > m_maxspeed) m_speed = m_maxspeed;
        }

        m_rigidbody.position += new Vector2(0, desp);
    }
   void ReceiveMessage(Collision2D mensaje)
    {

        m_direction = m_direction == Direction.Down ? Direction.Up: Direction.Down;
    }
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
}
