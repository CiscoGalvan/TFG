using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[ExecuteInEditMode] 

public class MoveToAPoint_Actuator : Actuator
{

	//No tiene sentido que en este movimiento preguntemos si el usuario quiere que sea o no acelerado cuando una de las opciones de las EasingFunctions es Lineal. -> quitar el booleno? 
	//Podriamos poner también que en vez de tomar como referencia el tiempo, poner velocidad minima, máxima y aceleracion como en los otros tipos de movimiento, así el booleano de si es o no acelerado cobraría sentido.

	[Tooltip("Time until the object speed reaches the position it moves to")]
	[SerializeField]
	private float m_timeUntilReachingPosition = 0f;

	[Tooltip("The position the object moves towards")]
	[SerializeField]
	private Transform m_objectivePosition;
	private Rigidbody2D m_rb;
	private bool moving;
	private float elapsedTime;
	private Vector2 startPos; float t;
	public override void StartActuator()
	{
		m_rb = GetComponent<Rigidbody2D>();
		elapsedTime = 0f;
		moving = false;
		if (m_objectivePosition != null)
		{
			startPos = m_rb.position;
			moving = true;
		}
	
	}

	// Update is called once per frame
	public override void UpdateActuator()
	{
		if (!moving || m_objectivePosition == null) return;

		#region Movement by time
		Vector2 targetPos = m_objectivePosition.position;
		elapsedTime += Time.deltaTime;
		t = elapsedTime / m_timeUntilReachingPosition;
		if (m_isAccelerated)
		{
			// Aplicamos una función de easing
			t = EasingFunction.GetEasingFunction(m_easingFunction)(0, 1, t);
		}
		Vector2 newPosition = Vector2.Lerp(startPos, targetPos, t);
		m_rb.MovePosition(newPosition);

		// Si llegamos al destino, detenemos el movimiento
		if (t >= 1f)
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
		if (m_objectivePosition == null)
		{
			Debug.LogError($"There was an error in GameObject{name}, script MoveToAPoint: You have to give 'm_objectivePos' a value");
			UnityEditor.EditorApplication.isPlaying = false;
		}
	}
	public override void DestroyActuator()
	{
		Debug.Log(t);
	}
	#region Setters and Getters
	#endregion

	//Dibuja la misma flecha que otros comportamientos 
	//private void OnDrawGizmosSelected()
	//{
	//	if (!this.isActiveAndEnabled) return;

	//	Gizmos.color = Color.green;
	//	Vector2 position = new Vector2(transform.position.x,transform.position.y);
	//	Vector2 objectivePos = new Vector2(m_objectivePosition.position.x, m_objectivePosition.position.y);

	//	Vector2 direction = (objectivePos - position).normalized;

	//	// Draw direction arrow
	//	Gizmos.DrawLine(position, position + direction);
	//	Vector3 arrowTip = position + direction;
	//	Vector3 right = Quaternion.Euler(0, 0, 135) * direction * 0.25f;
	//	Vector3 left = Quaternion.Euler(0, 0, -135) * direction * 0.25f;
	//	Gizmos.DrawLine(arrowTip, arrowTip + right);
	//	Gizmos.DrawLine(arrowTip, arrowTip + left);
	//}
}
