using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circular : MonoBehaviour
{
    [Tooltip("Radio de la trayectoria circular")]
    [SerializeField]
    private float m_radius = 5f;

    [Tooltip("Velocidad angular en grados por segundo")]
    [SerializeField]
    private float angularSpeed = 90f;

    [Tooltip("Centro de rotacion (si no se especifica, usa la posicion inicial del objeto)")]
    [SerializeField]
    private Transform m_rotationPointPosition;

    [Tooltip("Angulo maximo permitido en grados. 360 para un circulo completo, menos para un movimiento tipo pendulo")]
    [SerializeField]
    [Range(0f, 360f)]
    private float m_maxAngle = 360f;

    [Tooltip("Aceleracion angular en grados por segundo cuadrado (dejar en 0 para velocidad constante)")]
    [SerializeField]
    private float m_angularAcceleration = 0f;

    private float m_currentAngle = 0f; // Angulo actual en grados
    private float m_currentAngularSpeed; // Velocidad angular actual
    private bool m_reversing = false; // Indica si el movimiento está en reversa para el pendulo

    // Start is called before the first frame update
    private void Start()
    {

        Vector3 initialPosition = transform.position;
        if (m_rotationPointPosition == null)
        {
            m_rotationPointPosition = new GameObject("Rotation Point").transform;
            m_rotationPointPosition.position = initialPosition;
        }

        m_currentAngularSpeed = angularSpeed;
    }

    private void Update()
    {
        // Actualizar velocidad si hay aceleracion
        if (m_angularAcceleration != 0f)
        {
            m_currentAngularSpeed += m_angularAcceleration * Time.deltaTime;
        }

        if (m_maxAngle < 360f)// Movimiento tipo pendulo
        {
            if (m_reversing)
            {
                m_currentAngle -= m_currentAngularSpeed * Time.deltaTime;
                if (m_currentAngle <= -m_maxAngle)
                {
                    m_currentAngle = -m_maxAngle;
                    m_reversing = false;
                }
            }
            else
            {               
                m_currentAngle += m_currentAngularSpeed * Time.deltaTime;
                if (m_currentAngle >= m_maxAngle)
                {
                    m_currentAngle = m_maxAngle;
                    m_reversing = true;
                }
            }
        }
        else // Movimiento circular completo
        {
           
            m_currentAngle += m_currentAngularSpeed * Time.deltaTime;
            m_currentAngle %= 360f; 
        }

        // Convertir angulo a radianes
        float angleRadians = m_currentAngle * Mathf.Deg2Rad;

        // Calcular nueva posicion
        Vector3 offset = new Vector3(m_radius * Mathf.Cos(angleRadians), m_radius * Mathf.Sin(angleRadians), 0f);

        transform.position = m_rotationPointPosition.position + offset;
    }
}
