using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    [SerializeField]
    public List<Actuator> actuatorList = new List<Actuator>();

    [SerializeField]
    public List<Sensors> SensorList = new List<Sensors>();

    [SerializeField]
    public string name = "State";
    public void StartState()
    {
        //Debug.Log(name);
        foreach (var actuator in actuatorList)
        {
            if(actuator)
                actuator.StartActuator();
        }
        foreach (var sensor in SensorList)
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
       SensorList.Add(sen); 
    }
}
