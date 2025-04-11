using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Ensures that this component has both a Collider2D and a Rigidbody2D
[RequireComponent(typeof(Collider2D))]
public class CollisionSensor : Sensor
{
    [Tooltip("Layers that, in case of collision, will activate the sensor.")]
    [SerializeField]
    private LayerMask _layersToCollide =~0;

	
	// Handles the collision event when the object enters a collision
	private void OnCollisionEnter2D(Collision2D collision)
    {
		// Only process the collision if the sensor is active
		if (!_sensorActive || !_timerFinished) return;
		
		if ((_layersToCollide.value & (1 << collision.gameObject.layer)) != 0)
        {
            EventDetected(); // Call the event handler method
        }
    }

	private void OnCollisionStay2D(Collision2D collision)
	{
		// Only process the collision if the sensor is active
		if (!_sensorActive || !_timerFinished) return;

		if ((_layersToCollide.value & (1 << collision.gameObject.layer)) != 0)
		{
			EventDetected(); // Call the event handler method
		}
	}
}
