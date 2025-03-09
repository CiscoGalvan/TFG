using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Actuator: MonoBehaviour
{
    //hasset of sensors needed by the actuator
    protected HashSet<Sensors> sensors = new HashSet<Sensors>();
    protected Animator _animator;
	public abstract void UpdateActuator();
    public abstract void StartActuator(Animator _animator);
    public abstract void DestroyActuator();
    public HashSet<Sensors> GetSensors() { return sensors; }
}
