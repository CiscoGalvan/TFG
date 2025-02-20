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
    private bool m_endPersistentDamage;


    // Stores the latest collision event
    Collider2D m_collisionobj;

    public enum DamageType
    {
        Instant,
        Persistent,
        Residual
    }
	[SerializeField,HideInInspector]
	private bool _instaKill = false;
    [SerializeField]
    private DamageType m_damageType = 0; // 0 = Instantáneo, 1 = Persistente, 2 = Residual
    [SerializeField,HideInInspector]
    private float _amountOfDamage= 0; //para tipo 0,1 y 2
	#region Persistent Damage Variables
	private float _persistentCooldown = 0f;
	[SerializeField, HideInInspector]
	private float _damageCooldown = 1f; // para tipo  2
	#endregion
	#region Residual Damage Variables
	//[SerializeField]
	//private int _numOfDamage = 2; //cuantas veces haces daño
	#endregion


	#region Trigger Methods
	private void OnTriggerEnter2D(Collider2D collision)
    {
      
		if (m_isChecking)
        {
			#region Antiguo switch
			//       switch (m_damageType) //0 = Instantáneo, 1 = Persistente, 2 = Residual
			//       {
			//           case DamageType.Instant:
			//               m_col = true;
			//               m_collisionobj = collision;
			//               EventDetected(); // Call the event handler method
			//               break;
			//           case DamageType.Persistent: //daño que persiste mientras está dentro del trigger
			//m_col = true;
			//               EventDetected(); //mirar de que tipo es al recibir el evento
			//               //si es de tipo persistente
			//               break;
			//           case DamageType.Residual: //permanece activo
			//	//Representa el daño aplicado tras un impacto inicial,
			//	//pero que permanece activo durante un corto período
			//	//, infligiendo unas pequeñas cantidades de daño,
			//	//incluso si el volumen de colisión ya no está en contacto
			//	//con el volumen que inició el daño.
			//m_col = true;
			//               EventDetected();
			//               break;
			//           default:
			//               break;

			//       }
			#endregion
			m_col = true;
            m_collisionobj = collision;
			m_isChecking = false;
			EventDetected(); // Call the event handler method
        }
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		// In case it is a Persistent Damage, it will deal damage while the player is in the contact.
		if (m_isChecking && m_damageType == DamageType.Persistent)
		{
			m_col = true;
			m_collisionobj = collision;
			m_isChecking = false;
			EventDetected(); // Call the event handler method
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		m_isChecking = true;
		m_col = false;
		if (m_damageType == DamageType.Persistent)
		{
			_persistentCooldown = 0f;
			m_endPersistentDamage = true;
		}
			
	}
	#endregion
	private void Update()
	{
		if (m_damageType == DamageType.Persistent)
		{
			if (m_col)
			{
				_persistentCooldown += Time.deltaTime;
				if (_persistentCooldown > _damageCooldown)
				{
					m_col = false;
					m_isChecking = true;
					_persistentCooldown = 0f;
				}
			}
		}
	}
	#region ¿Si queremos chocarnos?
	// Tenemos que preguntar si queremos chocar contra el enemigo o no, para decir si tenemos triggers o colisiones.
	//private void OnCollisionEnter2D(Collision2D collision)
	//{
	//	if (m_isChecking)
	//	{
	//		switch (m_damageType) //0 = Instantáneo, 1 = Persistente, 2 = Residual
	//		{
	//			case DamageType.Instant:
	//				m_col = true;
	//				m_collisionobj = collision;
	//				EventDetected(); // Call the event handler method
	//				break;
	//			case DamageType.Persistent: //daño que persiste mientras está dentro del trigger
	//				m_col = true;
	//				EventDetected(); //mirar de que tipo es al recibir el evento
	//								 //si es de tipo persistente
	//				break;
	//			case DamageType.Residual: //permanece activo
	//									  //Representa el daño aplicado tras un impacto inicial,
	//									  //pero que permanece activo durante un corto período
	//									  //, infligiendo unas pequeñas cantidades de daño,
	//									  //incluso si el volumen de colisión ya no está en contacto
	//									  //con el volumen que inició el daño.
	//				m_col = true;
	//				EventDetected();
	//				break;
	//			default:
	//				break;

	//		}

	//		// instantaneo
	//		// ini residual

	//	}
	//}
	#endregion
	
    public override bool CanTransition()
    {
        //la transicion depende del tipo de daño, 
            //intantaneo al inicio
            //persistente cuando salga
            //resudual al aplicar el primer daño

        if (m_damageType == DamageType.Instant && m_col)
		{
            m_isChecking = false;
            return true;
        }
        else if (m_damageType == DamageType.Persistent && m_endPersistentDamage)
        {
            m_isChecking = false; 
            return true;
        }
        else if (m_damageType == DamageType.Residual)
        {
            //???
            return true;
        }
        return false; 
    }

    public override void StartSensor()
	{
		m_col = false;
        m_isChecking = true;
        m_endPersistentDamage = false;
    }
    // Getters
    public bool IsChecking() => m_isChecking;
    public bool HasCollisionOccurred() => m_col;
	public bool EndPersistentDamage() => m_endPersistentDamage;
    public Collider2D GetCollisionObject() => m_collisionobj;
    public DamageType GetDamageType() => m_damageType;
    public float GetAmountOfDamage() => _amountOfDamage;
	public void SetAmountOfDamage(float newValue)
	{
		_amountOfDamage = newValue;
	} 
	public float GetDamageCooldown() => _damageCooldown;
    //public int GetNumOfDamage() => _numOfDamage;

    public void SetDamageCooldown(float newValue)
    {
        _damageCooldown = newValue;
    }
	public bool GetInstaKill() => _instaKill;
	public void SetInstaKill(bool newValue)
	{
		_instaKill = newValue;
	}
}
