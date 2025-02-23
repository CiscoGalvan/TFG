using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Player movement speed in units per second")]
    [SerializeField]
    private float moveSpeed = 5f;
	[Tooltip("Jump force applied when jumping")]
	[SerializeField] private float jumpForce = 10f;

	private LayerMask playerLayer;
	[Tooltip("Ground check position")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckRadius = 0.2f;

	private Rigidbody2D rb;
    private Vector2 movement;
	private bool isGrounded;
	private void Awake()
    {
        // Get reference to Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
		playerLayer = LayerMask.GetMask("Player");
    }

    private void Update()
    {
		// Detect user input
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		movement = new Vector2(horizontalInput, rb.velocity.y);

		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ~playerLayer);

		// Jumping
		if (isGrounded && Input.GetButtonDown("Jump"))
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
		}
	}

    private void FixedUpdate()
    {
		// Apply horizontal movement
		rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
	}
	private void OnDrawGizmosSelected()
	{
		if (groundCheck != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
		}
	}
}
