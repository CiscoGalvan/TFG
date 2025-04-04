using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HorizontalActuator : MovementActuator
{

	[SerializeField,HideInInspector]
    private LayerMask _layersToCollide = ~0;
    private enum Direction
	{
		Left = -1,
		Right = 1
	}
	public enum OnCollisionReaction
	{
		None = 0,
		Bounce = 1,
		Destroy = 2
	}

    [SerializeField,HideInInspector]
    private float _speed;
	[SerializeField,HideInInspector]
	private float _goalSpeed;

	[SerializeField, HideInInspector]
	private float _interpolationTime = 0;

	
    [SerializeField, HideInInspector]
    private bool _throw; //if this is activated the velocity will be update just ones


    private float _initialSpeed = 0;

    [SerializeField,HideInInspector]
    private Direction _direction = Direction.Left;


	[SerializeField, HideInInspector]
	private OnCollisionReaction _onCollisionReaction = OnCollisionReaction.None;

    [SerializeField, HideInInspector]
    private bool _followPlayer = true;

    private float _time;

    private Rigidbody2D _rigidbody;
    private EasingFunction.Function _easingFunc;
    AnimatorManager _animatorManager;
    private GameObject _playerReference;
    public override void StartActuator()
    {
		
        _animatorManager = this.gameObject.GetComponent<AnimatorManager>();
        _rigidbody = this.GetComponent<Rigidbody2D>();
		_easingFunc = EasingFunction.GetEasingFunction(_easingFunction);

		_time = 0;
		if (_isAccelerated)
		{
			_speed = _rigidbody.velocity.x;
		}
		_initialSpeed = _speed;
        if (_throw) ApplyForce();
		
        if (_followPlayer)
        {
            var objectsWithPlayerTagArray = GameObject.FindGameObjectsWithTag("Player");
            if (objectsWithPlayerTagArray.Length == 0)
            {
                Debug.LogWarning("There was no object with Player tag, the proyectile angle won't be controlled");
            }
            else
            {
                _playerReference = objectsWithPlayerTagArray[0];
                Vector3 direction = _playerReference.transform.position - transform.position;
                if (direction.x > 0)
                {
                    _direction = Direction.Right;

                }
                else
                {
                    _direction = Direction.Left;
                }
            }

        }
		if (_animatorManager != null && _animatorManager.enabled)
		{
			if (_direction == Direction.Left)
				_animatorManager.XLeftChangeAndFlip();
			else
				_animatorManager.XRightChangeAndFlip();
		}
    }
	public override void DestroyActuator()
    {
		
    }
	public override void UpdateActuator()
	{
        if (!_throw) ApplyForce();
	}
	private void ApplyForce()
	{
		_time += Time.deltaTime;
        if (_followPlayer)
        {
            float playerX = _playerReference.transform.position.x;
            float playerWidth = 0f;

            // Intentamos obtener el ancho del jugador (half width)
            Collider2D playerCollider = _playerReference.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                playerWidth = playerCollider.bounds.extents.x;
            }

            float playerLeft = playerX - playerWidth;
            float playerRight = playerX + playerWidth;
			float enemyX = transform.position.x;

			Vector3 direction = _playerReference.transform.position - transform.position;
            if (enemyX > playerRight)
            {
                _direction = Direction.Left;
            }
            else if (enemyX < playerLeft)
            {
                _direction = Direction.Right;
            }
            
        }
        int dirValue = (int)_direction;
		if (!_isAccelerated)
		{
			//MRU
			_rigidbody.velocity = new Vector2(_speed * dirValue, _rigidbody.velocity.y);


		}
		else
		{
			//MRUA
			float t = (_time / _interpolationTime);
			float easedSpeed = _easingFunc(_initialSpeed, _goalSpeed, t);

			if (t >= 1.0f)
			{
				_speed = _goalSpeed;
				_rigidbody.velocity = new Vector2(_goalSpeed * dirValue, _rigidbody.velocity.y);
			}
			else
			{
				_rigidbody.velocity = new Vector2(easedSpeed * dirValue, _rigidbody.velocity.y);
				_speed = easedSpeed;
			}
		}
        if (_animatorManager != null && _animatorManager.enabled)
        {
            if (_direction == Direction.Left)
                _animatorManager.XLeftChangeAndFlip();
            else
                _animatorManager.XRightChangeAndFlip();
        }
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((_layersToCollide.value & (1 << collision.gameObject.layer)) == 0 || _onCollisionReaction ==  OnCollisionReaction.None) return;
		
		ContactPoint2D contact = collision.contacts[0];
		Vector2 normal = contact.normal; 
		if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
		{
			if (_onCollisionReaction == OnCollisionReaction.Bounce)
			{
				bool hitFromCorrectSide = (_direction == Direction.Left && normal.x > 0) || (_direction == Direction.Right && normal.x < 0);
				if (hitFromCorrectSide)
				{
					_direction = _direction == Direction.Left ? Direction.Right : Direction.Left;
				}

			}
			else if (_onCollisionReaction == OnCollisionReaction.Destroy)
			{
				if (_animatorManager != null || !_animatorManager.enabled) _animatorManager.Destroy();
				else Destroy(this.gameObject);

			}
		}
		
	}
	
	private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled || !_debugActuator) return;

        Gizmos.color = new Color(1f, 0.5f, 0f);
        Vector3 position = transform.position;

        Vector3 direction = new Vector3((int)_direction, 0, 0);

        // Draw direction arrow
        Gizmos.DrawLine(position, position + direction );
        Vector3 arrowTip = position + direction;
        Vector3 right = Quaternion.Euler(0, 0, 135) * direction * 0.25f;
        Vector3 left = Quaternion.Euler(0, 0, -135) * direction * 0.25f;
        Gizmos.DrawLine(arrowTip, arrowTip + right);
        Gizmos.DrawLine(arrowTip, arrowTip + left);
       
    }
  

	#region Setters and Getters 
    public void SetSpeed(float newValue)
    {
        _speed = newValue;
    }
    public float GetSpeed()
    {
        return _speed;
    }
	public void SetGoalSpeed(float newValue)
	{
		_goalSpeed = newValue;
	}
	public float GetGoalSpeed()
	{
		return _goalSpeed;
	}
	public void SetInterpolationTime(float newValue)
	{
		_interpolationTime = newValue;
	}
	public float GetInterpolationTime()
	{
		return _interpolationTime;
	}
    public bool GetBouncing()
    {
        return _onCollisionReaction == OnCollisionReaction.Bounce;
    }

    public bool GetDestroying()
    {
        return _onCollisionReaction == OnCollisionReaction.Destroy;
    }
    #endregion
}
