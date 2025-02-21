using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Player movement speed in units per second")]
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake()
    {
        // Get reference to Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Detect user input
        float horizontalInput = Input.GetAxisRaw("Horizontal"); //  -1 for left, 1 for right, 0 for no input
        movement = new Vector2(horizontalInput, 0).normalized;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 10000));
        }
    }

    private void FixedUpdate()
    {
        // Apply movement
        rb.velocity = movement * moveSpeed;
    }
}
