using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AreaSensor : Sensor
{
	[SerializeField]
	private GameObject _target; 
	
	private Collider2D _detectionZone;

	public override void StartSensor()
	{
		base.StartSensor();	
		_detectionZone = gameObject.GetComponent<Collider2D>();
		_detectionZone.isTrigger = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!_sensorActive ||!_timerFinished) return;

		if (collision.gameObject == _target)
		{
			EventDetected();
		}
	}
	
	// We have to check if the sensor may be triggered just when the timer finished.
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!_sensorActive || !_timerFinished) return;
		
		if (collision.gameObject == _target)
		{
			EventDetected();
		}
	}
}
