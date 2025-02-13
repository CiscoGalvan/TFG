using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Distance_Sensor : Sensors
{
    [SerializeField]
    private Transform m_target; // Objeto al que se mide la distancia
    [SerializeField, Min(0)]
    public float detectionDistance = 5f; // Distancia umbral para activar el evento
    public bool useMagnitude = true; // Si se mide la magnitud completa o solo en un eje
    public bool checkXAxis = true; // Si se mide en el eje X, si no, se mide en Y
    public static event Action<Transform> OnDistanceSensors;

    //Dibujado 
    //Medir en un solo eje (X o Y)
    //Lanzamiento de eventos
    //Repasar código

    private void Update()
    {
       
    }

    public override void StartSensor()
    {
        useMagnitude = true;
        checkXAxis = true;
	}

    public override bool CanTransition()
    {
		if (m_target == null) return false;

		float distance = useMagnitude
			? Vector2.Distance(transform.position, m_target.position)
			: checkXAxis
				? Mathf.Abs(transform.position.x - m_target.position.x)
				: Mathf.Abs(transform.position.y - m_target.position.y);
        if (distance <= detectionDistance)
        {
            //OnDistanceSensors?.Invoke(m_target);
            //Debug.Log("Distancia alcanzada, evento enviado.");
            return true;
        }
        else return false;
	}
}
