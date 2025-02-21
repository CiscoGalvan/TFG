using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damage_Sensor : Sensors
{
    // Boolean to track whether the sensor is actively checking for collisions
    private bool m_isChecking;
    // Boolean to track if a collision has occurred
    private bool m_col;
	DamageEmitter _damageEmitter;

	#region Trigger Methods
	private void OnTriggerEnter2D(Collider2D collision)
    {
		if (m_isChecking)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if(_damageEmitter != null)
			{
				m_col = true;
				EventDetected();
			}
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (m_isChecking) 
		{
			_damageEmitter= collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null)
			{
				
				m_col = false;
				EventDetected();
			}
		}
	}
	#endregion

	#region Collision Methods
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (m_isChecking)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null)
			{
				m_col = true;
				EventDetected();
			}
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (m_isChecking)
		{
			_damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();
			if (_damageEmitter != null)
			{

				m_col = false;
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
		m_col = false;
        m_isChecking = true;
    }
	private void Start()
	{
		if (gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			m_col = false;
			m_isChecking = true;
		} 
	}
	// Getters
	public bool IsChecking() => m_isChecking;
    public bool HasCollisionOccurred() => m_col;
	public DamageEmitter GetDamageEmitter() => _damageEmitter;
}
