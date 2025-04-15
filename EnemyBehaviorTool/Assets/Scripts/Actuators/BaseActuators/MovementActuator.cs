using UnityEngine;


public abstract class MovementActuator : Actuator
{
    // Indicates whether the movement should be accelerated or not
    [Tooltip("Is the movement accelerated?")]
    [SerializeField]
    protected bool _isAccelerated = false;

    // This field defines the easing function used for acceleration/deceleration
    [SerializeField, HideInInspector]
    protected EasingFunction.Ease _easingFunction;

    // Abstract method that must be implemented to define what happens when the actuator starts
    public abstract override void StartActuator();

    // Abstract method that must be implemented to update the actuator's behavior each frame
    public abstract override void UpdateActuator();

    // Abstract method that must be implemented to define what happens when the actuator is deleated
    public abstract override void DestroyActuator();
}
