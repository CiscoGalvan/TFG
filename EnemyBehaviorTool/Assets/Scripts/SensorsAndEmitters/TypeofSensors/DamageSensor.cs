using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageSensor : Sensor
{
    // Boolean to track if a collision has occurred
    private bool _col;
	private DamageEmitter _damageEmitter;
	[Tooltip("If true, the Damage Sensor won't need to be included in any State in order to activate itself.")]
	[SerializeField]
	private bool _activeFromStart = false;

	

	#region Trigger Methods
	private void OnTriggerEnter2D(Collider2D collision)
    {
		if (_sensorActive && _timerFinished)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if(_damageEmitter != null && _damageEmitter.enabled &&_damageEmitter.GetEmitting())
			{
				if (_damageEmitter.GetDestroyAfterDoingDamage())
				{
					//Como obtenemos la animacion?
					AnimatorManager _animatorManager = collision.gameObject.GetComponent<AnimatorManager>();
					if(_animatorManager != null)
					{
                        _animatorManager.Destroy();
					}
					else
						Destroy(collision.gameObject);
				}
				_col = true;
				EventDetected();
			}
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (_sensorActive && _timerFinished) 
		{
			_damageEmitter= collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null && _damageEmitter.enabled && _damageEmitter.GetEmitting())
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
		if (_sensorActive && _timerFinished)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null && _damageEmitter.enabled &&_damageEmitter.GetEmitting())
			{
				if (_damageEmitter.GetDestroyAfterDoingDamage())
				{
                    AnimatorManager animatorController = collision.gameObject.GetComponent<AnimatorManager>();
					if (animatorController != null)
					{
						animatorController.Destroy();
					}
					else
						Destroy(collision.gameObject);
				}
				_col = true;
				EventDetected();
			}
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (_sensorActive && _timerFinished)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null && _damageEmitter.enabled &&_damageEmitter.GetEmitting())
			{
				_col = false;
				EventDetected();
			}
		}
	}

	#endregion
    public override void StartSensor()
	{
		base.StartSensor();
		_col = false;
      
	}
	private void Start()
	{
		if (_activeFromStart)
		{
			StartSensor();
		} 
	}

	public bool HasCollisionOccurred() => _col;
	public DamageEmitter GetDamageEmitter() => _damageEmitter;
}
