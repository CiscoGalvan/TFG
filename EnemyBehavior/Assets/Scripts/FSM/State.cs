using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class State : MonoBehaviour
{
    [SerializeField]
    public List<Actuator> actuatorList = new List<Actuator>();
    private int _numElementsActuator = -1;

    
    public HashSet<Sensors> sensorHashSet = new HashSet<Sensors>();
    private int _numElementsSensor = -1;

    [SerializeField]
    public string name = "State";
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
        foreach (var sensor in sensorHashSet)
        {
            // This conditional is used to check when the list size is not zero and there is no sensor in it
            if(sensor)
                sensor.StartSensor();
        }
    }
	public void DestroyState()
	{
		
		foreach (var actuator in actuatorList)
		{
            actuator.DestroyActuator();
		}
        //Pueden tener este mismo destroy?
		//foreach (var sensor in SensorList)
		//{
		//	sensor.StartSensor();
		//}
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
