using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Ensures that this component has both a Collider2D and a Rigidbody2D
[RequireComponent(typeof(Collider2D))]
public class CollisionSensor : Sensors
{
    // Boolean to track if a collision has occurred
    private bool _col;

    // Stores the latest collision event
    private Collision2D _collisionObject;

    // Handles the collision event when the object enters a collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only process the collision if the sensor is active
        if (_sensorActive)
        {
            _col = true;
            _collisionObject = collision;
            EventDetected(); // Call the event handler method
        }
    }
    // Activates the sensor. Is the firts method call
    public override void StartSensor()
    {
        _sensorActive = true;
        _col = false;
    }
	public override void StopSensor()
	{
		_sensorActive = false;
	}


	// Returns the last collided object
	public Collision2D GetCollidedObject() { return _collisionObject; }

    // Returns whether a collision has been detected
    bool GetBooleanCollision() { return _col; }

	
}
