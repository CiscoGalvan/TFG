using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Sensors : MonoBehaviour
{
    // Evento Action
    private Action<Sensors> _onEventDetectedInternal;

    // Contador de suscriptores
    private int _subscriberCount = 0;

	// Propiedad pública para obtener el número de suscriptores
	public int SubscriberCount => _subscriberCount;

	// Sobrescribir las propiedades add y remove del evento
	public event Action<Sensors> onEventDetected
	{
		add
		{	
			_onEventDetectedInternal += value;
			_subscriberCount++; // Incrementar el contador de suscriptores
		}
		remove
		{
			_onEventDetectedInternal -= value;
			_subscriberCount--; // Decrementar el contador de suscriptores
		}
	}

	// Método para disparar el evento
	public void EventDetected()
	{
		_onEventDetectedInternal?.Invoke(this);
	}

	public abstract void StartSensor();

	public abstract bool CanTransition();

}
