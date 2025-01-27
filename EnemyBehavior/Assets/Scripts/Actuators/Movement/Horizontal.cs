using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Horizontal : MonoBehaviour
{
    [Tooltip("Initial speed of the object in units per second")]
    [SerializeField]
    float m_speed = 1f;

    [Tooltip("Maximum speed of the object in units per second")]
    [SerializeField]
    float m_maxspeed = 10f;

    [Tooltip("Object acceleration in units per second squared. Set to 0 for uniform motion")]
    [SerializeField]
    float m_acceleration = 0f;

    [Tooltip("Movement direction")]
    [SerializeField]
    private Direction m_dir = Direction.Right;


    [Tooltip("Easing function for acceleration")]
    [SerializeField]
    private EasingFunction.Ease m_easingFunction = EasingFunction.Ease.Linear;

    public enum Direction
    {
        Left = -1,
        Right = 1
    }

    float m_time =0;
    Rigidbody2D m_rigidbody;
    private EasingFunction.Function easingFunc;

    private void Start()
    {
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        easingFunc = EasingFunction.GetEasingFunction(m_easingFunction);
        Collision.OnMessageSent += ReceiveMessage;
    }
    private void OnDestroy()
    {
        Collision.OnMessageSent -= ReceiveMessage;
    }
   
    void FixedUpdate()
    {
        m_time += Time.deltaTime;
        int dirValue = (int)m_dir;
        // MRU: x = x0 + v*t
        // MRUA: x = x0 + v0*t + 1/2 * a * t^2
        float desp;
        if (m_acceleration == 0)
        {
            // MRU
            desp = m_speed * Time.deltaTime * dirValue;
        }
        else
        {
            float easedAcceleration = m_acceleration > 0 ? easingFunc(0, m_acceleration, Mathf.Clamp01(m_time)) : 0;

            // MRUA
            desp = m_speed * Time.deltaTime * dirValue + 0.5f * easedAcceleration * Mathf.Pow(Time.deltaTime, 2) * dirValue;
            m_speed += easedAcceleration * Time.deltaTime; // Update speed in accelerated motion
            if (m_speed > m_maxspeed) m_speed = m_maxspeed;
        }

        m_rigidbody.position += new Vector2(desp, 0);
    }
   void ReceiveMessage(Collision2D mensaje)
    {
       m_dir = m_dir == Direction.Left ? Direction.Right : Direction.Left;
    }

    private void OnDrawGizmosSelected()
    {
        if (! this.isActiveAndEnabled) return;

        Gizmos.color = Color.green;
        Vector3 position = transform.position;

        Vector3 direction = new Vector3((int)m_dir, 0, 0);

        // Draw direction arrow
        Gizmos.DrawLine(position, position + direction );
        Vector3 arrowTip = position + direction;
        Vector3 right = Quaternion.Euler(0, 0, 135) * direction * 0.25f;
        Vector3 left = Quaternion.Euler(0, 0, -135) * direction * 0.25f;
        Gizmos.DrawLine(arrowTip, arrowTip + right);
        Gizmos.DrawLine(arrowTip, arrowTip + left);
       
    }
}
