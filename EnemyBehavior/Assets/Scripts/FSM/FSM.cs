using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    [SerializeField]
    public State m_initialstate;

    private State m_currentstate;


    // Start is called before the first frame update
    void Awake()
    {
        m_currentstate = m_initialstate;
        m_currentstate.StartState();
    }

    // Update is called once per frame
    void Update()
    {
        //update of the state
        m_currentstate.UpdateState();
       
        Debug.Log(m_currentstate.name);
        foreach (var sensor in m_currentstate.SensorList)
        {
            bool tochanage = sensor.CanTransition();
            if (tochanage)
            {
                m_currentstate = sensor.destinationState;
                m_currentstate.StartState();
                break;

            }
        }
    }

}

