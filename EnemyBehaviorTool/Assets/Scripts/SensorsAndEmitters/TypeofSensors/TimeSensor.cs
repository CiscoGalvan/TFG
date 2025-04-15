
using UnityEditor;
using UnityEngine;

public class TimeSensor : Sensor
{
    // Time required for detection to trigger
    [SerializeField, Min(0)]
    private float _detectionTime = 5f;

    // Instancia de Timer
    private Timer _ownTimer;

	
	public override void StartSensor()
	{

		base.StartSensor();
		_ownTimer = new Timer(_detectionTime);
		_ownTimer.Start();
	}
	public override void UpdateSensor()
	{
		base.UpdateSensor();
		Debug.Log(_timerFinished);
		if (!_sensorActive || !_timerFinished) return;

		Debug.Log(_ownTimer.GetTimeRemaining());
		_ownTimer.Update(Time.deltaTime);

		// Si el temporizador llegó al tiempo de detección, activar evento
		if (_ownTimer.GetTimeRemaining() <= 0)
		{
			EventDetected();
			_ownTimer.Reset(); // Reiniciar el temporizador después de la detección
		}
	}

	// Displays the remaining time in the scene view (editor only)
	private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!_sensorActive) return;
        Gizmos.color = Color.blue;
        float timeRemaining = _ownTimer != null ? _ownTimer.GetTimeRemaining() : _detectionTime;

        UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, $"Time Remaining: {timeRemaining:0.00}s");
#endif
    }

}
