using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Directional_Actuator : Movement_Actuator
{
	[Header("Configuración de Movimiento Direccional")]
	[Tooltip("Velocidad inicial")]
	[SerializeField]
	private float _speed = 5f;

	[Tooltip("Velocidad meta (cuando se usa aceleración)")]
	[SerializeField, HideInInspector]
	private float _goalSpeed;

	[Tooltip("Tiempo de interpolación para acelerar/desacelerar")]
	[SerializeField, HideInInspector]
	private float _interpolationTime = 0;

	[Tooltip("Ángulo de movimiento (en grados)")]
	[SerializeField]
	private float _angle;

	[Tooltip("Si se activa, la fuerza se aplica solo una vez (tipo 'throw')")]
	[SerializeField]
	private bool _throw;

	private float _initialSpeed = 0;
	private float _time;
	private Rigidbody2D _rigidbody;
	private EasingFunction.Function _easingFunc;
	private AnimatorController _animatorController;

	[SerializeField]
	private Horizontal_Actuator.OnCollisionReaction _onCollisionReaction = Horizontal_Actuator.OnCollisionReaction.None;

	private Collision_Sensor _collisionSensor;

	// Variable para guardar la velocidad previa al choque
	private Vector2 _prevVelocity;

	private bool _alreadyThrown = false;
	public override void DestroyActuator()
	{
		_actuatorActive = false;
		if (_collisionSensor != null)
		{
			_collisionSensor.onEventDetected -= CollisionEvent;
		}
	}

	public override void StartActuator(AnimatorController animatorController)
	{
		_actuatorActive = true;
		_animatorController = animatorController;
		_rigidbody = GetComponent<Rigidbody2D>();
		_easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
		_time = 0;

		if (_onCollisionReaction == Horizontal_Actuator.OnCollisionReaction.Bounce ||
			_onCollisionReaction == Horizontal_Actuator.OnCollisionReaction.Destroy)
		{
			_collisionSensor = this.GameObject().GetComponent<Collision_Sensor>();
			if (_collisionSensor == null)
			{
				_collisionSensor = this.gameObject.AddComponent<Collision_Sensor>();
			}
			_collisionSensor.onEventDetected += CollisionEvent;
			sensors.Add(_collisionSensor);
		}

		if (_isAccelerated)
		{
			_speed = _rigidbody.velocity.magnitude;
		}
		_initialSpeed = _speed;


		//if (_throw)
		//	ApplyForce();

		// Actualización opcional del animator
		if (_animatorController != null)
		{
			//_animatorController.ChangeSpeed(_speed);
		}
	}

	public override void UpdateActuator()
	{
		// Guarda la velocidad actual antes de aplicar la fuerza
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

		// Convertir el ángulo (en grados) a vector unitario
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

			if (_animatorController != null)
			{
				//_animatorController.ChangeSpeed(_rigidbody.velocity.magnitude);
			}
		}
	}

	void CollisionEvent(Sensors s)
	{
		Collision2D col = _collisionSensor.GetCollidedObject();
		if (col == null) return;

		// Comprobar colisión con objetos de World (puedes ajustar según necesites)
		if (col.gameObject.layer != LayerMask.NameToLayer("World"))
			return;

		if (_onCollisionReaction == Horizontal_Actuator.OnCollisionReaction.Bounce)
		{
			ContactPoint2D contact = col.contacts[0];
			Vector2 normal = contact.normal;
			// Usamos la velocidad previa al choque para el cálculo de reflexión
			Vector2 currentVelocity = _prevVelocity;

			// Cálculo manual de la reflexión: V' = V - 2*(V·N)*N
			float dotProduct = Vector2.Dot(currentVelocity, normal);
			Vector2 reflectedVelocity = currentVelocity - 2 * dotProduct * normal;

			// Si el choque es con el techo (normal casi (0, -1)), forzamos una componente Y mínima
			if (Mathf.Abs(normal.x) < 0.2f && normal.y < 0)
			{
				float minDownwardSpeed = _speed * 0.5f;
				if (reflectedVelocity.y > -minDownwardSpeed)
				{
					reflectedVelocity.y = -minDownwardSpeed;
				}
			}

			_rigidbody.velocity = reflectedVelocity;
			_speed = reflectedVelocity.magnitude;
			_angle = Mathf.Atan2(reflectedVelocity.y, reflectedVelocity.x) * Mathf.Rad2Deg;
		}
		else if (_onCollisionReaction == Horizontal_Actuator.OnCollisionReaction.Destroy)
		{
			_animatorController?.Destroy();
		}
	}

	public void SetAngle(float newValue)
	{
		_angle = newValue;
	}
}
