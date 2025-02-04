using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//clase base de la que heredan todos los actuadores
public abstract class Actuator: MonoBehaviour 
{
    public abstract void Update();
    public abstract void Start();
    public abstract void Destroy();
}
