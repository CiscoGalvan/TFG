using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sensors/*<T>*/ : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public State destinationState;

    [SerializeField]
    protected bool m_transition;
    
	public event Action/*<T>*/ onEventDetected;
	public abstract void StartSensor();
    // Update is called once per frame
    public abstract bool CanTransition();
    public bool WantTransition() { return m_transition; }
	//protected void TriggerEvent(T data)
	//{
	//	onEventDetected?.Invoke(data);
	//}
}
