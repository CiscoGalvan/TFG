using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Actuator : Actuator
{
    
    [SerializeField]
    // [HideInInspector]
    private bool infiniteEnemies = true;
    [SerializeField,HideInInspector]
    private int numOfEnemiesToSpawn = 0;

    [SerializeField]
   // [HideInInspector]
    private GameObject prefabToSpawn;

    [SerializeField]
    // [HideInInspector]
    private Transform spawnPoint;

    private Timer_Sensor timerSensor;

    private int numEnemiesAlreadySpawn;
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
        if (timerSensor == null)
        {
            timerSensor = this.gameObject.AddComponent<Timer_Sensor>();
        }
        timerSensor.onEventDetected += SpawnEvent;
        sensors.Add(timerSensor);
        numEnemiesAlreadySpawn = 0;
    }

    void SpawnEvent(Sensors s)
    {
        if (infiniteEnemies || numEnemiesAlreadySpawn < numOfEnemiesToSpawn)
        {
			numEnemiesAlreadySpawn++;
            GameObject newEnemy = Instantiate(prefabToSpawn, spawnPoint.position,spawnPoint.rotation);
        }
    }

    #region Setters and getters
    public bool GetInfiniteEnemies() => infiniteEnemies;
	public void SetNumberOfEnemiesToSpawn(int newValue)
    {
        numOfEnemiesToSpawn = newValue;
	}
    public int GetNumberOfEnemiesToSpawn() => numOfEnemiesToSpawn;

    public override void UpdateActuator()
    {
        throw new System.NotImplementedException();
    }
    #endregion

}
