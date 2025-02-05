using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : Sensors
{
    [SerializeField]
    private Transform m_target; // Objeto al que se mide la distancia
    [SerializeField]
    float maxDistationRange = 20.0f; // Objeto al que se mide la distancia
    [SerializeField, Range(0.0f, 30000.0f)]
    public float detectionDistance = 5f; // Distancia umbral para activar el evento
    public bool useMagnitude = true; // Si se mide la magnitud completa o solo en un eje
    public bool checkXAxis = true; // Si se mide en el eje X, si no, se mide en Y

    public static event Action<Transform> OnDistanceSensors;

    private void Update()
    {
        if (m_target == null) return;

        float distance = useMagnitude
            ? Vector2.Distance(transform.position, m_target.position)
            : checkXAxis
                ? Mathf.Abs(transform.position.x - m_target.position.x)
                : Mathf.Abs(transform.position.y - m_target.position.y);

        if (distance <= detectionDistance)
        {
            OnDistanceSensors?.Invoke(m_target);
            Debug.Log("Distancia alcanzada, evento enviado.");
        }
    }

    public override void StartSensor()
    {
        throw new NotImplementedException();
    }

    public override bool CanTransition()
    {
        return false;
    }
}
