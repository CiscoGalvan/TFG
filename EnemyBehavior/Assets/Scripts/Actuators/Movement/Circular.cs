using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Circular : MonoBehaviour
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
	private void Start()
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

    private void Update()
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
    private void OnDrawGizmos()
    {
        if (this.isActiveAndEnabled)
        {
            if(m_maxAngle == 360f)
            {
				if (m_rotationPointPosition == null)
				{
					Gizmos.DrawWireSphere(transform.position, m_radius);
				}
				else Gizmos.DrawWireSphere(m_rotationPointPosition.position, m_radius);

			}
            else
			{
				// Cálculo del ángulo inicial en radianes basado en la posición actual del objeto
				Vector3 direction = m_startingPosition - m_rotationPointPosition.position;
				float initialAngleRadians = Mathf.Atan2(direction.y, direction.x);

				// Rango de ángulos para cada mitad
				float halfAngleRange = Mathf.Deg2Rad * (m_maxAngle / 2f);

				// Ángulos para las dos mitades
				float startAngleLeft = initialAngleRadians + halfAngleRange;
				float endAngleLeft = initialAngleRadians;
				float startAngleRight = initialAngleRadians;
				float endAngleRight = initialAngleRadians - halfAngleRange;

				// Configuración de segmentos
				int segments = 30; // Aumentar para suavizar las curvas
				float angleStepLeft = (startAngleLeft - endAngleLeft) / segments;
				float angleStepRight = (endAngleRight - startAngleRight) / segments;

				// Dibujar lado izquierdo (arco positivo)
				Vector3 previousPoint = m_rotationPointPosition.position + new Vector3(
					Mathf.Cos(startAngleLeft) * m_radius,
					Mathf.Sin(startAngleLeft) * m_radius,
					0f);

				Gizmos.color = Color.red; // Color para el lado izquierdo
				for (int i = 1; i <= segments; i++)
				{
					float angle = startAngleLeft - i * angleStepLeft;
					Vector3 currentPoint = m_rotationPointPosition.position + new Vector3(
						Mathf.Cos(angle) * m_radius,
						Mathf.Sin(angle) * m_radius,
						0f);

					Gizmos.DrawLine(previousPoint, currentPoint);
					previousPoint = currentPoint;
				}

				// Dibujar lado derecho (arco negativo)
				previousPoint = m_rotationPointPosition.position + new Vector3(
					Mathf.Cos(startAngleRight) * m_radius,
					Mathf.Sin(startAngleRight) * m_radius,
					0f);

				Gizmos.color = Color.blue; // Color para el lado derecho
				for (int i = 1; i <= segments; i++)
				{
					float angle = startAngleRight + i * angleStepRight;
					Vector3 currentPoint = m_rotationPointPosition.position + new Vector3(
						Mathf.Cos(angle) * m_radius,
						Mathf.Sin(angle) * m_radius,
						0f);

					Gizmos.DrawLine(previousPoint, currentPoint);
					previousPoint = currentPoint;
				}
			}
		}
	}
	public Transform GetRotationPoint() { return m_rotationPointPosition; }
    public float GetRadius() { return m_radius; }
    public void SetRadius(float newValue) { m_radius = newValue; }
    public bool RotationPointAssigned() { return !m_noRotationPointAssigned; }
	
}
