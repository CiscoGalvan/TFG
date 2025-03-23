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

		if (_onCollisionReaction == Horizontal_Actuator.OnCollisionReaction.Bounce || _onCollisionReaction == Horizontal_Actuator.OnCollisionReaction.Destroy)
		{
			_collisionSensor = this.GameObject().GetComponent<Collision_Sensor>();
			if (_collisionSensor == null) // Si no existe, lo crea
			{
				_collisionSensor = this.gameObject.AddComponent<Collision_Sensor>();
			}
			_collisionSensor.onEventDetected += CollisionEvent;
			sensors.Add(_collisionSensor);
		}

		if (_isAccelerated)
		{
			// Si se usa aceleración, se toma la velocidad actual (magnitud) del Rigidbody
			_speed = _rigidbody.velocity.magnitude;
		}
		_initialSpeed = _speed;

		// Aplicar fuerza inicial si se configuró en modo 'throw'
		if (_throw)
			ApplyForce();

		// Opcional: actualizar el animator con la dirección o velocidad si se requiere
		if (_animatorController != null)
		{
			//_animatorController.ChangeSpeed(_speed);
		}
	}

	public override void UpdateActuator()
	{
		if (!_throw)
			ApplyForce();
	}

	private void ApplyForce()
	{
		_time += Time.deltaTime;

		// Convertir el ángulo (en grados) a un vector unitario
		Vector2 direction = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));

		if (!_isAccelerated)
		{
			// Movimiento uniforme (MRU)
			_rigidbody.velocity = direction * _speed;
		}
		else
		{
			// Movimiento uniformemente acelerado (MRUA) usando interpolación con easing
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

		// Se comprueba la colisión únicamente con objetos de World o Player
		if (col.gameObject.layer != LayerMask.NameToLayer("World") /*&& col.gameObject.layer != LayerMask.NameToLayer("Player")*/)
			return;

		ContactPoint2D contact = col.contacts[0];
		Vector2 normal = contact.normal;

		if (_onCollisionReaction == Horizontal_Actuator.OnCollisionReaction.Bounce)
		{
			// Rebote en cualquier dirección reflejando la velocidad respecto a la normal
			Vector2 currentVelocity = _rigidbody.velocity;
			Vector2 reflectedVelocity = Vector2.Reflect(currentVelocity, normal);
			_rigidbody.velocity = reflectedVelocity;

			// Actualizar el ángulo a partir del vector reflejado
			_angle = Mathf.Atan2(reflectedVelocity.y, reflectedVelocity.x) * Mathf.Rad2Deg;

			if (_animatorController != null)
			{
				_animatorController.RotatesrpiteX();
			}
		}
		else if (_onCollisionReaction == Horizontal_Actuator.OnCollisionReaction.Destroy)
		{
			if (_animatorController != null)
				_animatorController.Destroy();
		}
	}
	public void SetAngle(float newValue)
	{
		_angle= newValue;
	}
}
