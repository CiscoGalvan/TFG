using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DistanceSensor;

[RequireComponent(typeof(Collider2D))]
public class AreaSensor : Sensor
{
	[SerializeField]
	private GameObject _target; 
	
	private Collider2D _detectionZone;
    [SerializeField]
    private DetectionCondition _detectionCondition = DetectionCondition.InsideMagnitude;
    public enum DetectionCondition
    {
        InsideMagnitude = 0,
        OutsideMagnitude = 1
    };
    public override void StartSensor()
	{
		base.StartSensor();	
		_detectionZone = gameObject.GetComponent<Collider2D>();
		_detectionZone.isTrigger = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!_sensorActive ||!_timerFinished) return;

		if (_detectionCondition == DetectionCondition.InsideMagnitude && collision.gameObject == _target)
		{
			EventDetected();
		}

        
    }
	
	// We have to check if the sensor may be triggered just when the timer finished.
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!_sensorActive || !_timerFinished) return;
		
		if (_detectionCondition == DetectionCondition.InsideMagnitude && collision.gameObject == _target)
		{
			EventDetected();
		}
	}
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_sensorActive || !_timerFinished) return;

        if (_detectionCondition == DetectionCondition.OutsideMagnitude && collision.gameObject == _target)
        {
            EventDetected();
        }
    }
}
