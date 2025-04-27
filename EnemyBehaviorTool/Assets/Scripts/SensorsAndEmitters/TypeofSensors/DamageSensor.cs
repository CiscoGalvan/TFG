using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageSensor : Sensor
{
    // Boolean to track if a collision has occurred
    private bool _col;
    private DamageEmitter _damageEmitter;

    [Tooltip("If true, the Damage Sensor won't need to be included in any State in order to activate itself.")]
    [SerializeField]
    private bool _activeFromStart = false;

    #region Trigger Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_sensorActive && _timerFinished)
        {
            // Check if the colliding object has a DamageEmitter component
            _damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();

            if (_damageEmitter != null && _damageEmitter.enabled && _damageEmitter.GetEmitting())
            {
                // Handle destruction if the DamageEmitter is configured to destroy after inflicting damage
                if (_damageEmitter.GetDestroyAfterDoingDamage())
                {
                    AnimatorManager _animatorManager = collision.gameObject.GetComponent<AnimatorManager>();
                    if (_animatorManager != null)
                    {
                        _animatorManager.Destroy(); // Trigger destruction animation
                    }
                    else
                    {
                        Destroy(collision.gameObject); // Destroy the object directly
                    }
                }

                _col = true; // Mark that a collision has occurred
                EventDetected(); // Notify listeners of the event
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_sensorActive && _timerFinished)
        {
            // Check if the exiting object has a DamageEmitter component
            _damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();

            if (_damageEmitter != null && _damageEmitter.enabled && _damageEmitter.GetEmitting())
            {
                _col = false; // Mark that the collision has ended
                EventDetected(); // Notify listeners of the event
            }
        }
    }
    #endregion

    #region Collision Methods
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_sensorActive && _timerFinished)
        {
            // Check if the colliding object has a DamageEmitter component
            _damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();

            if (_damageEmitter != null && _damageEmitter.enabled && _damageEmitter.GetEmitting())
            {
                // Handle destruction if the DamageEmitter is configured to destroy after inflicting damage
                if (_damageEmitter.GetDestroyAfterDoingDamage())
                {
                    AnimatorManager animatorController = collision.gameObject.GetComponent<AnimatorManager>();
                    if (animatorController != null)
                    {
                        animatorController.Destroy(); // Trigger destruction animation
                    }
                    else
                    {
                        Destroy(collision.gameObject); // Destroy the object directly
                    }
                }

                _col = true; // Mark that a collision has occurred
                EventDetected(); // Notify listeners of the event
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_sensorActive && _timerFinished)
        {
            // Check if the exiting object has a DamageEmitter component
            _damageEmitter = collision.gameObject.GetComponent<DamageEmitter>();

            if (_damageEmitter != null && _damageEmitter.enabled && _damageEmitter.GetEmitting())
            {
                _col = false; // Mark that the collision has ended
                EventDetected(); // Notify listeners of the event
            }
        }
    }
    #endregion

    // Start the sensor and reset the collision flag
    public override void StartSensor()
    {
        base.StartSensor();
        _col = false; // Reset the collision flag
    }

    private void Start()
    {
        if (_activeFromStart)
        {
            StartSensor(); // Automatically start the sensor if configured
        }
    }

    // Check if a collision has occurred
    public bool HasCollisionOccurred() => _col;

    // Retrieve the associated DamageEmitter
    public DamageEmitter GetDamageEmitter() => _damageEmitter;
}