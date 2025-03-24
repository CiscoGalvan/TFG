using System;
using UnityEditor;
using UnityEngine;

public class TimeSensor : Sensors
{
    // Time required for detection to trigger
    [SerializeField, Min(0)]
    private float _detectionTime = 5f;

    // Instancia de Timer
    private Timer _timer;
    private void Awake()
    {
        _timer = new Timer(_detectionTime);
    }
    private void Update()
    {
        if (!_sensorActive) return;
        _timer.Update(Time.deltaTime);

        // Si el temporizador llegó al tiempo de detección, activar evento
        if (_timer.GetTimeRemaining() <= 0)
        {
            EventDetected();
            _timer.Reset(); // Reiniciar el temporizador después de la detección
        }
    }

    // Activates the sensor. Is the firts method call
    public override void StartSensor()
    {
        _timer.Start();
        _sensorActive= true;
    }

    // Displays the remaining time in the scene view (editor only)
    private void OnDrawGizmos()
    {
        if (!_sensorActive) return;
        Gizmos.color = Color.blue;
        float timeRemaining = _timer != null ? _timer.GetTimeRemaining() : _detectionTime;
        Handles.Label(transform.position + Vector3.up * 1.5f, $"Time Remaining: {timeRemaining:0.00}s");
    }

	public override void StopSensor()
	{
		_sensorActive= false;
	}
}
