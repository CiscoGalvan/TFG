using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    [Header("FSM Configuration")]
    [Tooltip("Defines the initial state of the FSM.")]
    [SerializeField]
    private State initialState;

    private State _currentstate; // Stores the current active state

    void Awake()
    {
        // Set the initial state and execute its start logic
        _currentstate = initialState;
        _currentstate.StartState();
    }

    void Update()
    {
        // Executes the logic of the current state in each frame
        _currentstate.UpdateState();
    }
  
     private void OnDestroy()
    {
        // Executes the exit actions of the last state when the FSM is destroyed
        _currentstate.DestroyState();
    }

    void LateUpdate()
    {
        // Checks for state transitions after all updates are processed
        State newState = _currentstate.CheckTransitions();
        if (newState != null && newState != _currentstate)
        {
            ChangeState(newState);
        }
    }

    /// <summary>
    /// Changes the current state to a new state.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
    private void ChangeState(State newState)
    {
        // Execute exit actions of the current state
        _currentstate.DestroyState();

        // Set the new state and execute its start logic
        _currentstate = newState;
        _currentstate.StartState();
    }
}

