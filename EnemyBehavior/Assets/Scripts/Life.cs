using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(Damage_Sensor))]
public class Life : MonoBehaviour
{
	
	[SerializeField]
	private float _initialLife = 5;

	private float _currentLife;
	[SerializeField]
	private TextMeshProUGUI _lifeText;
	[SerializeField]
	private string _textname;


	private bool _update = false;
	private float _amount = -1;

	private float _residualDamageAmount = 0;
	private Damage_Sensor _sensor;
	private DamageEmitter _damageEmitter;
	private float _actualDamageCooldown = -1;

	private float _damageCooldown = 0;
	private int _numOfDamage;
	private  void Awake()
	{
		// Validar que lifeText tenga un valor asignado
		if (_lifeText == null)
		{
			Debug.LogError($"The TextMeshProUGUI reference in {gameObject.name} is not assigned. Please assign it in the inspector.", this);
			enabled = false; // Desactiva el script si no está configurado correctamente
		}
	}
	private void Start()
	{
		_currentLife = _initialLife;
		_sensor = GetComponent<Damage_Sensor>();
		_sensor.onEventDetected += ReceiveDamageEmitter;
		_actualDamageCooldown = 0f;
		UpdateLifeText();
	}

	private void Update()
	{
		if (_update)
		{
			switch (_damageEmitter.GetDamageType())
			{
				case DamageEmitter.DamageType.Persistent:
					{
						_actualDamageCooldown += Time.deltaTime;
						if (_actualDamageCooldown > _damageCooldown)
						{
							DecreaseLife(_amount);
							_actualDamageCooldown = 0;
						}
					}
					break;
				case DamageEmitter.DamageType.Residual:
					{
						if (_numOfDamage > 0)
						{
							_actualDamageCooldown += Time.deltaTime;
							if (_actualDamageCooldown > _damageCooldown)
							{
								_numOfDamage--;
								_actualDamageCooldown = 0;
								DecreaseLife(_residualDamageAmount);
							}
						}
					}
					break;
			}
		}

		if(_currentLife <=0)
		{
			Destroy(this.gameObject);
		}
	}
	
	private void OnDestroy()
	{
		_sensor.onEventDetected -= ReceiveDamageEmitter;
	}

	private void ReceiveDamageEmitter(Sensors damageSensor)
	{
		_damageEmitter = (damageSensor as Damage_Sensor).GetDamageEmitter();
		if (_damageEmitter != null)
		{
			if (_sensor.HasCollisionOccurred())
			{
				switch (_damageEmitter.GetDamageType())
				{
					case DamageEmitter.DamageType.Instant:
						if (_damageEmitter.GetInstaKill())
							InstantKill();
						else
							DecreaseLife(_damageEmitter.GetAmountOfDamage());
						break;
					case DamageEmitter.DamageType.Persistent:
						{
							_amount = _damageEmitter.GetAmountOfDamage();
							DecreaseLife(_amount);
							_update = true;
							_damageCooldown = _damageEmitter.GetDamageCooldown();
						}
						break;
					case DamageEmitter.DamageType.Residual:
						{
							_amount = _damageEmitter.GetAmountOfDamage();
							_update = true;
							_residualDamageAmount = _damageEmitter.GetResidualDamageAmount();
							_damageCooldown = _damageEmitter.GetDamageCooldown();
							_numOfDamage = _damageEmitter.GetNumberOfResidualApplication();
							DecreaseLife(_amount);
							_actualDamageCooldown = 0;
						}
						break;
					default:
						break;
				}
			}
			else
			{
				// Reinciar variables.
				_update = false;
				_actualDamageCooldown = 0;
			}

		}

	}
	private void DecreaseLife(float num)
	{
		_currentLife -= num;
		Debug.Log("Life = " + _currentLife);
		UpdateLifeText();
	}
	private void InstantKill()
	{
		_currentLife = 0;
		Debug.Log("Life = " + _currentLife);
		UpdateLifeText();
	}
	private void IncreaseLife(float num)
	{
		_currentLife += num;
		UpdateLifeText();
	}
	private void SetLife(float num)
	{
		_currentLife = num;
		UpdateLifeText();
	}
	private void SetInitialLife()
	{
		_currentLife = _initialLife;
		UpdateLifeText();
	}
	private void UpdateLifeText()
	{
		if (_lifeText != null)
		{
			_lifeText.text = _textname + _currentLife;
		}
	}
	private bool IsLifeLessThan(int value)
	{
		return _currentLife < value;
	}

}
