using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AreaSensor : Sensors
{
	[SerializeField]
	private GameObject _target; 
	
	private Collider2D _detectionZone;
	public override void StartSensor()
	{
		_sensorActive= true;
		_detectionZone = gameObject.GetComponent<Collider2D>();
		_detectionZone.isTrigger = true;
	}

	public override void StopSensor()
	{
		_sensorActive = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!_sensorActive) return;
		if (collision.gameObject == _target)
		{
			EventDetected();
		}
	}
}
