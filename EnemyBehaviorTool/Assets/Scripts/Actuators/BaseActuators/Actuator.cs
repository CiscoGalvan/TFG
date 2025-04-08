using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Actuator: MonoBehaviour
{
    //hasset of sensors needed by the actuator
    protected HashSet<Sensor> sensors = new HashSet<Sensor>();
   
    protected bool _debugActuator;
	public abstract void UpdateActuator();
    public abstract void StartActuator();
    public abstract void DestroyActuator();
    public HashSet<Sensor> GetSensors() { return sensors; }

	public void SetDebug(bool debug)
	{
		_debugActuator = debug;
	}
}
