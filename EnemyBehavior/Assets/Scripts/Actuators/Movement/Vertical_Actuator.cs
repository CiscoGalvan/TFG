using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Rigidbody2D))]

public class Vertical_Actuator : Movement_Actuator
{
    [SerializeField, HideInInspector]
    private bool bounceAfterCollision = false;
    [SerializeField, HideInInspector]
    private bool destroyAfterCollision = false;

    [SerializeField]
	[HideInInspector]
	private float speed;


	[SerializeField]
	[HideInInspector]
	private float goalSpeed;


	[SerializeField]
	[HideInInspector]
	private float interpolationTime = 0;
    Collision_Sensor collisionSensor;

    private float initial_speed = 0;
    //[Tooltip("Object acceleration in units per second squared. Set to 0 for uniform motion")]

    //private float m_acceleration = 0f;

    [Tooltip("Movement direction")]
    [SerializeField]
    private Direction direction = Direction.Up;

    private enum Direction
    {
        Up = -1,
        Down = 1
    }

    private float time;
    Rigidbody2D rigidbody;

    private EasingFunction.Function easingFunc;
    public override void StartActuator()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        easingFunc = EasingFunction.GetEasingFunction(m_easingFunction);
        collisionSensor = this.GameObject().GetComponent<Collision_Sensor>();
        if (bounceAfterCollision || destroyAfterCollision)
        {
            collisionSensor = this.GameObject().GetComponent<Collision_Sensor>();
            if (collisionSensor == null) //si no esta creado lo crea
            {
                collisionSensor = this.gameObject.AddComponent<Collision_Sensor>();
            }
            collisionSensor.onEventDetected += CollisionEvent;
            sensors.Add(collisionSensor);
        }
        
        time = 0;
        if (m_isAccelerated)
        {
            speed = rigidbody.velocity.x;
        }
        initial_speed = speed;

    }
    public override void DestroyActuator()
    {
        if (collisionSensor != null)
        {
            collisionSensor.onEventDetected += CollisionEvent;
        }
    }

    public override void UpdateActuator()
    {
		time += Time.deltaTime;
		int dirValue = (int)direction;
		if (!m_isAccelerated)
		{
			//MRU
			rigidbody.velocity = new Vector2(rigidbody.velocity.x,speed * dirValue);
		}
		else
		{
			//MRUA
			float t = (time / interpolationTime);
			float easedSpeed = easingFunc(initial_speed, goalSpeed, t);
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, easedSpeed * dirValue);

			if (t >= 1.0f)
			{
				speed = goalSpeed;
			    rigidbody.velocity = new Vector2( rigidbody.velocity.x, goalSpeed * dirValue);
			}
			else
			{
				speed = easedSpeed;
			}
		}
	}
    void CollisionEvent(Sensors s)
    {

        Collision2D col = collisionSensor.GetCollidedObject();

        if (col == null) return;
        //comprobacion  de:
        // choque enemigo con mundo 
        //choque por izquierda o derecha
        if (col.gameObject.layer != LayerMask.NameToLayer("World")) return;
        ContactPoint2D contact = col.contacts[0];
        Vector2 normal = contact.normal;

        if (Mathf.Abs(normal.x) < Mathf.Abs(normal.y))
        {
           
            if (bounceAfterCollision)
            {
                direction = direction == Direction.Up ? Direction.Down : Direction.Up;
            }
            else if (destroyAfterCollision)
            {
                Destroy(this.gameObject);
            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled) return;

        Gizmos.color = Color.green;
        Vector3 position = transform.position;

        Vector3 dir = new Vector3(0, (int)direction, 0);

        // Draw direction arrow
        Gizmos.DrawLine(position, position + dir);
        Vector3 arrowTip = position + dir;
        Vector3 right = Quaternion.Euler(0, 0, 135) * dir * 0.25f;
        Vector3 left = Quaternion.Euler(0, 0, -135) * dir * 0.25f;
        Gizmos.DrawLine(arrowTip, arrowTip + right);
        Gizmos.DrawLine(arrowTip, arrowTip + left);
    }

	public void SetSpeed(float newValue)
	{
		speed = newValue;
	}
	public float GetSpeed()
	{
		return speed;
	}
	public void SetGoalSpeed(float newValue)
	{
		goalSpeed = newValue;
	}
	public float GetGoalSpeed()
	{
		return goalSpeed;
	}
	public void SetInterpolationTime(float newValue)
	{
		interpolationTime = newValue;
	}
    public void SetDirectionUP()
    {
		direction = Direction.Up;
    }
    public void SetDirectionDown()
    {
        direction = Direction.Down;
    }
    public float GetInterpolationTime()
    {
        return interpolationTime;
    }
    public void SetBouncesAfterCollision(bool newValue)
    {
        bounceAfterCollision = newValue;
    }
    public bool GetBouncesAfterCollision() => bounceAfterCollision;
    public void SetDestroyAfterCollision(bool newValue)
    {
        destroyAfterCollision = newValue;
    }
    public bool GetDestroyAfterCollision() => destroyAfterCollision;
}
