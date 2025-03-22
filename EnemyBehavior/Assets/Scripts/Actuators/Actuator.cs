using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Actuator: MonoBehaviour
{
    //hasset of sensors needed by the actuator
    protected HashSet<Sensors> sensors = new HashSet<Sensors>();
    protected bool _actuatorActive;
    protected bool _debugActuator;
	public abstract void UpdateActuator();
    public abstract void StartActuator(AnimatorController animatorController);
    public abstract void DestroyActuator();
    public HashSet<Sensors> GetSensors() { return sensors; }

	public void SetDebug(bool debug)
	{
		_debugActuator = debug;
	}
}
