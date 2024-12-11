using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horizontal : MonoBehaviour
{
    [Tooltip("Velocidad inicial del objeto en unidades por segundo")]
    [SerializeField]
    float m_velocity = 1f;

    [Tooltip("Aceleracion del objeto en unidades por segundo cuadrado. Dejar en 0 para movimiento uniforme")]
    [SerializeField]
    float m_aceleration = 0f;

    [Tooltip("Direccion de movimiento. -1 izquierda / 1 derecha")]
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
        // x = x0 + v*t mru
        // x = x0 + v0*t + 1/2 a t^2 
        float desp = m_velocity * Time.deltaTime * m_dir;
        m_transform.position += new Vector3(desp,0,0);
    }
   void RecibirMensaje(string mensaje)
    {
        Debug.Log("mensaje recivido: "+ mensaje);
        m_dir *= -1;
    }
}
