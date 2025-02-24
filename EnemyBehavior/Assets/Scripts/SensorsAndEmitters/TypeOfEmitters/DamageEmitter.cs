using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DamageEmitter;

public class DamageEmitter : MonoBehaviour
{
	public enum DamageType
	{
		Instant,
		Persistent,
		Residual
	}
	[SerializeField]
	private DamageType _damageType;

	[SerializeField, HideInInspector]
	private bool _instaKill = false;
	[SerializeField,HideInInspector]
	private float _amountOfDamage = 0; //para tipo 0,1 y 2
	#region Persistent Damage Variables

	[SerializeField,HideInInspector]
	private float _damageCooldown = 1f; // para tipo  2
	#endregion
	#region Residual Damage Variables
	[SerializeField,HideInInspector]
	private int _numOfDamageApplication= 2; //cuantas veces haces daño
	[SerializeField, HideInInspector]
	private float _residualDamageAmount = 0;
	#endregion
	private bool _endPersistentDamage;
	#region Getters and setters
	public void SetResidualDamageAmount(float newValue)
	{
		_residualDamageAmount = newValue;
	}
	public float GetResidualDamageAmount() => _residualDamageAmount;
	public DamageType GetDamageType() => _damageType;
	public float GetAmountOfDamage() => _amountOfDamage;
	public void SetAmountOfDamage(float newValue)
	{
		_amountOfDamage = newValue;
	}
	public float GetDamageCooldown() => _damageCooldown;
	public int GetNumberOfResidualApplication() => _numOfDamageApplication;
	public void SetNumberOfResidualApplication(int newValue) 
	{
		_numOfDamageApplication = newValue;
	}
	public void SetDamageCooldown(float newValue)
	{
		_damageCooldown = newValue;
	}
	public bool GetInstaKill() => _instaKill;
	public void SetInstaKill(bool newValue)
	{
		_instaKill = newValue;
	}
	public bool EndPersistentDamage() => _endPersistentDamage;
	#endregion
}
