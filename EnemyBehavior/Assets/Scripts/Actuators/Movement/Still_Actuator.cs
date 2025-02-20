using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Still_Actuator : Actuator
{

	private Rigidbody2D m_rigidbody;
	private Damage_Sensor _damageSensor;
	public override void DestroyActuator()
	{
		
	}

	public override void StartActuator()
	{
		m_rigidbody = this.GetComponent<Rigidbody2D>();
		_damageSensor = this.GameObject().GetComponent<Damage_Sensor>();
		if (_damageSensor != null)
		{
			_damageSensor.onEventDetected += HarmPlayer;
		}
	}

	public override void UpdateActuator()
	{
		
	}

	//Esto tiene que ir aqui?
	void HarmPlayer(Sensors s)
	{
		if (_damageSensor.GetCollisionObject().gameObject.tag == "Player")
		{
			if (!_damageSensor.GetInstaKill())
			{
				_damageSensor.GetCollisionObject().gameObject.SendMessage("DecreaseLife", _damageSensor.GetAmountOfDamage());
			}
			else _damageSensor.GetCollisionObject().gameObject.SendMessage("InstantKill");

		}
	}

}
