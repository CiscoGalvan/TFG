using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    [SerializeField]
    public State m_initialstate;

    private State m_currentstate;
	//public static List<Action> eventsToProcess;


	// Start is called before the first frame update
	void Awake()
    {
        m_currentstate = m_initialstate;
        m_currentstate.StartState();
		//eventsToProcess = new List<Action>();
	}

    // Update is called once per frame
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

}

