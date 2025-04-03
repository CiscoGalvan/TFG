using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Directional_Actuator : MovementActuator
{
	[Header("Layers")]
	public LayerMask _layersToCollide;

	[SerializeField,HideInInspector]
	private float _speed = 5f;


	[SerializeField, HideInInspector]
	private float _goalSpeed;


	[SerializeField, HideInInspector]
	private float _interpolationTime = 0;

	
	[SerializeField, HideInInspector]
	private float _angle;

	
	[SerializeField,HideInInspector]
	private bool _throw;

	private float _initialSpeed = 0;
	private float _time;
	private Rigidbody2D _rigidbody;
	private EasingFunction.Function _easingFunc;
	AnimatorManager _animatorManager;

    [SerializeField,HideInInspector]
	private HorizontalActuator.OnCollisionReaction _onCollisionReaction = HorizontalActuator.OnCollisionReaction.None;


	private Vector2 _prevVelocity;

	private bool _alreadyThrown = false;

	[SerializeField, HideInInspector]
	private bool _aimPlayer = false;

	private GameObject _playerReference;
	public override void DestroyActuator()
	{
		
	}

	public override void StartActuator()
	{
		_animatorManager = this.gameObject.GetComponent<AnimatorManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
		_easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
		_time = 0;

		if (_aimPlayer)
		{
			var objectsWithPlayerTagArray = GameObject.FindGameObjectsWithTag("Player");
			if(objectsWithPlayerTagArray.Length == 0)
			{
				Debug.LogWarning("There was no object with Player tag, the proyectile angle won't be controlled");
			}
			else 
			{
				_playerReference = objectsWithPlayerTagArray[0];
				Vector3 direction = _playerReference.transform.position - transform.position;
				_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			}
			
		}


		if (_isAccelerated)
		{
			_speed = _rigidbody.velocity.magnitude;
		}
		_initialSpeed = _speed;


		
		//if (_animatorController != null)
		//{
		//	//_animatorController.ChangeSpeed(_speed);
		//}
	}

	public override void UpdateActuator()
	{
		
		_prevVelocity = _rigidbody.velocity;

		if (!_throw)
			ApplyForce();
		else
		{
			if (!_alreadyThrown)
			{
				ApplyForce();
				_alreadyThrown = true;
			}
		}
	}

	private void ApplyForce()
	{
		_time += Time.deltaTime;

		
		Vector2 direction = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));
	
		if (!_isAccelerated)
		{
			_rigidbody.velocity = direction * _speed;
		}
		else
		{
			float t = (_time / _interpolationTime);
			float easedSpeed = _easingFunc(_initialSpeed, _goalSpeed, t);

			if (t >= 1.0f)
			{
				_speed = _goalSpeed;
				_rigidbody.velocity = direction * _goalSpeed;
			}
			else
			{
				_rigidbody.velocity = direction * easedSpeed;
				_speed = easedSpeed;
			}

			//if (_animatorController != null)
			//{
			//	//_animatorController.ChangeSpeed(_rigidbody.velocity.magnitude);
			//}
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((_layersToCollide.value & (1 << collision.gameObject.layer)) == 0 || _onCollisionReaction == HorizontalActuator.OnCollisionReaction.None) return;

		if (_onCollisionReaction == HorizontalActuator.OnCollisionReaction.Bounce)
		{
			ContactPoint2D contact = collision.contacts[0];
			Vector2 normal = contact.normal;
			// We use the velocity stored before the collision happened
			Vector2 currentVelocity = _prevVelocity;
			if (Vector2.Dot(currentVelocity, normal) >= 0)
			{
				return; // Ignorar colisión no válida
			}
			// We calcule reflection
			float dotProduct = Vector2.Dot(currentVelocity, normal);
			Vector2 reflectedVelocity = currentVelocity - 2 * dotProduct * normal;

			_rigidbody.velocity = reflectedVelocity;
			_speed = reflectedVelocity.magnitude;
			_angle = Mathf.Atan2(reflectedVelocity.y, reflectedVelocity.x) * Mathf.Rad2Deg;
		}
		else if (_onCollisionReaction == HorizontalActuator.OnCollisionReaction.Destroy)
		{
			if (_animatorManager != null)
				_animatorManager.Destroy();
			else
				Destroy(this.gameObject);
		}
	}

	public void SetAngle(float newValue)
	{
		_angle = newValue;
	}

	private void OnDrawGizmosSelected()
	{
		if (!this.isActiveAndEnabled || !_debugActuator) return;
		
		Vector3 origin = transform.position;
		float arrowLength = 2f;

		
		Vector3 direction = new Vector3(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad), 0);

		
		Gizmos.color = Color.red;
		Gizmos.DrawLine(origin, origin + direction * arrowLength);

		
		Vector3 rightTip = Quaternion.Euler(0, 0, 135) * direction;
		Vector3 leftTip = Quaternion.Euler(0, 0, -135) * direction;

		Gizmos.DrawLine(origin + direction * arrowLength, origin + direction * arrowLength + rightTip * 0.5f);
		Gizmos.DrawLine(origin + direction * arrowLength, origin + direction * arrowLength + leftTip * 0.5f);
	}
}
