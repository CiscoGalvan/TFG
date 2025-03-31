using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CircularActuator : MovementActuator
{
	// Velocidad angular en grados por segundo (se convertirá a radianes)
	[SerializeField, HideInInspector]
	private float _angularSpeed ;

	[Tooltip("Center of rotation (if not specified, uses the object's initial position)")]
	[SerializeField]
	private Transform _rotationPointPosition;

	[Tooltip("Maximum allowed angle in degrees. Use 360 for a full circle, less for pendulum-like motion")]
	[SerializeField, Range(0f, 360f)]
	private float _maxAngle = 360f;

    [SerializeField, HideInInspector]
    private float _angularAcceleration = 0f;

    [SerializeField]
    private bool _canRotate = false;

    private Rigidbody2D _rigidbody;
	
	private float _currentAngularSpeed;
	
	private float _currentAngle;

	private bool _reversing = false;
	private float _radius = 0;
	
	private float _initAngle;
	private float _initAngularSpeed;
	private float _goalAngularSpeed;

	private float _time;
	[SerializeField, HideInInspector]
	private float _interpolationTime;

	private Vector3 _startingPosition;
	private EasingFunction.Function _easingFunc;
    AnimatorManager _animatorManager;

    public override void StartActuator()
	{
         _animatorManager = this.gameObject.GetComponent<AnimatorManager>();
        _actuatorActive = true;
		_startingPosition = transform.position;
		_rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;

		if (_rotationPointPosition == null)
		{
			Debug.Log("No rotation point assigned. Using object's initial position.");
			_rotationPointPosition = new GameObject(gameObject.name + " RotationPoint").transform;
			_rotationPointPosition.position = transform.position;
		}

		_radius = Vector3.Distance(_rotationPointPosition.position, transform.position);
		Vector2 dir = transform.position - _rotationPointPosition.position;
		_initAngle = Mathf.Atan2(dir.y, dir.x) ;
		_currentAngle = _initAngle;
		_currentAngularSpeed = _angularSpeed * Mathf.Deg2Rad;
		_initAngularSpeed = _currentAngularSpeed;
		_time = 0;
		
		_easingFunc = EasingFunction.GetEasingFunction(_easingFunction);
	}

	public override void UpdateActuator()
	{
		_time += Time.deltaTime;

		
		if (_isAccelerated)
		{
			float t = Mathf.Clamp01(_time / _interpolationTime);
			_currentAngularSpeed = _easingFunc(_initAngularSpeed, _goalAngularSpeed, t);
			if (t >= 1.0f)
			{
				_currentAngularSpeed = _goalAngularSpeed;
			}
		}

		
		float maxAngleRad = _maxAngle * Mathf.Deg2Rad;

		if (_maxAngle < 360f) // Modo pendular
		{
			
			float upperLimit = _initAngle + (maxAngleRad / 2f);
			float lowerLimit = _initAngle - (maxAngleRad / 2f);

			if (!_reversing)
			{
				_currentAngle += _currentAngularSpeed * Time.deltaTime;
				if (_currentAngle > upperLimit)
				{
					_currentAngle = upperLimit;
					_reversing = true;
				}
			}
			else
			{
				_currentAngle -= _currentAngularSpeed * Time.deltaTime;
				if (_currentAngle < lowerLimit)
				{
					_currentAngle = lowerLimit;
					_reversing = false;
				}
			}
		}
		else // Movimiento circular completo
		{
			_currentAngle += _currentAngularSpeed * Time.deltaTime;
			
			_currentAngle = Mathf.Repeat(_currentAngle, 2 * Mathf.PI);
		}

	
		float tangentialSpeed = _currentAngularSpeed * _radius;
		
		if (_maxAngle < 360f && _reversing)
		{
			tangentialSpeed *= -1;
		}

		
		Vector2 tangentialVelocity = new Vector2(
			-Mathf.Sin(_currentAngle) * tangentialSpeed,
			Mathf.Cos(_currentAngle) * tangentialSpeed
		);

		_rigidbody.velocity = tangentialVelocity;
		// Correccion de posicion para mantener el radio constante
		
            Vector3 expectedPosition = _rotationPointPosition.position + new Vector3( Mathf.Cos(_currentAngle) * _radius,Mathf.Sin(_currentAngle) * _radius,0f);
            _rigidbody.MovePosition(expectedPosition);
		// Rotar el objeto si _canRotate es true
		if (_canRotate) transform.rotation = Quaternion.Euler(0f, 0f, _currentAngle * Mathf.Rad2Deg);
       
        if (_animatorManager != null) _animatorManager.ChangeSpeedRotation(_currentAngle * Mathf.Rad2Deg);

    }

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		if (!_debugActuator) return;
		if (this.isActiveAndEnabled)
		{
			if (_maxAngle == 360f)
			{
				if (_rotationPointPosition == null)
				{
					Gizmos.DrawWireSphere(transform.position, _radius);
				}
				else
				{
					_radius = Vector3.Distance(_rotationPointPosition.position, transform.position);
					Gizmos.DrawWireSphere(_rotationPointPosition.position, _radius);
				}
			}
			else
			{
				Vector3 direction = _startingPosition - _rotationPointPosition.position;
				float initialAngleDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				float halfAngleRange = _maxAngle / 2f;

				Handles.color = Color.red;
				Handles.DrawWireArc(_rotationPointPosition.position, Vector3.forward,
					Quaternion.Euler(0, 0, initialAngleDegrees + halfAngleRange) * Vector3.right,
					-halfAngleRange, _radius);

				Handles.color = Color.blue;
				Handles.DrawWireArc(_rotationPointPosition.position, Vector3.forward,
					Quaternion.Euler(0, 0, initialAngleDegrees) * Vector3.right,
					-halfAngleRange, _radius);
			}
		}
	}
#endif

	public Transform GetRotationPoint() { return _rotationPointPosition; }
	public float GetRadius() { return _radius; }
	public void SetRadius(float newValue) { _radius = newValue; }
	public bool RotationPointAssigned() { return _rotationPointPosition != null; }

	public override void DestroyActuator()
	{
		_actuatorActive = false;
	}

	#region Setters and getters
	public void SetAngularSpeed(float newValue)
	{
		_angularSpeed = newValue;
		_currentAngularSpeed = _angularSpeed * Mathf.Deg2Rad;
	}
	public float GetAngularSpeed() { return _angularSpeed; }
	public void SetGoalAngularSpeed(float newValue) { _goalAngularSpeed = newValue; }
	public float GetGoalAngularSpeed() { return _goalAngularSpeed; }
	public void SetInterpolationTime(float newValue) { _interpolationTime = newValue; }
	public float GetInterpolationTime() { return _interpolationTime; }
	#endregion
}
