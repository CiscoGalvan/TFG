using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
// Observation: The distance between the two objects is measured from their center.
public class DistanceSensor : Sensors
{
    public enum TypeOfDistance
    {
        Area = 0,
        Magnitude = 1,
        SingleAxis = 2
    };
    public enum Axis
    {
        Y = 0,
        X = 1
    };
    public enum PartOfAxis
    {
        UpOrLeft = 0,
        DownOrRight = 1
    };
    public enum DetectionSide
    {
        Both = 0,
        Single = 1
    };

    [SerializeField] private TypeOfDistance _distanceType = TypeOfDistance.Magnitude;
    [SerializeField] private Axis _axis = Axis.X;
    [SerializeField] private PartOfAxis _partOfAxis = PartOfAxis.UpOrLeft;
    [SerializeField] private DetectionSide _detectionSide = DetectionSide.Both;

    /// <summary>
    /// variables para magnitud y eje
    /// </summary>
    [SerializeField]
    private GameObject _target; // Target to check distance against
    [SerializeField, Min(0)] private float _detectionDistance = 5f; // Threshold distance


    [SerializeField, Tooltip("External trigger used for area detection.")]
    private Collider2D _areaTrigger; // Now we use an external trigger instead of the sensor itself


    // Indicates whether the timer is active
    private bool _startDistance = false;


    //private bool _isNear;


    [SerializeField]
    private float _startDetectingTime = 0f;

    private float _t;

    // Initializes the sensor settings
    public override void StartSensor()
    {
        _sensorActive = true;

		if (_startDetectingTime == 0)
        {
			_startDistance = true;
		}
        else
        {
			_startDistance = false;
		}
		_t = 0;

		if (_distanceType == TypeOfDistance.Area && (_areaTrigger == null || !_areaTrigger.isTrigger))
        {
           
             Debug.LogError("Area detection requires a Collider2D set as a trigger!");
        
        }
    }
	public override void StopSensor()
	{
        _sensorActive = false;
	}

	// Determines if the sensor should transition based on distance
	private void Update()
    {
        if (!_sensorActive) return;
        if (!_startDistance)
        {
            _t += Time.deltaTime;
            if(_t > _startDetectingTime)
            {
                _t = 0;
                _startDistance = true;

			}
        }
        if (!_startDistance || _target == null || _distanceType == TypeOfDistance.Area)
            return;
        
        Vector2 selfPos = transform.position;
        Vector2 targetPos = _target.transform.position;
        bool detected = false;

        switch (_distanceType)
        {
            case TypeOfDistance.Magnitude:
                detected = Vector2.Distance(selfPos, targetPos) <= _detectionDistance;
                break;

            case TypeOfDistance.SingleAxis:
                float distance = (_axis == Axis.X)
                    ? Mathf.Abs(selfPos.x - targetPos.x)
                    : Mathf.Abs(selfPos.y - targetPos.y);
                bool correctSide = _detectionSide == DetectionSide.Both ||
                   (_axis == Axis.X
                       ? (_partOfAxis == PartOfAxis.UpOrLeft ? targetPos.x < selfPos.x : targetPos.x > selfPos.x)
                       : (_partOfAxis == PartOfAxis.UpOrLeft ? targetPos.y > selfPos.y : targetPos.y < selfPos.y));

                detected = distance <= _detectionDistance && correctSide;
                break;
        }

        if (detected)
        {
            EventDetected();
		}
        
       
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_startDistance &&_distanceType == TypeOfDistance.Area && other.gameObject == _target )
        {
            EventDetected();
		}
    }
    // Draws the detection range in the scene view
    private void OnDrawGizmos()
    {
       
        if (!_debugSensor) return;
        Gizmos.color = new Color(0, 0, 1, 0.3f);
        switch (_distanceType)
        {
            case TypeOfDistance.Magnitude:
				Handles.color = new Color(0, 0, 1, 0.3f); 
				Handles.SphereHandleCap(0, transform.position, Quaternion.identity, _detectionDistance * 2, EventType.Repaint);
				break;

            case TypeOfDistance.SingleAxis:
                Vector3 size;
                Vector3 positionOffset = Vector3.zero;

                if (_axis == Axis.X)
                {
                    if (_detectionSide == DetectionSide.Both)
                    {
                        size = new Vector3(_detectionDistance * 2, 10, 1);
                    }
                    else
                    {
                        size = new Vector3(_detectionDistance, 10, 1);
                        positionOffset = new Vector3(
                            _partOfAxis == PartOfAxis.UpOrLeft ? -_detectionDistance / 2 : _detectionDistance / 2,
                            0, 0);
                    }
                }
                else // Axis.Y
                {
                    if (_detectionSide == DetectionSide.Both)
                    {
                        size = new Vector3(10, _detectionDistance * 2, 1);
                    }
                    else
                    {
                        size = new Vector3(10, _detectionDistance, 1);
                        positionOffset = new Vector3(
                            0, _partOfAxis == PartOfAxis.UpOrLeft ? _detectionDistance / 2 : -_detectionDistance / 2,
                            0);
                    }
                }

                Gizmos.DrawCube(transform.position + positionOffset, size);
                break;
        }

    }

    public void SetDetectionDistance(float newValue)
    {
        _detectionDistance = newValue;
    }
    public void SetTarget(GameObject g)
    {
        _target = g;
    }


}
