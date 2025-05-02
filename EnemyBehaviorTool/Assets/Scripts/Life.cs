using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Ensure the GameObject has a DamageSensor component
[RequireComponent(typeof(DamageSensor))]
public class Life : MonoBehaviour
{
    // Enum to differentiate between Player and Enemy types
    public enum EntityType { Enemy, Player }

    [SerializeField, HideInInspector]
    private EntityType _entityType; // The type of entity (Player or Enemy)

    [SerializeField]
    private float _initialLife = 5; // Initial life value
    [SerializeField]
    private float _maxLife = 5; // Maximum life value

    private float _currentLife; // Current life value

    [SerializeField]
    private TextMeshProUGUI _lifeText; // UI text to display life value

    [SerializeField]
    private string _textName; // Prefix for life text display

    private bool _update = false; // Whether to update life continuously
    private float _amount = -1; // Amount of damage or healing
    private float _residualDamageAmount = 0; // Residual damage value
    private DamageSensor _sensor; // Reference to the DamageSensor component
    private DamageEmitter _damageEmitter; // Reference to the DamageEmitter component
    private float _actualDamageCooldown = -1; // Cooldown timer for damage application
    private float _damageCooldown = 0; // Damage cooldown value
    private int _numOfDamage; // Number of residual damage applications


    private void Start()
    {
        _currentLife = _initialLife; // Set current life to initial life
        _sensor = GetComponent<DamageSensor>(); // Get the DamageSensor component
        _sensor.onEventDetected += ReceiveDamageEmitter; // Subscribe to DamageSensor events
        _actualDamageCooldown = 0f; // Initialize cooldown timer
        _numOfDamage = 0; // Initialize number of residual damage applications
        UpdateLifeText(); // Update the UI text
    }

    private void Update()
    {
        // Handle damage over time or residual damage
        if (_update)
        {
            switch (_damageEmitter.GetDamageType())
            {
                case DamageEmitter.DamageType.Permanence:
                    _actualDamageCooldown += Time.deltaTime; // Increment cooldown timer
                    if (_actualDamageCooldown > _damageCooldown)
                    {
                        DecreaseLife(_amount); // Apply damage
                        _actualDamageCooldown = 0;
                    }
                    break;
                case DamageEmitter.DamageType.Residual:
                    if (_numOfDamage > 0)
                    {
                        _actualDamageCooldown += Time.deltaTime; // Increment cooldown timer
                        if (_actualDamageCooldown > _damageCooldown)
                        {
                            _numOfDamage--; // Decrease number of applications
                            _actualDamageCooldown = 0;
                            DecreaseLife(_residualDamageAmount); // Apply residual damage
                        }
                    }
                    break;
            }
        }

        // Handle entity destruction when life reaches zero
        if (_currentLife <= 0)
        {
            AnimatorManager _animatorManager = this.GetComponent<AnimatorManager>();

            if (_animatorManager == null || !_animatorManager.enabled)
            {
                Destroy(this.gameObject); // Destroy the GameObject if no animator is active
            }
            else
            {
                _animatorManager.Destroy(); // Trigger animator destruction
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from DamageSensor events
        if (_sensor != null)
            _sensor.onEventDetected -= ReceiveDamageEmitter;
    }

    private void ReceiveDamageEmitter(Sensor damageSensor)
    {
        // Handle different types of damage from the emitter
        _damageEmitter = (damageSensor as DamageSensor).GetDamageEmitter();
        if (_damageEmitter != null)
        {
            if (_sensor.HasCollisionOccurred())
            {
                switch (_damageEmitter.GetDamageType())
                {
                    case DamageEmitter.DamageType.Instant:
                        if (_damageEmitter.GetInstaKill())
                            InstantKill(); // Apply instant kill
                        else
                            DecreaseLife(_damageEmitter.GetAmountOfDamage()); // Apply instant damage
                        break;
                    case DamageEmitter.DamageType.Permanence:
                        _amount = _damageEmitter.GetAmountOfDamage(); // Set damage amount
                        DecreaseLife(_amount); // Apply initial damage
                        _update = true; // Enable damage over time
                        _damageCooldown = _damageEmitter.GetDamageCooldown(); // Set cooldown
                        break;
                    case DamageEmitter.DamageType.Residual:
                        _amount = _damageEmitter.GetAmountOfDamage(); // Set initial damage
                        _update = true; // Enable residual damage
                        _residualDamageAmount = _damageEmitter.GetResidualDamageAmount(); // Set residual damage amount
                        _damageCooldown = _damageEmitter.GetDamageCooldown(); // Set cooldown
                        _numOfDamage = _damageEmitter.GetNumberOfResidualApplication(); // Set number of applications
                        DecreaseLife(_amount); // Apply initial damage
                        _actualDamageCooldown = 0; // Reset cooldown timer
                        break;
                }
            }
            else
            {
                // Reset variables if no collision occurred
                if (_numOfDamage <= 0)
                {
                    _update = false;
                    _actualDamageCooldown = 0;
                }
            }
        }
    }

    private void DecreaseLife(float num)
    {
        // Apply damage and update life text
        AnimatorManager _animatorManager = this.GetComponent<AnimatorManager>();
        _animatorManager?.Damage(); // Trigger damage animation
        _currentLife -= num; // Decrease life
        if (_currentLife < 0)
        {
            _currentLife = 0; // Ensure life doesn't go below zero
        }
        UpdateLifeText(); // Update the UI text
    }

    private void InstantKill()
    {
        // Set life to zero and update UI text
        _currentLife = 0;
        UpdateLifeText();
    }

    public void IncreaseLife(float num)
    {
        // Increase life and ensure it doesn't exceed the maximum
        _currentLife += num;
        if (_currentLife > _maxLife)
        {
            _currentLife = _maxLife;
        }
        UpdateLifeText(); // Update the UI text
    }

    public void SetLife(float num)
    {
        // Set life to a specific value and ensure it's within valid bounds
        _currentLife = num;
        if (_currentLife > _maxLife)
        {
            _currentLife = _maxLife;
        }
        UpdateLifeText(); // Update the UI text
    }

    public void ResetLife()
    {
        // Reset life to its initial value
        _currentLife = _initialLife;
        UpdateLifeText(); // Update the UI text
    }

    public float GetInitialLife()
    {
        // Get the initial life value
        return _initialLife;
    }

    public void SetInitialLife(float value)
    {
        // Set the initial life value
        _initialLife = value;
    }

    private void UpdateLifeText()
    {
        // Update the UI text with the current life value
        if (_lifeText != null && _entityType == EntityType.Player)
        {
            _lifeText.text = _textName + _currentLife;
        }
    }

    public bool IsLifeLessThan(int value)
    {
        // Check if current life is less than a specified value
        return _currentLife < value;
    }

    public string GetTextName()
    {
        // Get the prefix for the life text
        return _textName;
    }

    public void SetTextName(string value)
    {
        // Set the prefix for the life text
        _textName = value;
    }

    public TextMeshProUGUI GetLifeText()
    {
        // Get the TextMeshProUGUI reference
        return _lifeText;
    }

    public void SetLifeText(TextMeshProUGUI value)
    {
        // Set the TextMeshProUGUI reference
        _lifeText = value;
    }

    public EntityType GetEntityType()
    {
        // Get the entity type (Player or Enemy)
        return _entityType;
    }

    public void SetEntityType(EntityType value)
    {
        // Set the entity type (Player or Enemy)
        _entityType = value;
    }
}