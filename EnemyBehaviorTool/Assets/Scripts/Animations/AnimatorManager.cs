using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Ensures that this component requires an Animator component
[RequireComponent(typeof(Animator))]
public class AnimatorManager : MonoBehaviour
{
    private Animator _animator; // Reference to the Animator component

    [Tooltip("If true, allows the sprite to be flipped horizontally")]
    [SerializeField]
    private bool _canFlipX = true; // Determines if the sprite can flip horizontally

    [Tooltip("If true, allows the sprite to be flipped vertically")]
    [SerializeField]
    private bool _canFlipY = true; // Determines if the sprite can flip vertically
    private SpriteRenderer _spriteRenderer; // Reference to the SpriteRenderer component
    private Rigidbody2D _rb; // Reference to the Rigidbody2D component
    private bool die = false;
    private void Start()
    {
        // Initialize references to required components
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        // Log an error if the Animator component is not attached
        if (_animator == null)
        {
            Debug.LogError("NO ANIMATOR IS ATTACHED");
            return;
        }
    }

    private void Update()
    {
        // Update Animator parameters with the Rigidbody2D's velocity
        if (_rb != null)
        {
            Vector2 velocity = _rb.velocity;
            _animator.SetFloat("XSpeed", velocity.x); // Set X-axis speed
            _animator.SetFloat("YSpeed", velocity.y); // Set Y-axis speed
        }
    }

    // Flips the sprite horizontally to face left
    private void RotateSpriteXLeft()
    {
        if (!_canFlipX) return; // Exit if horizontal flipping is disabled
        if (_spriteRenderer != null)
        {
            _spriteRenderer.flipX = true;
        }
    }

    // Flips the sprite horizontally to face right
    private void RotateSpriteXRight()
    {
        if (!_canFlipX) return; // Exit if horizontal flipping is disabled
        if (_spriteRenderer != null)
        {
            _spriteRenderer.flipX = false;
        }
    }

    // Toggles the sprite's vertical flipping
    public void RotateSpriteY()
    {
        if (!_canFlipY) return; // Exit if vertical flipping is disabled
        if (_spriteRenderer != null)
        {
            _spriteRenderer.flipY = !_spriteRenderer.flipY; // Toggle the flipY property
        }
    }

    // Handles the destruction process of the object
    public void Destroy()
    {
        if (_animator == null || !_animator.enabled) return;
        if(die)return;
        // Trigger the "Die" animation
        _animator.SetTrigger("Die");
        // Stop Rigidbody2D movement
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        die = true;
        // Start a coroutine to destroy the object after the animation
        StartCoroutine(DestroyAfterAnimation());

        
    }

    // Coroutine to destroy the object after the "Die" animation finishes
    private IEnumerator DestroyAfterAnimation()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // Esperar hasta que el estado actual sea "Die"
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            yield return null;

        float duration = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration- 0.1f);
        Destroy(gameObject);
    }

    // Triggers the "Damage" animation
    public void Damage()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetTrigger("Damage");
    }

    // Changes the state in the animator and resets all directional booleans
    public void ChangeState()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetTrigger("ChangeState");
        _animator.SetBool("Left", false);
        _animator.SetBool("Right", false);
        _animator.SetBool("Up", false);
        _animator.SetBool("Down", false);
        _animator.SetBool("Follow", false);
    }

    // Triggers the "Spawn" animation
    public void SpawnEvent()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetTrigger("Spawn");
    }

    // Sets the animator's direction to left
    private void LeftDirection()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetBool("Left", true);
        _animator.SetBool("Right", false);
    }

    // Sets the animator's direction to right
    private void RightDirection()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetBool("Left", false);
        _animator.SetBool("Right", true);
    }

    // Sets the animator's direction to down
    public void DownDirection()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetBool("Up", false);
        _animator.SetBool("Down", true);
    }

    // Sets the animator's direction to up
    public void UpDirection()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetBool("Down", false);
        _animator.SetBool("Up", true);
    }

    // Changes the animator state and flips the sprite to the left
    public void XLeftChangeAndFlip()
    {
        if (_animator == null || !_animator.enabled) return;

        RotateSpriteXLeft();
        LeftDirection();
    }

    // Changes the animator state and flips the sprite to the right
    public void XRightChangeAndFlip()
    {
        if (_animator == null || !_animator.enabled) return;

        RotateSpriteXRight();
        RightDirection();
    }

    // Updates the animator's rotation speed
    public void ChangeSpeedRotation(float speed)
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetFloat("RotationSpeed", speed);
    }

    // Enables the "Follow" behavior in the animator
    public void Follow()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetBool("Follow", true);
    }
}