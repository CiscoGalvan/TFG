using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Actuator: MonoBehaviour 
{
    public abstract void UpdateActuator();
    public abstract void StartActuator();
    public abstract void Destroy();
}
