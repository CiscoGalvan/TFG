using UnityEngine;


public abstract class Actuator: MonoBehaviour
{
      
    protected bool _debugActuator;
	public abstract void UpdateActuator();
    public abstract void StartActuator();
    public abstract void DestroyActuator();

	public void SetDebug(bool debug)
	{
		_debugActuator = debug;
	}
}
