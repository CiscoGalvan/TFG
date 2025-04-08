using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public abstract class Sensor : MonoBehaviour
{
	// Action event
	private Action<Sensor> _onEventDetectedInternal;

	protected bool _debugSensor;
	// Subscriber counter
	private int _subscriberCount = 0;

	protected bool _sensorActive;

    // Override the add and remove properties of the event
    public event Action<Sensor> onEventDetected
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
	public abstract void StopSensor();

	public void SetDebug(bool debug)
	{
		_debugSensor = debug;
	}

}
