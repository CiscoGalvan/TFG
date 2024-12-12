using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Velocidad de movimiento del jugador en unidades por segundo")]
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake()
    {
        // Obtener referencia al Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Detectar entrada del usuario
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 para izquierda, 1 para derecha, 0 sin entrada
        movement = new Vector2(horizontalInput, 0).normalized;
    }

    private void FixedUpdate()
    {
        // Aplicar movimiento
        rb.velocity = movement * moveSpeed;
    }
}
