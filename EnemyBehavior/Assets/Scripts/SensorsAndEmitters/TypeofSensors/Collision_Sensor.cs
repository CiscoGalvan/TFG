using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Collision_Sensor : Sensors
{
    bool col;
	bool m_isChecking;	//estará a false cuando el sensor no esté activo y a true cuando lo esté
    Collision2D m_collisionobj;

	//Dibujado 
	//Medir en un solo eje (X o Y)
	//Lanzamiento de eventos
	//Repasar código
	private void OnCollisionEnter2D(Collision2D collision)
    {
		if (m_isChecking)
		{
			col = true;
			m_collisionobj = collision;
			EventDetected();
			Debug.Log("message sent");
		}
       
	}
	public override bool CanTransition()
    {
		if(col)m_isChecking = false;
        return col;
    }
    public override void StartSensor() 
	{
		
		m_isChecking = true;
		col = false;
	}

	Collision2D GetCollidedObject(){ return m_collisionobj; }
}
