using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using UnityEngine.UIElements;

[System.Serializable]
public class SensorStatePair
{
	public Sensors sensor;
	public State targetState;
	public Action<Sensors> transitionEvent;
}

public class State : MonoBehaviour
{
	[SerializeField]
	public string stateName = "State";

	[SerializeField]
    public List<Actuator> actuatorList = new List<Actuator>();

    private int _numElementsActuator = -1;

    //hashset con todos los sensores
    public HashSet<Sensors> sensorHashSet = new HashSet<Sensors>();
    //Lista con los sensores que pueden transicionara transicionar
    [SerializeField,HideInInspector]
    private List<SensorStatePair> _sensorTransitions = new List<SensorStatePair>();
    private State _nextState = null;


    
    public void StartState()
    {
        //Debug.Log(name);
        foreach (var actuator in actuatorList)
        {
            if (actuator)
            {
                actuator.StartActuator();
                sensorHashSet.UnionWith(actuator.GetSensors());
            }
               
        }
        // Iniciar todos los sensores de _sensorTransitions
        foreach (var pair in _sensorTransitions)
        {
            if (pair.sensor != null)
			{
				pair.sensor.StartSensor();
                sensorHashSet.Add(pair.sensor); // Opcional, si quieres que también estén en sensorHashSet
			}
        }
        foreach (var sensor in sensorHashSet)
        {
            // This conditional is used to check when the list size is not zero and there is no sensor in it
            if(sensor)
                sensor.StartSensor();
        }
        SubscribeToSensorEvents();
    }
	public void DestroyState()
	{
		
		foreach (var actuator in actuatorList)
		{
            actuator.DestroyActuator();
		}
        _nextState = null;
        UnsubscribeFromSensorEvents();
    }

	// Update is called once per frame
	public void UpdateState()
    {
        foreach (Actuator a in actuatorList)
        {
            a.UpdateActuator();
        }
        
    }
    public void AddActuator( Actuator act)
    {
        actuatorList.Add(act);
    }
    public void AddSensor(Sensors sen)
    {
        sensorHashSet.Add(sen);
    }
    public State CheckTransitions()
    {
        return _nextState;
    }
	private void SubscribeToSensorEvents()
	{
		foreach (var pair in _sensorTransitions)
		{
			if (pair.sensor != null && pair.targetState != null)
			{
				SetTransitionEvent(pair); // Asegurar que transitionEvent está bien asignado

				//if (pair.transitionEvent != null)
				//{
				//	pair.transitionEvent += SensorTriggeredWrapper;
				//	Debug.Log("Evento correctamente enlazado a " + pair.sensor.name);
				//}
				//else
				//{
				//	Debug.LogError("No se pudo asignar transitionEvent en " + pair.sensor.name);
				//}
			}
		}
	}
    

	private void UnsubscribeFromSensorEvents()
    {
        foreach (var pair in _sensorTransitions)
        {
            if (pair.sensor != null)
            {
                pair.sensor.onEventDetected -= SensorTriggeredWrapper;
            }
        }
    }
	private void SensorTriggeredWrapper(Sensors sensor)
	{
        int I = 0;
		foreach (var pair in _sensorTransitions)
		{
            Debug.Log("I = " + I);
			if (pair.sensor == sensor)
			{
				SensorTriggered(pair);
				break;
			}
            I++;
		}
	}
	private void SensorTriggered(SensorStatePair pair)
    {
        Debug.Log("CAMBIO");
        _nextState = pair.targetState;
    }



	private void SetTransitionEvent(SensorStatePair pair)
	{
		if (pair.sensor == null) return;
		switch (pair.sensor)
		{
			case Collision_Sensor collisionSensor:
				switch (collisionSensor.GetSuscriberType())
				{
					case Collision_Sensor.SensorEventTypes.OnCollisionEnterEvent:
						pair.transitionEvent = collisionSensor._onCollisionEnterEvent;
						collisionSensor._onCollisionEnterEvent += SensorTriggeredWrapper; // SUSCRIPCIÓN CORRECTA
						break;
                    case Collision_Sensor.SensorEventTypes.OnPlayerCollision:
						pair.transitionEvent = collisionSensor._onPlayerCollision;
						collisionSensor._onPlayerCollision += SensorTriggeredWrapper; // SUSCRIPCIÓN CORRECTA
						break;
					default:
						pair.transitionEvent = collisionSensor._onNoCollisionEvent;
						collisionSensor._onNoCollisionEvent += SensorTriggeredWrapper; // SUSCRIPCIÓN CORRECTA
						break;
				}
				break;
			default:
				Debug.LogWarning("Sensor type not handled in SetTransitionEvent.");
				break;
		}
	}


	#region Editor listas evitar duplicados


	private void OnValidate() //metodo que se llama cuandocambiamosalgo del editor
    {
        // queremos comprobar que no existan duplicados en actuadores y sensores si la lista se ha modificado
        if(actuatorList.Count != _numElementsActuator) _numElementsActuator = VerificarLista(actuatorList, "actuatorList");
    }

    private int VerificarLista<T>(List<T> lista, string nombreLista)
    {
        if (lista == null || lista.Count <= 1)// No hay suficientes elementos para verificar
        {

            return -1;
        }
        //cogemos el ultimo elemento
        Type ultimoTipo = lista[lista.Count - 1]?.GetType();

            if (ultimoTipo == null) //si es nulo, entonces nada
            {
                return -1; 
            }

            for (int i = 0; i < lista.Count - 1; i++) //comprobamos si es igual a algun otro tipo de la lista
            {
                if (lista[i] != null && lista[i].GetType() == ultimoTipo)
                {
                    Debug.LogWarning($"Se ha intentado agregar un segundo {nombreLista.TrimEnd('s')} del tipo {ultimoTipo.Name}");
                    lista[lista.Count - 1] = default; // Dejarlo creado pero sin tipo
                 
                    return lista.Count;
                }
            }
        return lista.Count;
    }
    #endregion
}
