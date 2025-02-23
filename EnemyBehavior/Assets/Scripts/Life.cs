using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(Damage_Sensor))]
public class Life : MonoBehaviour
{
	
	[SerializeField]
	private float m_initialLife = 5;

	private float m_currentLife;
	[SerializeField]
	private TextMeshProUGUI lifeText;
	[SerializeField]
	private string textname;


	private bool m_update = false;
	private float m_amount = -1;

	private float _residualDamageAmount = 0;
	private Damage_Sensor _sensor;
	private DamageEmitter _damageEmitter;
	private float _actualDamageCooldown = -1;

	private float _damageCooldown = 0;
	private int _numOfDamage;
	private  void Awake()
	{
		// Validar que lifeText tenga un valor asignado
		if (lifeText == null)
		{
			Debug.LogError($"The TextMeshProUGUI reference in {gameObject.name} is not assigned. Please assign it in the inspector.", this);
			enabled = false; // Desactiva el script si no está configurado correctamente
		}
	}
	private void Start()
	{
		m_currentLife = m_initialLife;
		_sensor = GetComponent<Damage_Sensor>();
		_sensor.onEventDetected += ReceiveDamageEmitter;
		_actualDamageCooldown = 0f;
		UpdateLifeText();
	}

	private void Update()
	{
		if (m_update)
		{
			switch (_damageEmitter.GetDamageType())
			{
				case DamageEmitter.DamageType.Persistent:
					{
						_actualDamageCooldown += Time.deltaTime;
						if (_actualDamageCooldown > _damageCooldown)
						{
							DecreaseLife(m_amount);
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

		if(m_currentLife <=0)
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
							m_amount = _damageEmitter.GetAmountOfDamage();
							DecreaseLife(m_amount);
							m_update = true;
							_damageCooldown = _damageEmitter.GetDamageCooldown();
						}
						break;
					case DamageEmitter.DamageType.Residual:
						{
							m_amount = _damageEmitter.GetAmountOfDamage();
							m_update = true;
							_residualDamageAmount = _damageEmitter.GetResidualDamageAmount();
							_damageCooldown = _damageEmitter.GetDamageCooldown();
							_numOfDamage = _damageEmitter.GetNumberOfResidualApplication();
							DecreaseLife(m_amount);
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
				m_update = false;
				_actualDamageCooldown = 0;
			}

		}

	}
	private void DecreaseLife(float num)
	{
		m_currentLife -= num;
		Debug.Log("Life = " + m_currentLife);
		UpdateLifeText();
	}
	private void InstantKill()
	{
		m_currentLife = 0;
		Debug.Log("Life = " + m_currentLife);
		UpdateLifeText();
	}
	private void IncreaseLife(float num)
	{
		m_currentLife += num;
		UpdateLifeText();
	}
	private void SetLife(float num)
	{
		m_currentLife = num;
		UpdateLifeText();
	}
	private void SetInitialLife()
	{
		m_currentLife = m_initialLife;
		UpdateLifeText();
	}
	private void UpdateLifeText()
	{
		if (lifeText != null)
		{
			lifeText.text = textname + m_currentLife;
		}
	}
	private bool IsLifeLessThan(int value)
	{
		return m_currentLife < value;
	}

}
