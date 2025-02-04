using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Circular : Actuator
{
    

    [Tooltip("Angular speed in degrees per second")]
    [SerializeField]
    private float angularSpeed = 90f;

    [Tooltip("Center of rotation (if not specified, uses the object's initial position)")]
    [SerializeField]
    private Transform m_rotationPointPosition;

    [Tooltip("Maximum allowed angle in degrees. Use 360 for a full circle, less for pendulum-like motion")]
    [SerializeField]
    [Range(0f, 360f)]
    private float m_maxAngle = 360f;

    [Tooltip("Angular acceleration in degrees per second squared (set to 0 for constant speed)")]
    [SerializeField]
    private float m_angularAcceleration = 0f;

    private float m_currentAngle = 0f; // Current angle in degrees
    private float m_currentAngularSpeed; // Current angular speed
    private bool m_reversing = false; // Indicates whether the motion is reversing (pendulum behavior)
    private bool m_noRotationPointAssigned = false; //Indicates whether there is a rotation point assigned or not (used for editor purposes)
    private float m_radius = 0;
    private Vector2 m_direction; //vector that points from the rotation point towards the moving object
    private Vector3 m_startingPosition;
    private float m_initAngle;
    // Start is called before the first frame update
    public override void Start()
    {
        m_startingPosition = transform.position;
		m_direction = transform.position - m_rotationPointPosition.position;
        

		float angleRadians = Mathf.Atan2(m_direction.y, m_direction.x);
		m_currentAngle = angleRadians * Mathf.Rad2Deg;
       
		Vector3 initialPosition = transform.position;
        m_initAngle = m_currentAngle;

		if (m_rotationPointPosition == null)
        {
            Debug.Log("There was no rotation point assigned to object: " + gameObject.name +"\nIt will rotate from it's initial position with radius: " + m_radius);
            m_rotationPointPosition = new GameObject(gameObject.name + "RotationPoint").transform;
            m_rotationPointPosition.position = initialPosition;
            m_noRotationPointAssigned = true;
		}
		else m_radius = Vector3.Distance(m_rotationPointPosition.position, transform.position);
		m_currentAngularSpeed = angularSpeed;
    }

    public override void UpdateActuator()
	{
		//Debug.Log(m_currentAngle);
		// Update speed if there's angular acceleration
		if (m_angularAcceleration != 0f)
        {
            m_currentAngularSpeed += m_angularAcceleration * Time.deltaTime;
        }

        if (m_maxAngle < 360f)// Pendulum-like motion
        {
			if (m_reversing)
            {

				float limitAngle = m_initAngle - m_maxAngle / 2;
				m_currentAngle -= m_currentAngularSpeed * Time.deltaTime;
				if (m_currentAngle < limitAngle)
				{
					m_reversing = false;
				}
			}
            else
			{
				float limitAngle = m_initAngle + m_maxAngle / 2;
				m_currentAngle += m_currentAngularSpeed * Time.deltaTime;
                if (m_currentAngle > limitAngle)
                {
                    m_reversing = true;
                }
            }
        }
        else // Full circular motion
        {
           
            m_currentAngle += m_currentAngularSpeed * Time.deltaTime;
            m_currentAngle %= 360f; 
        }

        // Convert angle to radians
        float angleRadians = m_currentAngle * Mathf.Deg2Rad;

        // Calculate new position
        Vector3 offset = new Vector3(m_radius * Mathf.Cos(angleRadians), m_radius * Mathf.Sin(angleRadians), 0f);

        transform.position = m_rotationPointPosition.position + offset;
    }
    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
		
        if (this.isActiveAndEnabled)
        {
            if(m_maxAngle == 360f)
            {

				if (m_rotationPointPosition == null)
				{					
					Gizmos.DrawWireSphere(transform.position, m_radius);
				}
				else {
                    m_radius = Vector3.Distance(m_rotationPointPosition.position, transform.position);
                    Gizmos.DrawWireSphere(m_rotationPointPosition.position, m_radius); 

				}

			}
            else
			{
                Vector3 direction = m_startingPosition - m_rotationPointPosition.position;
                float initialAngleDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Rango de ángulos para cada mitad
                float halfAngleRange = m_maxAngle / 2f;

                // Dibujar lado izquierdo (arco positivo)
                Handles.color = Color.red;
                Handles.DrawWireArc(m_rotationPointPosition.position,Vector3.forward,Quaternion.Euler(0, 0, initialAngleDegrees + halfAngleRange) * Vector3.right, -halfAngleRange,m_radius);

                // Dibujar lado derecho (arco negativo)
                Handles.color = Color.blue;
                Handles.DrawWireArc(m_rotationPointPosition.position,Vector3.forward,Quaternion.Euler(0, 0, initialAngleDegrees) * Vector3.right,-halfAngleRange,m_radius);
            }
		}
	}
    #endif
    public Transform GetRotationPoint() { return m_rotationPointPosition; }
    public float GetRadius() { return m_radius; }
    public void SetRadius(float newValue) { m_radius = newValue; }
    public bool RotationPointAssigned() { return !m_noRotationPointAssigned; }

    public override void Destroy()
    {
        
    }
}
