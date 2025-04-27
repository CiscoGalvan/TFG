using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerScript : MonoBehaviour
{
    public Transform target; // Reference to the player
    public float smoothSpeed = 0.125f; // Smoothing speed
    public float xOffset = 2f; // Offset in X direction
    private float lastXPosition; // Last X position of the camera

    void Start()
    {
        // Initialize the last X position with the current position of the camera
        lastXPosition = transform.position.x;
    }

    void LateUpdate()
    {
        // Check if the target is assigned
        if (target == null)
        {
            Debug.LogWarning("No target assigned to the camera");
            return;
        }

        // Determine the new X position only if the player moves forward
        float targetX = target.position.x + xOffset;
        if (targetX > lastXPosition) // Update position only if the player moves forward
        {
            // Calculate the desired position with the offset
            Vector3 desiredPosition = new Vector3(targetX, transform.position.y, transform.position.z);

            // Smoothly interpolate from the current position to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Apply the smoothed position to the camera
            transform.position = smoothedPosition;

            // Update the last X position
            lastXPosition = transform.position.x;
        }
    }
}