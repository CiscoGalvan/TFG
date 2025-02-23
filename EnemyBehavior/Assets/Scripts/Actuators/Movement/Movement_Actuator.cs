using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement_Actuator : Actuator
{
	[Tooltip("Is the movement accelerated?")]
	[SerializeField]
	protected bool m_isAccelerated = false;
	[SerializeField]
	[HideInInspector]
	protected EasingFunction.Ease m_easingFunction;
	protected float m_accelerationValue;

	public abstract override void DestroyActuator();

	public abstract override void StartActuator();

	public abstract override void UpdateActuator();

	#region Setters and Getters
	public void SetEasingFunction(EasingFunction.Ease value)
	{
		m_easingFunction = value;
	}
	public EasingFunction.Ease GetEasingFunctionValue()
	{
		return m_easingFunction;
	}
	public bool IsMovementAccelerated()
	{
		return m_isAccelerated;
	}
	public void SetAccelerationValue(float value)
	{
		m_accelerationValue = value;
	}
	public float GetAccelerationValue()
	{
		return m_accelerationValue;
	}
	#endregion
}
