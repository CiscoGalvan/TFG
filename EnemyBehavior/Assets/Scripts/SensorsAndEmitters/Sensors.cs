using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public abstract class Sensors : MonoBehaviour
{
	//[SerializeField]
	//public UnityEvent<Sensors> a;

	// Action event
	private Action<Sensors> _onEventDetectedInternal;

    // Subscriber counter
    private int _subscriberCount = 0;

    // Public property to get the number of subscribers
    public int SubscriberCount => _subscriberCount;

    // Override the add and remove properties of the event
    public event Action<Sensors> onEventDetected
	{
		add
		{	
			_onEventDetectedInternal += value;
			_subscriberCount++;
		}
		remove
		{
			_onEventDetectedInternal -= value;
            if (_subscriberCount <= 0)
            {
                Debug.LogError("Attempted to remove a subscriber when there are none.");
                return;
            }
            _subscriberCount--; 
			
		}
	}

    // Method to trigger the event
    public void EventDetected()
	{
		_onEventDetectedInternal?.Invoke(this);
	}

	public abstract void StartSensor();


}
