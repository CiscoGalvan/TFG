using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Sensors : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField]
	public State destinationState;

	[SerializeField]
	protected bool m_transition;

    // Evento Action
    private Action<Sensors> onEventDetectedInternal;

    // Contador de suscriptores
    private int subscriberCount = 0;

	// Propiedad pública para obtener el número de suscriptores
	public int SubscriberCount => subscriberCount;

	// Sobrescribir las propiedades add y remove del evento
	public event Action<Sensors> onEventDetected
	{
		add
		{	
			onEventDetectedInternal += value;
			subscriberCount++; // Incrementar el contador de suscriptores
		}
		remove
		{
			onEventDetectedInternal -= value;
			subscriberCount--; // Decrementar el contador de suscriptores
		}
	}

	// Método para disparar el evento
	public void EventDetected()
	{
		onEventDetectedInternal?.Invoke(this);
	}

	public abstract void StartSensor();

	public abstract bool CanTransition();

	public bool WantTransition() { return m_transition; }
}
