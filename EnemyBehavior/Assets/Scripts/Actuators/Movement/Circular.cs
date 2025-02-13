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
	[SerializeField]
	[HideInInspector]
	private float m_angularSpeed;

	[Tooltip("Center of rotation (if not specified, uses the object's initial position)")]
	[SerializeField]
	private Transform m_rotationPointPosition;

	[Tooltip("Maximum allowed angle in degrees. Use 360 for a full circle, less for pendulum-like motion")]
	[SerializeField]
	[Range(0f, 360f)]
	private float m_maxAngle = 360f;

	[SerializeField]
	[HideInInspector]
	private float m_angularAcceleration = 0f;

	private Rigidbody2D m_rigidbody;
	private float m_currentAngularSpeed;
	private float m_currentAngle;
	private bool m_reversing = false;
	private float m_radius = 0;
	private float m_initAngle;
	private float m_initAngularSpeed;
	private float m_goalAngularSpeed;

	
	float m_time;
	[SerializeField]
	[HideInInspector]
	float m_interpolationTime;


	private Vector3 m_startingPosition;
	private EasingFunction.Function easingFunc;
	private Vector2 m_direction; //vector that points from the rotation point towards the moving object
	public override void StartActuator()
	{
		m_startingPosition = transform.position;
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_direction = transform.position - m_rotationPointPosition.position;
		easingFunc = EasingFunction.GetEasingFunction(m_easingFunction);
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
		Debug.Log(m_angularSpeed);
		m_currentAngularSpeed = m_angularSpeed * Mathf.Deg2Rad; // Convertimos a radianes
		m_initAngularSpeed = m_currentAngularSpeed;
		m_time = 0;
	}

	public override void UpdateActuator()
	{
		m_time += Time.deltaTime;
		// Actualizar la velocidad angular si hay aceleración
		if (m_isAccelerated)
		{
			float t = Mathf.Clamp01(m_time / m_interpolationTime);
			m_currentAngularSpeed = easingFunc(m_initAngularSpeed, m_goalAngularSpeed, t);
			if (t >= 1.0f)
			{
				m_currentAngularSpeed = m_goalAngularSpeed;
			}
		}

		if (m_maxAngle < 360f) // Movimiento pendular
		{

			if (m_reversing)
			{
				float limitAngle = m_initAngle - (m_maxAngle) / 2;
				m_currentAngle -= m_currentAngularSpeed * Time.deltaTime;
				if (m_currentAngle < limitAngle)
				{
					m_reversing = false;
				}
			}
			else
			{
				float limitAngle = m_initAngle + (m_maxAngle ) / 2;
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
			m_currentAngle %=360; 
		}

		// **Calcular velocidad tangencial**
		float tangentialSpeed = m_currentAngularSpeed * m_radius;

		// Si está en reversa, invertir dirección
		if (m_reversing)
		{
			tangentialSpeed *= -1;
		}

		Vector2 tangentialVelocity = new Vector2(
			-Mathf.Sin(m_currentAngle) * tangentialSpeed,
			Mathf.Cos(m_currentAngle) * tangentialSpeed
		);

		m_rigidbody.velocity = tangentialVelocity;
		Debug.Log(m_reversing);
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
				Vector3 direction = m_startingPosition - m_rotationPointPosition.position;
				float initialAngleDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

				// Rango de ángulos para cada mitad
				float halfAngleRange = m_maxAngle / 2f;

				// Dibujar lado izquierdo (arco positivo)
				Handles.color = Color.red;
				Handles.DrawWireArc(m_rotationPointPosition.position, Vector3.forward, Quaternion.Euler(0, 0, initialAngleDegrees + halfAngleRange) * Vector3.right, -halfAngleRange, m_radius);

				// Dibujar lado derecho (arco negativo)
				Handles.color = Color.blue;
				Handles.DrawWireArc(m_rotationPointPosition.position, Vector3.forward, Quaternion.Euler(0, 0, initialAngleDegrees) * Vector3.right, -halfAngleRange, m_radius);
			}
		}
	}
#endif

	public Transform GetRotationPoint() { return m_rotationPointPosition; }
	public float GetRadius() { return m_radius; }
	public void SetRadius(float newValue) { m_radius = newValue; }
	public bool RotationPointAssigned() { return m_rotationPointPosition != null; }

	public override void DestroyActuator() { }

	#region Setters and getters
	public void SetAngularSpeed(float newValue)
	{
		m_angularSpeed = newValue;
		//Para que la velocidad actual se actualice, si no solo podria cambiarse al iniciar el estado.
		m_currentAngularSpeed = m_angularSpeed * Mathf.Deg2Rad; // Convertimos a radianes
	}
	public float GetAngularSpeed()
	{
		return m_angularSpeed;
	}
	public void SetGoalAngularSpeed(float newValue)
	{
		m_goalAngularSpeed = newValue;
	}
	public float GetGoalAngularSpeed()
	{
		return m_goalAngularSpeed;
	}
	public void SetInterpolationTime(float newValue)
	{
		m_interpolationTime = newValue;
	}
	public float GetInterpolationTime()
	{
		return m_interpolationTime;
	}
	#endregion
}



