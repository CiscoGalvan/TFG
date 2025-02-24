using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(Damage_Sensor))]
public class Life : MonoBehaviour
{
	
	[SerializeField]
	private float initialLife = 5;

	private float currentLife;
	[SerializeField]
	private TextMeshProUGUI lifeText;
	[SerializeField]
	private string textname;


	private bool update = false;
	private float amount = -1;

	private float residualDamageAmount = 0;
	private Damage_Sensor sensor;
	private DamageEmitter damageEmitter;
	private float actualDamageCooldown = -1;

	private float damageCooldown = 0;
	private int numOfDamage;
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
		currentLife = initialLife;
		sensor = GetComponent<Damage_Sensor>();
		sensor.onEventDetected += ReceiveDamageEmitter;
		actualDamageCooldown = 0f;
		UpdateLifeText();
	}

	private void Update()
	{
		if (update)
		{
			switch (damageEmitter.GetDamageType())
			{
				case DamageEmitter.DamageType.Persistent:
					{
						actualDamageCooldown += Time.deltaTime;
						if (actualDamageCooldown > damageCooldown)
						{
							DecreaseLife(amount);
							actualDamageCooldown = 0;
						}
					}
					break;
				case DamageEmitter.DamageType.Residual:
					{
						if (numOfDamage > 0)
						{
							actualDamageCooldown += Time.deltaTime;
							if (actualDamageCooldown > damageCooldown)
							{
								numOfDamage--;
								actualDamageCooldown = 0;
								DecreaseLife(residualDamageAmount);
							}
						}
					}
					break;
			}
		}

		if(currentLife <=0)
		{
			Destroy(this.gameObject);
		}
	}
	
	private void OnDestroy()
	{
		sensor.onEventDetected -= ReceiveDamageEmitter;
	}

	private void ReceiveDamageEmitter(Sensors damageSensor)
	{
		damageEmitter = (damageSensor as Damage_Sensor).GetDamageEmitter();
		if (damageEmitter != null)
		{
			if (sensor.HasCollisionOccurred())
			{
				switch (damageEmitter.GetDamageType())
				{
					case DamageEmitter.DamageType.Instant:
						if (damageEmitter.GetInstaKill())
							InstantKill();
						else
							DecreaseLife(damageEmitter.GetAmountOfDamage());
						break;
					case DamageEmitter.DamageType.Persistent:
						{
							amount = damageEmitter.GetAmountOfDamage();
							DecreaseLife(amount);
							update = true;
							damageCooldown = damageEmitter.GetDamageCooldown();
						}
						break;
					case DamageEmitter.DamageType.Residual:
						{
							amount = damageEmitter.GetAmountOfDamage();
							update = true;
							residualDamageAmount = damageEmitter.GetResidualDamageAmount();
							damageCooldown = damageEmitter.GetDamageCooldown();
							numOfDamage = damageEmitter.GetNumberOfResidualApplication();
							DecreaseLife(amount);
							actualDamageCooldown = 0;
						}
						break;
					default:
						break;
				}
			}
			else
			{
				// Reinciar variables.
				update = false;
				actualDamageCooldown = 0;
			}

		}

	}
	private void DecreaseLife(float num)
	{
		currentLife -= num;
		Debug.Log("Life = " + currentLife);
		UpdateLifeText();
	}
	private void InstantKill()
	{
		currentLife = 0;
		Debug.Log("Life = " + currentLife);
		UpdateLifeText();
	}
	private void IncreaseLife(float num)
	{
		currentLife += num;
		UpdateLifeText();
	}
	private void SetLife(float num)
	{
		currentLife = num;
		UpdateLifeText();
	}
	private void SetInitialLife()
	{
		currentLife = initialLife;
		UpdateLifeText();
	}
	private void UpdateLifeText()
	{
		if (lifeText != null)
		{
			lifeText.text = textname + currentLife;
		}
	}
	private bool IsLifeLessThan(int value)
	{
		return currentLife < value;
	}

}
