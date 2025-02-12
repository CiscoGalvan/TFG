using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

	private Rigidbody2D rb;
	private float m_currentAngularSpeed;
	private float m_currentAngle;
	private bool m_reversing = false;
	private float m_radius = 0;
	private float m_initAngle;

	public override void StartActuator()
	{
		rb = GetComponent<Rigidbody2D>();

		if (m_rotationPointPosition == null)
		{
			Debug.Log("No rotation point assigned. Using object's initial position.");
			m_rotationPointPosition = new GameObject(gameObject.name + " RotationPoint").transform;
			m_rotationPointPosition.position = transform.position;
		}

		m_radius = Vector3.Distance(m_rotationPointPosition.position, transform.position);
		Vector3 direction = transform.position - m_rotationPointPosition.position;
		m_initAngle = Mathf.Atan2(direction.y, direction.x);
		m_currentAngle = m_initAngle;
		m_currentAngularSpeed = angularSpeed * Mathf.Deg2Rad; // Convertimos a radianes
	}

	public override void UpdateActuator()
	{
		if (m_angularAcceleration != 0f)
		{
			m_currentAngularSpeed += m_angularAcceleration * Time.deltaTime;
		}

		if (m_maxAngle < 360f) // Movimiento pendular
		{
			if (m_reversing)
			{
				float limitAngle = m_initAngle - (m_maxAngle * Mathf.Deg2Rad) / 2;
				m_currentAngle -= m_currentAngularSpeed * Time.deltaTime;
				if (m_currentAngle < limitAngle)
				{
					m_reversing = false;
				}
			}
			else
			{
				float limitAngle = m_initAngle + (m_maxAngle * Mathf.Deg2Rad) / 2;
				m_currentAngle += m_currentAngularSpeed * Time.deltaTime;
				if (m_currentAngle > limitAngle)
				{
					m_reversing = true;
				}
			}
		}
		else // Movimiento circular completo
		{
			m_currentAngle += m_currentAngularSpeed * Time.deltaTime;
			m_currentAngle %= Mathf.PI * 2f; // Asegurar que se mantenga en 0-360°
		}

		// **Calcular velocidad tangencial**
		float tangentialSpeed = m_currentAngularSpeed * m_radius;
		Vector2 tangentialVelocity = new Vector2(
			-Mathf.Sin(m_currentAngle) * tangentialSpeed,
			Mathf.Cos(m_currentAngle) * tangentialSpeed
		);

		rb.velocity = tangentialVelocity;

	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		if (this.isActiveAndEnabled)
		{
			if (m_maxAngle == 360f)
			{
				if (m_rotationPointPosition == null)
				{
					Gizmos.DrawWireSphere(transform.position, m_radius);
				}
				else
				{
					m_radius = Vector3.Distance(m_rotationPointPosition.position, transform.position);
					Gizmos.DrawWireSphere(m_rotationPointPosition.position, m_radius);
				}
			}
			else
			{
				Vector3 direction = transform.position - m_rotationPointPosition.position;
				float initialAngleDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				float halfAngleRange = m_maxAngle / 2f;

				Handles.color = Color.red;
				Handles.DrawWireArc(m_rotationPointPosition.position, Vector3.forward,
					Quaternion.Euler(0, 0, initialAngleDegrees + halfAngleRange) * Vector3.right,
					-halfAngleRange, m_radius);

				Handles.color = Color.blue;
				Handles.DrawWireArc(m_rotationPointPosition.position, Vector3.forward,
					Quaternion.Euler(0, 0, initialAngleDegrees) * Vector3.right,
					-halfAngleRange, m_radius);
			}
		}
	}
#endif

	public Transform GetRotationPoint() { return m_rotationPointPosition; }
	public float GetRadius() { return m_radius; }
	public void SetRadius(float newValue) { m_radius = newValue; }
	public bool RotationPointAssigned() { return m_rotationPointPosition != null; }

	public override void DestroyActuator() { }
}
