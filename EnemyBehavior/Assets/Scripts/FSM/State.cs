using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    [SerializeField]
     public List<Actuator> actuatorList = new List<Actuator>();

    [SerializeField]
    public List<Sensors> SensorList = new List<Sensors>();


    public string name;
    public void StartState()
    {
        foreach (var actuator in actuatorList)
        {
            actuator.Start();
        }
        foreach (var sensor in SensorList)
        {
            sensor.Start();
        }
    }

    // Update is called once per frame
    public void UpdateState()
    {
        foreach (Actuator a in actuatorList)
        {
            a.UpdateActuator();
        }
        
    }

}
