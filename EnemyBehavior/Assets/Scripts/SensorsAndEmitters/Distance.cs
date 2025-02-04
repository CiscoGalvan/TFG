using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : Sensors
{
    [SerializeField]
    Transform target; // Objeto al que se mide la distancia
    [SerializeField]
    float maxDistationRange = 20.0f; // Objeto al que se mide la distancia
    [SerializeField, Range(0.0f, 30000.0f)]
    public float detectionDistance = 5f; // Distancia umbral para activar el evento
    public bool useMagnitude = true; // Si se mide la magnitud completa o solo en un eje
    public bool checkXAxis = true; // Si se mide en el eje X, si no, se mide en Y

    public static event Action<Transform> OnDistanceSensors;

    private void Update()
    {
        if (target == null) return;

        float distance = useMagnitude
            ? Vector2.Distance(transform.position, target.position)
            : checkXAxis
                ? Mathf.Abs(transform.position.x - target.position.x)
                : Mathf.Abs(transform.position.y - target.position.y);

        if (distance <= detectionDistance)
        {
            OnDistanceSensors?.Invoke(target);
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
