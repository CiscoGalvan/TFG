using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : Sensors
{
    [SerializeField, Range(0.1f, 6000000f)]
    private float detectionTime = 5f;

    private float timer = 0f;
    public static event Action OnTimeReached;
    //private void Update()
    //{
    //    timer += UnityEngine.Time.deltaTime;

    //    if (timer >= detectionTime)
    //    {
    //        OnTimeReached?.Invoke();
    //        Debug.Log("Tiempo alcanzado");
    //        timer = 0f; // Reiniciar el contador
    //    }
    //}

    public void ResetTimer()
    {
        timer = 0f;
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
