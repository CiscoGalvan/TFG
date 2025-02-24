using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damage_Sensor : Sensors
{
    // Boolean to track whether the sensor is actively checking for collisions
    private bool _isChecking;
    // Boolean to track if a collision has occurred
    private bool _col;
	private DamageEmitter _damageEmitter;

	#region Trigger Methods
	private void OnTriggerEnter2D(Collider2D collision)
    {
		if (_isChecking)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if(_damageEmitter != null)
			{
				_col = true;
				EventDetected();
			}
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (_isChecking) 
		{
			_damageEmitter= collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null)
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
		if (_isChecking)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null)
			{
				_col = true;
				EventDetected();
			}
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (_isChecking)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null)
			{

				_col = false;
				EventDetected();
			}
		}
	}

	#endregion
	public override bool CanTransition()
    {
  //      //la transicion depende del tipo de daño, 
  //          //intantaneo al inicio
  //          //persistente cuando salga
  //          //resudual al aplicar el primer daño

  //      if (m_damageType == DamageType.Instant && m_col)
		//{
  //          m_isChecking = false;
  //          return true;
  //      }
  //      else if (m_damageType == DamageType.Persistent && m_endPersistentDamage)
  //      {
  //          m_isChecking = false; 
  //          return true;
  //      }
  //      else if (m_damageType == DamageType.Residual)
  //      {
  //          //???
  //          return true;
  //      }
       return false; 
    }

    public override void StartSensor()
	{
		_col = false;
        _isChecking = true;
    }
	private void Start()
	{
		if (gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			_col = false;
			_isChecking = true;
		} 
	}
	// Getters
	public bool IsChecking() => _isChecking;
    public bool HasCollisionOccurred() => _col;
	public DamageEmitter GetDamageEmitter() => _damageEmitter;
}
