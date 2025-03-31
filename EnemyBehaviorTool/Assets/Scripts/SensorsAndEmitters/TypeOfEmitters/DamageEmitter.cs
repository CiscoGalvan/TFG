using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DamageEmitter;

[RequireComponent(typeof(Collider2D))]
public class DamageEmitter : MonoBehaviour
{
    // Enum representing different types of damage
    public enum DamageType
	{
        Instant,    // Damage is applied immediately
        Persistent, // Damage is applied over time
        Residual    // Damage is applied in multiple instances after the initial hit
    }
    [SerializeField,HideInInspector]
    private DamageType _damageType; // Stores the selected type of damage

    [SerializeField, HideInInspector]
    private bool _instaKill = false; // If true, the target is instantly killed

    [SerializeField, HideInInspector]
    private float _amountOfDamage = 0; // Damage amount for Instant, Persistent, and Residual types

    #region Persistent Damage Variables

    [SerializeField, HideInInspector]
    private float _damageCooldown = 1f; // Time interval between damage applications for Persistent damage

    #endregion
    #region Residual Damage Variables
    [SerializeField, HideInInspector]
    private int _numOfDamageApplication = 2; // Number of times residual damage is applied

    [SerializeField, HideInInspector]
    private float _residualDamageAmount = 0; // Damage amount per application for Residual damage

    #endregion

    private bool _endPersistentDamage; // Tracks whether persistent damage has ended

    [SerializeField, HideInInspector]
    private Collider2D _damageEmitterCollider;

	[SerializeField, HideInInspector]
	private bool _moreThanOneCollider;

    [SerializeField,HideInInspector]
    private bool _destroyAfterDoingDamage = false;
    #region Getters and Setters

    // Sets the amount of residual damage
    public void SetResidualDamageAmount(float newValue)
    {
        _residualDamageAmount = newValue;
    }

    // Returns the amount of residual damage
    public float GetResidualDamageAmount() => _residualDamageAmount;

    // Returns the damage type
    public DamageType GetDamageType() => _damageType;

    // Returns the base damage amount
    public float GetAmountOfDamage() => _amountOfDamage;

    // Sets the base damage amount
    public void SetAmountOfDamage(float newValue)
    {
        _amountOfDamage = newValue;
    }

    // Returns the cooldown between persistent damage applications
    public float GetDamageCooldown() => _damageCooldown;

    // Returns the number of times residual damage is applied
    public int GetNumberOfResidualApplication() => _numOfDamageApplication;

    // Sets the number of residual damage applications
    public void SetNumberOfResidualApplication(int newValue)
    {
        _numOfDamageApplication = newValue;
    }

    // Sets the cooldown duration for persistent damage
    public void SetDamageCooldown(float newValue)
    {
        _damageCooldown = newValue;
    }

    // Returns whether the damage is instant kill
    public bool GetInstaKill() => _instaKill;

    // Sets whether the damage is instant kill
    public void SetInstaKill(bool newValue)
    {
        _instaKill = newValue;
    }

    // Returns whether persistent damage has ended
    public bool EndPersistentDamage() => _endPersistentDamage;

	#endregion

	private void Start()
	{
		if(_damageEmitterCollider == null)
        {
            _damageEmitterCollider = GetComponent<Collider2D>();
        }
	}

    public Collider2D GetDamageEmitterCollider() => _damageEmitterCollider;
    public bool GetDestroyAfterDoingDamage() => _destroyAfterDoingDamage;
}
