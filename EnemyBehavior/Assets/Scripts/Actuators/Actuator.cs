using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Actuator: MonoBehaviour
{
    //hasset of sensors needed by the actuator
    protected HashSet<Sensors> sensors = new HashSet<Sensors>();
	public abstract void UpdateActuator();
    public abstract void StartActuator();
    public abstract void DestroyActuator();
    public HashSet<Sensors> GetSenors() { return sensors; }
}
