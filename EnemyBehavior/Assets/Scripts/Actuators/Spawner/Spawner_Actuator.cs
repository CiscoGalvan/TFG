using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Spawner_Actuator : Actuator
{
    
    [SerializeField]
    // [HideInInspector]
    private bool m_infiniteEnemies = true;
    [SerializeField]
    // [HideInInspector]
    private int m_numOfEnemiesToSpawn = 1;

    [SerializeField]
   // [HideInInspector]
    private GameObject _prefabToSpawn;

    [SerializeField]
    // [HideInInspector]
    private Transform m_pointToSpawn;

    Timer_Sensor timerSensor;

    private int m_numEnemiesAlrreadySpawn;
    // Update is called once per frame
    public override void DestroyActuator()
    {
        if (timerSensor != null)
        {
            timerSensor.onEventDetected -= SpawnEvent;
        }

    }
    // Start is called before the first frame update
    public override void StartActuator()
    {
       timerSensor = this.gameObject.GetComponent<Timer_Sensor>();
        if (timerSensor != null)
        {
            timerSensor.onEventDetected += SpawnEvent;
        }
        m_numEnemiesAlrreadySpawn = 0;
    }

    public override void UpdateActuator()
    {
       
    }

    void SpawnEvent(Sensors s)
    {
        if (m_infiniteEnemies || m_numEnemiesAlrreadySpawn < m_numOfEnemiesToSpawn)
        {
            m_numEnemiesAlrreadySpawn++;
            //hacer el new del prefab en el transform indicado
            GameObject newEnemy = Instantiate(_prefabToSpawn, m_pointToSpawn.position, m_pointToSpawn.rotation);
            
        }
    }

}
