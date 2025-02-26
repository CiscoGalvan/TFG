using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    [SerializeField]
    public State initialState;

    private State _currentstate;

    // Necesitamos acciones al entrar al estado y al salir de él, las de durante las tenemos.

	void Awake()
    {
        _currentstate = initialState;
        _currentstate.StartState();
	}

    void Update()
    {
        //update of the state
        _currentstate.UpdateState();
    }
  
    private void OnDestroy()
    {
        // Hace las acciones de salida del ultimo estado.
        _currentstate.DestroyState();
    }
    void LateUpdate()
    {
        //cambio de estados una vez se ha actualizado todo
        State newState = _currentstate.CheckTransitions();
        if (newState != null && newState != _currentstate)
        {
            ChangeState(newState);
        }
    }

    private void ChangeState(State newState)
    {
        _currentstate.DestroyState();
        _currentstate = newState;
        _currentstate.StartState();
    }
}

