using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Directional_Actuator : MovementActuator
{

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

	private CollisionSensor _collisionSensor;

	private Vector2 _prevVelocity;

	private bool _alreadyThrown = false;

	[SerializeField, HideInInspector]
	private bool _aimPlayer = false;

	private GameObject _playerReference;
	public override void DestroyActuator()
	{
		_actuatorActive = false;
		if (_collisionSensor != null)
		{
			_collisionSensor.onEventDetected -= CollisionEvent;
		}
	}

	public override void StartActuator()
	{
		_actuatorActive = true;
        _animatorManager = this.gameObject.GetComponent<AnimatorManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
		_easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
		_time = 0;

		if (_onCollisionReaction == HorizontalActuator.OnCollisionReaction.Bounce ||
			_onCollisionReaction == HorizontalActuator.OnCollisionReaction.Destroy)
		{
			_collisionSensor = this.gameObject.GetComponent<CollisionSensor>();
			if (_collisionSensor == null)
			{
				_collisionSensor = this.gameObject.AddComponent<CollisionSensor>();
			}
			_collisionSensor.onEventDetected += CollisionEvent;
			sensors.Add(_collisionSensor);
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

	void CollisionEvent(Sensors s)
	{
		Collision2D col = _collisionSensor.GetCollidedObject();
		if (col == null) return;

		
		if (col.gameObject.layer != LayerMask.NameToLayer("World"))
			return;

		if (_onCollisionReaction == HorizontalActuator.OnCollisionReaction.Bounce)
		{

            
            ContactPoint2D contact = col.contacts[0];
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
