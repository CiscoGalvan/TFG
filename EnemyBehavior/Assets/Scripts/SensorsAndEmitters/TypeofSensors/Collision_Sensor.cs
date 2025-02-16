using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Ensures that this component has both a Collider2D and a Rigidbody2D
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Collision_Sensor : Sensors
{
    // Boolean to track if a collision has occurred
    bool col;

    // Boolean to track whether the sensor is actively checking for collisions
    bool m_isChecking;

    // Stores the latest collision event
    Collision2D m_collisionobj;

    // Handles the collision event when the object enters a collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only process the collision if the sensor is active
        if (m_isChecking)
        {
            col = true;
            m_collisionobj = collision;
            EventDetected(); // Call the event handler method
        }
    }
    // Activates the sensor. Is the firts method call
    public override void StartSensor()
    {
        m_isChecking = true;
        col = false;
    }

    // Determines if the sensor whants to change the state
    public override bool CanTransition()
    {
        if (col) m_isChecking = false; // Deactivates checking after a collision is detected
        return col;
    }

   

    // Returns the last collided object
    Collision2D GetCollidedObject() { return m_collisionobj; }

    // Returns whether a collision has been detected
    bool GetBooleanCollision() { return col; }
}
