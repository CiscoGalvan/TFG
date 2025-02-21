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
	[SerializeField]
	private float _amountOfDamage = 0; //para tipo 0,1 y 2
	#region Persistent Damage Variables

	[SerializeField]
	private float _damageCooldown = 1f; // para tipo  2
	#endregion
	#region Residual Damage Variables
	[SerializeField]
	private int _numOfDamage = 2; //cuantas veces haces daño
	#endregion
	private bool m_endPersistentDamage;
	public DamageType GetDamageType() => _damageType;

	public float GetAmountOfDamage() => _amountOfDamage;
	public void SetAmountOfDamage(float newValue)
	{
		_amountOfDamage = newValue;
	}
	public float GetDamageCooldown() => _damageCooldown;
	public int GetNumOfDamage() => _numOfDamage;

	public void SetDamageCooldown(float newValue)
	{
		_damageCooldown = newValue;
	}
	public bool GetInstaKill() => _instaKill;
	public void SetInstaKill(bool newValue)
	{
		_instaKill = newValue;
	}

	public bool EndPersistentDamage() => m_endPersistentDamage;
}
