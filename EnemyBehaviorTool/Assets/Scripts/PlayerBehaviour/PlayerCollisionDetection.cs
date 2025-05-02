using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credits to Mix and Jam

public class PlayerCollisionDetection : MonoBehaviour
{
    [SerializeField]
    private bool _debugBoxes = false; // Enable/disable debug boxes for visualization

    [Header("Layers")]
    [Tooltip("Layers that will be tracked in case there is an overlap with one of the three boxes.")]
    public LayerMask _detectionLayers; // Layers to detect collisions

    // Booleans to track collision states
    private bool onGround;
    private bool onWall;
    private bool onRightWall;
    private bool onLeftWall;

    [Header("Sizes")]
    [Space]
    public Vector2 bottomSize; // Size of the bottom detection box
    public Vector2 rightSize;  // Size of the right wall detection box
    public Vector2 leftSize;   // Size of the left wall detection box

    [Header("Offsets")]
    [Space]
    public Vector2 bottomOffset; // Offset for the bottom detection box
    public Vector2 rightOffset;  // Offset for the right wall detection box
    public Vector2 leftOffset;   // Offset for the left wall detection box

    // Update is called once per frame
    void Update()
    {
        // Check for collisions with the ground
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, bottomSize, 0f, _detectionLayers);

        // Check for collisions with any wall
        onWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, rightSize, 0f, _detectionLayers)
              || Physics2D.OverlapBox((Vector2)transform.position + leftOffset, leftSize, 0f, _detectionLayers);

        // Check for collisions with the right wall
        onRightWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, rightSize, 0f, _detectionLayers);

        // Check for collisions with the left wall
        onLeftWall = Physics2D.OverlapBox((Vector2)transform.position + leftOffset, leftSize, 0f, _detectionLayers);

    }

    // Draw debug boxes in the scene view
    void OnDrawGizmos()
    {
        if (!_debugBoxes) return; // Exit if debug boxes are disabled

        Gizmos.color = Color.red; // Set the color of the gizmos to red

        // Draw wireframes for the detection boxes
        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, bottomSize);
        Gizmos.DrawWireCube((Vector2)transform.position + rightOffset, rightSize);
        Gizmos.DrawWireCube((Vector2)transform.position + leftOffset, leftSize);
    }

    // Public methods to retrieve collision states
    public bool GetPlayerOnGround() => onGround;
    public bool GetPlayerOnWall() => onWall;
    public bool GetPlayerOnRightWall() => onRightWall;
    public bool GetPlayerOnLeftWall() => onLeftWall;
}