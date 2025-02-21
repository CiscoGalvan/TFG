using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    [SerializeField]
    public State m_initialstate;

    private State m_currentstate;

    // Necesitamos acciones al entrar al estado y al salir de él, las de durante las tenemos.

	void Awake()
    {
        m_currentstate = m_initialstate;
        m_currentstate.StartState();
	}

    void Update()
    {
        //update of the state
        m_currentstate.UpdateState();
   //     foreach (var sensor in m_currentstate.SensorList)
   //     {
   //         if (sensor.WantTransition())
   //         {
			//	bool changeState = sensor.CanTransition();
			//	if (changeState)
			//	{
   //                 m_currentstate.DestroyState();
			//		m_currentstate = sensor.destinationState;
			//		m_currentstate.StartState();
			//		break;
			//	}
			//}
   //     }
    }
    private void LateUpdate()
    {
        //cambio de estados una vez se ha actualizado todo

    }

	private void OnDestroy()
	{
	    // Hace las acciones de salida del ultimo estado.
	}
}

