using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnInfo
{
    [SerializeField]
    public GameObject _prefabToSpawn;

    [SerializeField]
    public Transform _spawnPoint;
}
public class SpawnerActuator : Actuator
{

    [SerializeField, Min(0)]
    private float _spawnInterval = 5f; // Tiempo de spawn ajustable desde el editor
    [SerializeField]
    private bool _infiniteEnemies = true;
    [SerializeField, HideInInspector]
    private int _numOfEnemiesToSpawn = 0;
    [SerializeField] private List<SpawnInfo> _spawnList = new List<SpawnInfo>();
    

    private Timer _timer;
    private int _numEnemiesAlreadySpawn;
    AnimatorManager _animatorManager;
    // Update is called once per frame
    public override void DestroyActuator()
    {

    }
    // Start is called before the first frame update
    public override void StartActuator()
    {
        _timer = new Timer(_spawnInterval);
        _numEnemiesAlreadySpawn = 0;
        _timer.Start();
        _numEnemiesAlreadySpawn = 0;
        _animatorManager = this.gameObject.GetComponent<AnimatorManager>();
    }

    void SpawnEvent()
    {
        if (_infiniteEnemies || _numEnemiesAlreadySpawn < _numOfEnemiesToSpawn)
        {
            _numEnemiesAlreadySpawn++;
            foreach (var spawnInfo in _spawnList)
            {
                if (spawnInfo._prefabToSpawn != null && spawnInfo._spawnPoint != null)
                {
                    Instantiate(spawnInfo._prefabToSpawn, spawnInfo._spawnPoint.position, spawnInfo._spawnPoint.rotation);
                }
            }
            _animatorManager?.SpawnEvent();
        }
    }
	public override void UpdateActuator()
	{
        _timer.Update(Time.deltaTime);
        if (_timer.GetTimeRemaining() <= 0)
        {
            SpawnEvent();
            _timer.Start(); // Reiniciar el temporizador despu�s de cada spawn
        }
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
