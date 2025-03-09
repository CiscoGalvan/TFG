using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Ensures that this component has both a Collider2D and a Rigidbody2D
[RequireComponent(typeof(Collider2D))]
public class Collision_Sensor : Sensors
{
	//Si queremos hacer combinaciones de eventos debemos usar [Flags]
	public new enum SensorEventTypes
	{
		OnCollisionEnterEvent,
        NoCollisionEvent,
        OnPlayerCollision
	}
    [SerializeField, HideInInspector]
    private SensorEventTypes _subscriberType;


	public Action<Sensors> _onCollisionEnterEvent;
	public Action<Sensors> _onNoCollisionEvent;
	public Action<Sensors> _onPlayerCollision;
    // Boolean to track if a collision has occurred
    private bool _col;

    // Boolean to track whether the sensor is actively checking for collisions
    private bool _isChecking;

    // Stores the latest collision event
    private Collision2D _collisionObject;


    float _dt;
    private bool _aaa = false;
	// Handles the collision event when the object enters a collision
	private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only process the collision if the sensor is active
        if (_isChecking)
        {
            _col = true;
            _collisionObject = collision;
            EventDetected(_onCollisionEnterEvent); // Call the event handler method
            if(collision.gameObject.name == "Player")
            {
                Debug.Log("PLAYER");   
                EventDetected(_onPlayerCollision);
			}
        }
	}

	private void Update()
	{
        _dt += Time.deltaTime;
        if (!_aaa && _dt > 5)
        {
            _aaa = true;
            EventDetected(_onNoCollisionEvent);
        }
	}
	// Activates the sensor. Is the firts method call
	public override void StartSensor()
    {
        _isChecking = true;
        _col = false;
		if (_onCollisionEnterEvent == null)
		{
			_onCollisionEnterEvent = delegate { };
			
		}

		if (_onNoCollisionEvent == null)
		{
			_onNoCollisionEvent = delegate { };
		
		}
	}


    // Returns the last collided object
    public Collision2D GetCollidedObject() { return _collisionObject; }

    // Returns whether a collision has been detected
    bool GetBooleanCollision() { return _col; }

    public SensorEventTypes GetSuscriberType() => _subscriberType;
}
