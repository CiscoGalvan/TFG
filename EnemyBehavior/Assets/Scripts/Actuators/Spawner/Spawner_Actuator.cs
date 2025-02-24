using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Actuator : Actuator
{
    
    [SerializeField]
    // [HideInInspector]
    private bool _infiniteEnemies = true;
    [SerializeField,HideInInspector]
    private int _numOfEnemiesToSpawn = 0;

    [SerializeField]
    private GameObject _prefabToSpawn;

    [SerializeField]
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
        if (_timerSensor == null)
        {
            _timerSensor = this.gameObject.AddComponent<Timer_Sensor>();
        }
        _timerSensor.onEventDetected += SpawnEvent;
        sensors.Add(_timerSensor);
        _numEnemiesAlreadySpawn = 0;
    }

    void SpawnEvent(Sensors s)
    {
        if (_infiniteEnemies || _numEnemiesAlreadySpawn < _numOfEnemiesToSpawn)
        {
			_numEnemiesAlreadySpawn++;
            GameObject newEnemy = Instantiate(_prefabToSpawn, _spawnPoint.position,_spawnPoint.rotation);
        }
    }
	public override void UpdateActuator()
	{

	}

	#region Setters and getters
	public bool GetInfiniteEnemies() => _infiniteEnemies;
	public void SetNumberOfEnemiesToSpawn(int newValue)
    {
        _numOfEnemiesToSpawn = newValue;
	}
    public int GetNumberOfEnemiesToSpawn() => _numOfEnemiesToSpawn;
	#endregion
}
