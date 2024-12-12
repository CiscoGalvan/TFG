using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertical : MonoBehaviour
{
    [Tooltip("Velocidad inicial del objeto en unidades por segundo")]
    [SerializeField]
    float m_velocity = 1f;

    [Tooltip("Velocidad maxima del objeto en unidades por segundo")]
    [SerializeField]
    float m_maxvelocity = 10f;

    [Tooltip("Aceleracion del objeto en unidades por segundo cuadrado. Dejar en 0 para movimiento uniforme")]
    [SerializeField]
    float m_aceleration = 0f;

    [Tooltip("Direccion de movimiento. -1 abajo / 1 arriba")]
    [Range (-1,1)]
    [SerializeField]
    int m_dir = 1;

    float m_time =0;

    Transform m_transform;
    private void Start()
    {
        m_transform = this.GetComponent<Transform>();
        Colision.OnMensajeEnviado += RecibirMensaje;
    }
    private void OnDestroy()
    {
        Colision.OnMensajeEnviado -= RecibirMensaje;
    }
    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;

        // MRU: x = x0 + v*t
        // MRUA: x = x0 + v0*t + 1/2 * a * t^2
        float desp;
        if (m_aceleration == 0)
        {
            // MRU
            desp = m_velocity * Time.deltaTime * m_dir;
        }
        else
        {
            // MRUA
            desp = m_velocity * Time.deltaTime * m_dir + 0.5f * m_aceleration * Mathf.Pow(Time.deltaTime, 2) * m_dir;
            m_velocity += m_aceleration * Time.deltaTime; // Actualizar velocidad en MRUA
            if (m_velocity > m_maxvelocity) m_velocity = m_maxvelocity;
        }

        m_transform.position += new Vector3(0, desp, 0);
    }
   void RecibirMensaje(string mensaje)
    {
        Debug.Log("mensaje recivido: "+ mensaje);
        m_dir *= -1;
    }
}
