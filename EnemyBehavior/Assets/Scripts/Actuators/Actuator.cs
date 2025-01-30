using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//clase base de la que heredan todos los actuadores
public class Actuator : ScriptableObject
{
    public virtual void OnDrawGizmosSelected() {
        Debug.Log("actuator");
    }
}
