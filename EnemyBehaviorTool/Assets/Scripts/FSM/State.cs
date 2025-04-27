using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

[System.Serializable]
public class Transition
{
    public Sensor sensor; // Sensor that triggers the transition
    public State targetState; // Target state to transition to
}

public class State : MonoBehaviour
{
    [SerializeField]
    public List<Actuator> actuatorList = new List<Actuator>(); // List of actuators associated with the state

    private int _numElementsActuator = -1; // Tracks the number of actuators

    // List of sensors that can trigger a transition
    [SerializeField]
    private List<Transition> _transitionList = new List<Transition>();

    private State _nextState = null; // Tracks the next state to transition to

    [SerializeField]
    private List<DamageEmitter> _damageEmitterList = new List<DamageEmitter>(); // List of damage emitters in the state

    [SerializeField]
    [Tooltip("It determines whether the debug elements from the actuators and sensors included in this state are visible or not")]
    private bool _debugState = true;

    // Starts the state by initializing actuators, sensors, and damage emitters
    public void StartState()
    {
        foreach (var actuator in actuatorList)
        {
            if (actuator)
            {
                actuator.StartActuator();
            }
        }

        // Start all sensors in the transition list
        foreach (var pair in _transitionList)
        {
            if (pair.sensor != null)
            {
                pair.sensor.StartSensor();
            }
        }

        foreach (var damageEmitter in _damageEmitterList)
        {
            if (damageEmitter)
                damageEmitter.SetEmitting(true);
        }

        SubscribeToSensorEvents();
    }

    // Destroys the state by stopping actuators, sensors, and damage emitters
    public void DestroyState()
    {
        foreach (var actuator in actuatorList)
        {
            actuator.DestroyActuator();
        }

        _nextState = null;
        UnsubscribeFromSensorEvents();

        foreach (var pair in _transitionList)
        {
            if (pair.sensor != null)
            {
                pair.sensor.StopSensor();
            }
        }

        foreach (var damageEmitter in _damageEmitterList)
        {
            if (damageEmitter)
                damageEmitter.SetEmitting(false);
        }
    }

    // Updates the state by invoking the Update method on all actuators
    public void UpdateState()
    {
        foreach (Actuator a in actuatorList)
        {
            a.UpdateActuator();
        }
    }

    // Checks if any transition conditions are met and returns the next state
    public State CheckTransitions()
    {
        return _nextState;
    }

    // Subscribes to sensor events in the transition list
    private void SubscribeToSensorEvents()
    {
        foreach (var pair in _transitionList)
        {
            if (pair.sensor != null && pair.targetState != null) // Ensure data is not null
            {
                pair.sensor.onEventDetected += SensorTriggeredWrapper;
            }
        }
    }

    // Unsubscribes from sensor events in the transition list
    private void UnsubscribeFromSensorEvents()
    {
        foreach (var pair in _transitionList)
        {
            if (pair.sensor != null)
            {
                pair.sensor.onEventDetected -= SensorTriggeredWrapper;
            }
        }
    }

    // Wrapper method for handling sensor-triggered events
    private void SensorTriggeredWrapper(Sensor sensor)
    {
        foreach (var pair in _transitionList)
        {
            if (pair.sensor == sensor)
            {
                SensorTriggered(pair);
                break;
            }
        }
    }

    // Handles the transition when a sensor is triggered
    private void SensorTriggered(Transition pair)
    {
        _nextState = pair.targetState;
    }

    // Deactivates all actuators in the state
    public void DeactivateAllActuators()
    {
        foreach (var actuator in actuatorList)
        {
            if (actuator != null)
            {
                actuator.enabled = false;
            }
        }
    }

    #region Editor

    // Called when changes are made in the Unity Editor
    private void OnValidate()
    {
        // Ensure there are no duplicate actuators or sensors in the list
        if (actuatorList.Count != _numElementsActuator)
            _numElementsActuator = VerificarLista(actuatorList, "actuatorList");

        foreach (var actuator in actuatorList)
        {
            if (actuator != null)
            {
                actuator.SetDebug(_debugState);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(actuator);
#endif
            }
        }

        foreach (var sensor in _transitionList)
        {
            if (sensor.sensor != null)
            {
                sensor.sensor.SetDebug(_debugState);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(sensor.sensor);
#endif
            }
        }
    }

    // Verifies the list for duplicates and warns if duplicates are found
    private int VerificarLista<T>(List<T> lista, string nombreLista)
    {
        if (lista == null || lista.Count <= 1) // Not enough elements to verify
        {
            return -1;
        }

        // Get the type of the last element
        Type ultimoTipo = lista[lista.Count - 1]?.GetType();

        if (ultimoTipo == null) // If it's null, do nothing
        {
            return -1;
        }

        // Check if it's the same type as any other in the list
        for (int i = 0; i < lista.Count - 1; i++)
        {
            if (lista[i] != null && lista[i].GetType() == ultimoTipo)
            {
                Debug.LogWarning($"An attempt was made to add a second {nombreLista.TrimEnd('s')} of type {ultimoTipo.Name}");
                lista[lista.Count - 1] = default; // Leave it created but without a type

                return lista.Count;
            }
        }

        return lista.Count;
    }

    #endregion

    // Returns the list of damage emitters
    public List<DamageEmitter> GetDamageEmitters() => _damageEmitterList;
}