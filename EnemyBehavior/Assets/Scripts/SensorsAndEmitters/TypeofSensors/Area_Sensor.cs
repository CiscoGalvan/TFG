using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Area_Sensor : Sensors
{
	[SerializeField]
	private GameObject _target; 
	
	private Collider2D _detectionZone;
	public override void StartSensor()
	{
		_detectionZone = gameObject.GetComponent<Collider2D>();
		_detectionZone.isTrigger = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == _target)
		{
			EventDetected();
		}
	}
}
