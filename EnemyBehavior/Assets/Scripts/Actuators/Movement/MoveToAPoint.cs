using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[ExecuteInEditMode] 

public class MoveToAPoint : Actuator
{

	//No tiene sentido que en este movimiento preguntemos si el usuario quiere que sea o no acelerado cuando una de las opciones de las EasingFunctions es Lineal. -> quitar el booleno? 
	//Podriamos poner también que en vez de tomar como referencia el tiempo, poner velocidad minima, máxima y aceleracion como en los otros tipos de movimiento, así el booleano de si es o no acelerado cobraría sentido.

	[Tooltip("Time until the object speed reaches the position it moves to")]
	[SerializeField]
	private float m_timeUntilReachingPosition = 0f;

	[Tooltip("The position the object moves towards")]
	[SerializeField]
	private Transform m_objectivePos;

	[Tooltip("Is the movement accelerated?")]
	[SerializeField]
	bool m_isAccelerated = false;

	private float m_accelerationValue;
	private EasingFunction.Ease m_easingFunction;
	private Rigidbody2D m_rb;
	private bool moving = false;
	private float elapsedTime = 0f;
	private Vector2 startPos;
	public override void StartActuator()
	{
		m_rb = GetComponent<Rigidbody2D>();
		if (m_objectivePos != null)
		{
			startPos = m_rb.position;
			moving = true;
		}
	}

	// Update is called once per frame
	public override void UpdateActuator()
	{
		if (!moving || m_objectivePos == null) return;

		#region Movement by time
		Vector2 targetPos = m_objectivePos.position;
		elapsedTime += Time.deltaTime;
		float t = elapsedTime - m_timeUntilReachingPosition;
		Debug.Log(elapsedTime);
		if (m_isAccelerated)
		{
			// Aplicamos una función de easing
			t = EasingFunction.GetEasingFunction(m_easingFunction)(0, 1, t);
		}
		Vector2 newPosition = Vector2.Lerp(startPos, targetPos, t);
		m_rb.MovePosition(newPosition);

		// Si llegamos al destino, detenemos el movimiento
		if (t >= 0f)
		{
			moving = false;
			m_rb.velocity = Vector2.zero;
		}
		#endregion
		#region Movement by velocities
		#endregion
	}
	private void Start()
	{
		if (m_objectivePos == null)
		{
			Debug.LogError($"There was an error in GameObject{name}, script MoveToAPoint: You have to give 'm_objectivePos' a value");
			UnityEditor.EditorApplication.isPlaying = false;
		}
	}
	public override void Destroy()
	{

	}
	#region Setters and Getters
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

	public void SetEasingFunction(EasingFunction.Ease value)
	{
		m_easingFunction = value;
	}
	public EasingFunction.Ease GetEasingFunctionValue()
	{
		return m_easingFunction;
	}
	#endregion
}
