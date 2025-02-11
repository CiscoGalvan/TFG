using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : Sensors
{
    [SerializeField, Min(0)]
    private float detectionTime = 5f;
    private float timer = 0f;
    private bool startTimer;
    public static event Action OnTimeReached;



	//Dibujado 
	//Medir en un solo eje (X o Y)
	//Lanzamiento de eventos
	//Repasar código
	private void Update()
    {
        if (startTimer)
        {
            timer += UnityEngine.Time.deltaTime;
            //if (timer >= detectionTime)
            //{
            //    OnTimeReached?.Invoke();
            //    Debug.Log("Tiempo alcanzado");
            //    timer = 0f; // Reiniciar el contador
            //}
        }
    }
    public override void StartSensor()
    {
        timer = 0f;
        startTimer = true;
    }

    public override bool CanTransition()
    {
        return timer >= detectionTime;
    }
}
