using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HorizontalActuator : MovementActuator
{
    [SerializeField, HideInInspector] private LayerMask _layersToCollide = ~0;

    // Enumerations
    private enum Direction { Left = -1, Right = 1 }
    public enum OnCollisionReaction { None = 0, Bounce = 1, Destroy = 2 }

    [SerializeField, HideInInspector] private float _speed;
    [SerializeField, HideInInspector] private float _goalSpeed;
    [SerializeField, HideInInspector] private float _interpolationTime = 0;
    [SerializeField, HideInInspector] private bool _throw;
    [SerializeField, HideInInspector] private Direction _direction = Direction.Left;
    [SerializeField, HideInInspector] private OnCollisionReaction _onCollisionReaction = OnCollisionReaction.None;
    [SerializeField, HideInInspector] private bool _followPlayer = false;

    private float _initialSpeed = 0;
    private float _time;
    private Rigidbody2D _rigidbody;
    private EasingFunction.Function _easingFunc;
    private AnimatorManager _animatorManager;
    private GameObject _playerReference;

    public override void StartActuator()
    {
        InitializeComponents();
        UpdateAnimation();
        if (_throw) ApplyForce();
    }

    public override void UpdateActuator()
    {
        if (!_throw) ApplyForce();
    }

    public override void DestroyActuator()
    {
        // Handle cleanup logic when the actuator is destroyed
    }

    private void ApplyForce()
    {
        _time += Time.deltaTime;

        UpdateDirectionBasedOnPlayer();

        int dirValue = (int)_direction;
        _rigidbody.velocity = new Vector2(CalculateSpeed() * dirValue, _rigidbody.velocity.y);
        UpdateAnimation();
    }

    private float CalculateSpeed()
    {
        if (!_isAccelerated) return _speed;

        float t = _time / _interpolationTime;
        float easedSpeed = _easingFunc(_initialSpeed, _goalSpeed, t);

        if (t >= 1.0f) _speed = _goalSpeed;
        else _speed = easedSpeed;

        return _speed;
    }

    private void UpdateDirectionBasedOnPlayer()
    {
        if (!_followPlayer) return;
        if (_playerReference == null)
        {
            Debug.LogWarning("Player reference was null, the actuator may not be precise");
            return;
        }
        float playerX = _playerReference.transform.position.x;
        float playerWidth = _playerReference.GetComponent<Collider2D>()?.bounds.extents.x ?? 0;
        float enemyX = transform.position.x;

        if (enemyX > playerX + playerWidth) _direction = Direction.Left;
        else if (enemyX < playerX - playerWidth) _direction = Direction.Right;
    }

    private void InitializeComponents()
    {
        _animatorManager = GetComponent<AnimatorManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
        _time = 0;

        if (_isAccelerated) _speed = _rigidbody.velocity.x;
        _initialSpeed = _speed;

        if (_followPlayer)
        {
            _playerReference = GameObject.FindWithTag("Player");
            if (_playerReference == null) Debug.LogWarning("Player not found, actuator may not work as intended.");
            else
            {
                Vector3 direction = _playerReference.transform.position - transform.position;
                _direction = direction.x > 0 ? Direction.Right : Direction.Left;
            }
        }
    }

    private void UpdateAnimation()
    {
        if (_animatorManager == null || !_animatorManager.enabled) return;

        if (_direction == Direction.Left) _animatorManager.XLeftChangeAndFlip();
        else _animatorManager.XRightChangeAndFlip();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((_layersToCollide.value & (1 << collision.gameObject.layer)) == 0 || _onCollisionReaction == OnCollisionReaction.None) return;

        ContactPoint2D contact = collision.contacts[0];
        Vector2 normal = contact.normal;

        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
        {
            HandleCollisionReaction(normal);
        }
    }

    private void HandleCollisionReaction(Vector2 normal)
    {
        if (_onCollisionReaction == OnCollisionReaction.Bounce)
        {
            if ((_direction == Direction.Left && normal.x > 0) || (_direction == Direction.Right && normal.x < 0))
            {
                _direction = _direction == Direction.Left ? Direction.Right : Direction.Left;
            }
        }
        else if (_onCollisionReaction == OnCollisionReaction.Destroy)
        {
            if (_animatorManager != null && _animatorManager.enabled) _animatorManager.Destroy();
            else Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!isActiveAndEnabled || !_debugActuator) return;

        Gizmos.color = new Color(1f, 0.5f, 0f);
        Vector3 direction = new Vector3((int)_direction, 0, 0);
        Vector3 position = transform.position;
        Gizmos.DrawLine(position, position + direction);

        Vector3 arrowTip = position + direction;
        Vector3 right = Quaternion.Euler(0, 0, 135) * direction * 0.25f;
        Vector3 left = Quaternion.Euler(0, 0, -135) * direction * 0.25f;

        Gizmos.DrawLine(arrowTip, arrowTip + right);
        Gizmos.DrawLine(arrowTip, arrowTip + left);
    }

    #region Setters and Getters 

    public void SetSpeed(float newValue) { _speed = newValue; }
    public float GetSpeed() { return _speed; }

    public void SetGoalSpeed(float newValue) { _goalSpeed = newValue; }
    public float GetGoalSpeed() { return _goalSpeed; }

    public void SetInterpolationTime(float newValue) { _interpolationTime = newValue; }
    public float GetInterpolationTime() { return _interpolationTime; }

    public bool GetBouncing() { return _onCollisionReaction == OnCollisionReaction.Bounce; }
    public bool GetDestroying() { return _onCollisionReaction == OnCollisionReaction.Destroy; }

    #endregion
}