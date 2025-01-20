using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
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

    public enum Direction
    {
        Left = -1,
        Right = 1
    }

    float m_time =0;

    Transform m_transform;
    private void Start()
    {
        m_transform = this.GetComponent<Transform>();
        Collision.OnMessageSent += ReceiveMessage;
    }
    private void OnDestroy()
    {
        Collision.OnMessageSent -= ReceiveMessage;
    }
    // Update is called once per frame
    void Update()
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
            // MRUA
            desp = m_speed * Time.deltaTime * dirValue + 0.5f * m_acceleration * Mathf.Pow(Time.deltaTime, 2) * dirValue;
            m_speed += m_acceleration * Time.deltaTime; // Update speed in accelerated motion
            if (m_speed > m_maxspeed) m_speed = m_maxspeed;
        }

        m_transform.position += new Vector3(desp, 0, 0);
    }
   void ReceiveMessage(Collision2D mensaje)
    {
       m_dir = m_dir == Direction.Left ? Direction.Right : Direction.Left;
    }
}
