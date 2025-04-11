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

	[SerializeField, Min(0)]
	[Tooltip("Initial time the sensor will need to be active")]
	protected float _startDetectingTime = 0f;
	protected Timer _timer;
	protected bool _timerFinished = false;

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

	public virtual void StartSensor()
	{
		_sensorActive = true;
		_timer = new Timer(_startDetectingTime);

		if (_startDetectingTime > 0)
		{
			_timer.Start();
			_timerFinished = false;
		}
		else
		{
			_timerFinished = true;
		}
	}
	public virtual void UpdateSensor()
	{
		if (!_sensorActive) return;
		if (!_timerFinished)
		{
			_timer.Update(Time.deltaTime);
			if (_timer.GetTimeRemaining() <= 0)
			{
				_timerFinished = true;
			}
			else
			{
				return;
			}
		}
	}
	public virtual void StopSensor() 
	{
		_sensorActive = false;
	}

	public void SetDebug(bool debug)
	{
		_debugSensor = debug;
	}

	
	private void Update()
	{
		UpdateSensor();
	}


}
