using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VerticalActuator : MovementActuator
{
    [SerializeField]
    private LayerMask _layersToCollide; // Defines which layers this object can collide with
    private enum Direction { Down = -1, Up = 1 } // Vertical movement direction
    public enum OnCollisionReaction { None = 0, Bounce = 1, Destroy = 2 } // Collision reactions

    [SerializeField, HideInInspector] private float _speed;
    [SerializeField, HideInInspector] private bool _throw; // If true, movement is applied only once
    [SerializeField, HideInInspector] private float _goalSpeed;
    [SerializeField, HideInInspector] private float _interpolationTime = 0;
    [Tooltip("Movement direction")][SerializeField, HideInInspector] private Direction _direction = Direction.Up;
    [SerializeField, HideInInspector] private OnCollisionReaction _onCollisionReaction = OnCollisionReaction.None;
    [SerializeField, HideInInspector] private bool _followPlayer = false;

    private float _initialSpeed = 0;
    private float _time;
    private Rigidbody2D _rigidbody;
    private EasingFunction.Function _easingFunc;
    AnimatorManager _animatorManager;
    private GameObject _playerReference;

    // Called when the actuator starts
    public override void StartActuator()
    {
        InitializeComponents();
        if (_throw) ApplyForce(); // Apply force once if "throw" is enabled
        UpdateAnimatorState();
    }

    // Called every frame if is in the actual State
    public override void UpdateActuator()
    {
        // Continuously apply force unless it's a "throw"
        if (!_throw) ApplyForce();
    }

    // Called when the actuator is destroyed
    public override void DestroyActuator()
    {
        // No specific cleanup required currently
    }

    private void InitializeComponents()
    {
        _animatorManager = GetComponent<AnimatorManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
        _time = 0;

        _initialSpeed = _isAccelerated ? _rigidbody.velocity.y : _speed;

        if (_followPlayer)
        {
            _playerReference = GameObject.FindWithTag("Player");
            if (_playerReference != null)
            {
                UpdateDirectionTowardsPlayer();
            }
            else
            {
                Debug.LogWarning("Player not found. Movement may not behave as expected.");
            }
        }
    }
    // Updates the movement direction towards the player's position
    private void UpdateDirectionTowardsPlayer()
    {
        Vector3 direction = _playerReference.transform.position - transform.position;
        _direction = direction.y > 0 ? Direction.Up : Direction.Down;
    }

    // Updates the animator's state based on movement direction
    private void UpdateAnimatorState()
    {
        if (!_animatorManager) return;
        if (_direction == Direction.Up)
            _animatorManager.UpDirection();
        else
            _animatorManager.DownDirection();
    }


    // Applies the movement force to the Rigidbody2D
    private void ApplyForce()
    {
        _time += Time.deltaTime;

        if (_followPlayer && _playerReference != null)
        {
            UpdateDirectionTowardsPlayer();
        }
        else if (_followPlayer && _playerReference == null)
        {
            Debug.LogWarning("Player reference was null, the actuator may not be precise.");
        }
        int dirValue = (int)_direction;

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, CalculateSpeed() * dirValue );

    }

    // Calculates eased speed for acceleration
    private float CalculateSpeed()
    {
      
        if (!_isAccelerated) return _speed;

        float t = _time / _interpolationTime;
        float easedSpeed = _easingFunc(_initialSpeed, _goalSpeed, t);

        if (t >= 1.0f) _speed = _goalSpeed;
        else _speed = easedSpeed;

        return _speed;
    }
    // Checks if the collision is valid based on layers and reaction type
    private bool IsCollisionValid(Collision2D collision)
    {
        return (_layersToCollide.value & (1 << collision.gameObject.layer)) != 0
            && _onCollisionReaction != OnCollisionReaction.None;
    }

    // Handles collisions with defined layers and reacts accordingly
    private void OnCollisionStay2D(Collision2D collision) 
    {
        if (!IsCollisionValid(collision)) return;

        ContactPoint2D contact = collision.contacts[0];
        Vector2 normal = contact.normal;

        if (Mathf.Abs(normal.x) < Mathf.Abs(normal.y)) // Check for vertical collision
        {
            HandleCollisionReaction(normal);
        }
    }
    // Handles the collision reaction
    private void HandleCollisionReaction(Vector2 normal)
    {
        if (_onCollisionReaction == OnCollisionReaction.Bounce)
        {
            if (IsBounceValid(normal))
            {
                ReverseDirection();
                UpdateAnimatorState();
            }
        }
        else if (_onCollisionReaction == OnCollisionReaction.Destroy)
        {
            DestroyActuatorOrObject();
        }
    }
    // Checks if the bounce reaction is valid based on collision normal
    private bool IsBounceValid(Vector2 normal)
    {
        return (_direction == Direction.Up && normal.y < 0) || (_direction == Direction.Down && normal.y > 0);
    }

    // Reverses the movement direction
    private void ReverseDirection()
    {
        _direction = _direction == Direction.Up ? Direction.Down : Direction.Up;
        _animatorManager?.RotateSpriteY();
    }
    // Destroys the actuator or the GameObject
    private void DestroyActuatorOrObject()
    {
        if (_animatorManager != null && _animatorManager.enabled)
            _animatorManager.Destroy();
        else
            Destroy(gameObject);
    }
  

    // Draws a direction arrow in the scene view
    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled || !_debugActuator) return;

        Gizmos.color = new Color(1f, 0.5f, 0f);
        Vector3 position = transform.position;
        Vector3 dir = new Vector3(0, (int)_direction, 0);

        // Draw main arrow line
        Gizmos.DrawLine(position, position + dir);

        // Draw arrowhead
        Vector3 arrowTip = position + dir;
        Vector3 right = Quaternion.Euler(0, 0, 135) * dir * 0.25f;
        Vector3 left = Quaternion.Euler(0, 0, -135) * dir * 0.25f;
        Gizmos.DrawLine(arrowTip, arrowTip + right);
        Gizmos.DrawLine(arrowTip, arrowTip + left);
    }
}

