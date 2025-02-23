using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Actuator : Actuator
{
    
    [SerializeField]
    // [HideInInspector]
    private bool _infiniteEnemies = true;
    [SerializeField,HideInInspector]
    private int m_numOfEnemiesToSpawn = 0;

    [SerializeField]
   // [HideInInspector]
    private GameObject _prefabToSpawn;

    [SerializeField]
    // [HideInInspector]
    private Transform _spawnPoint;

    private Timer_Sensor _timerSensor;

    private int _numEnemiesAlreadySpawn;
    // Update is called once per frame
    public override void DestroyActuator()
    {
        if (_timerSensor != null)
        {
			_timerSensor.onEventDetected -= SpawnEvent;
        }

    }
    // Start is called before the first frame update
    public override void StartActuator()
    {
		_timerSensor = this.gameObject.GetComponent<Timer_Sensor>();
        if (_timerSensor != null)
        {
			_timerSensor.onEventDetected += SpawnEvent;
        }
		_numEnemiesAlreadySpawn = 0;
    }

    public override void UpdateActuator()
    {
       
    }

    void SpawnEvent(Sensors s)
    {
        if (_infiniteEnemies || _numEnemiesAlreadySpawn < m_numOfEnemiesToSpawn)
        {
			_numEnemiesAlreadySpawn++;
            GameObject newEnemy = Instantiate(_prefabToSpawn, _spawnPoint.position, _spawnPoint.rotation);
        }
    }

    #region Setters and getters
    public bool GetInfiniteEnemies() => _infiniteEnemies;
	public void SetNumberOfEnemiesToSpawn(int newValue)
    {
        m_numOfEnemiesToSpawn = newValue;
	}
    public int GetNumberOfEnemiesToSpawn() => m_numOfEnemiesToSpawn;
    #endregion

}
