using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damage_Sensor : Sensors
{
    // Boolean to track if a collision has occurred
    private bool _col;
	private DamageEmitter _damageEmitter;
	[SerializeField]
	private bool _activeFromStart = false;
	[SerializeField, HideInInspector]
	private bool _moreThanOneCollider;
	#region Trigger Methods
	private void OnTriggerEnter2D(Collider2D collision)
    {
		if (_sensorActive)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if(_damageEmitter != null && _damageEmitter.GetDamageEmitterCollider() == collision)
			{
				_col = true;
				EventDetected();
			}
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (_sensorActive) 
		{
			_damageEmitter= collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null && _damageEmitter.GetDamageEmitterCollider() == collision)
			{
				
				_col = false;
				EventDetected();
			}
		}
	}
	#endregion

	#region Collision Methods
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (_sensorActive)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null && _damageEmitter.GetDamageEmitterCollider() == collision.collider)
			{
				_col = true;
				EventDetected();
			}
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (_sensorActive)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null && _damageEmitter.GetDamageEmitterCollider() == collision.collider)
			{
				_col = false;
				EventDetected();
			}
		}
	}

	#endregion
    public override void StartSensor()
	{
		_col = false;
        _sensorActive = true;
    }
	private void Start()
	{
		if (_activeFromStart)
		{
			_col = false;
			_sensorActive = true;
		} 
	}
	// Getters
    public bool HasCollisionOccurred() => _col;
	public DamageEmitter GetDamageEmitter() => _damageEmitter;

	public override void StopSensor()
	{
		_sensorActive = false;
	}
}
