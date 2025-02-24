using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Player movement speed in units per second")]
    [SerializeField]
    private float _moveSpeed = 5f;
	[Tooltip("Jump force applied when jumping")]
	[SerializeField] private float _jumpForce = 10f;

	private LayerMask playerLayer;
	[Tooltip("Ground check position")]
	[SerializeField] private Transform _groundCheck;
	[SerializeField] private float _groundCheckRadius = 0.2f;

	private Rigidbody2D _rb;
    private Vector2 _movement;
	private bool _isGrounded;
	private void Awake()
    {
        // Get reference to Rigidbody2D
        _rb = GetComponent<Rigidbody2D>();
		playerLayer = LayerMask.GetMask("Player");
    }

    private void Update()
    {
		// Detect user input
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		_movement = new Vector2(horizontalInput, _rb.velocity.y);

		_isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, ~playerLayer);

		// Jumping
		if (_isGrounded && Input.GetButtonDown("Jump"))
		{
			_rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
		}
	}

    private void FixedUpdate()
    {
		// Apply horizontal movement
		_rb.velocity = new Vector2(_movement.x * _moveSpeed, _rb.velocity.y);
	}
	private void OnDrawGizmosSelected()
	{
		if (_groundCheck != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
		}
	}
}
