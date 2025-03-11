using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    List<Actuator> _listofActuators;
    [SerializeField]
    private bool _isanimrihgtflip = true;
    [SerializeField]
    private bool _canFlipX = true; //esto deberia mostrarse solo si el movhorizonal permite bounce
    [SerializeField]
    private bool _canFlipY = true; //esto deberia mostrarse solo si el movhorizonal permite bounce
    [SerializeField]
    private bool _canRotate = true;


    Rigidbody2D _rigidbody;



    private bool _isMoving = false;
    
    private int _facingDirection = 1; // 1 = derecha, -1 = izquierda

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_canFlipX && !_isanimrihgtflip) //si no esta correctamente orientado al inicio rota el obj
        {
            HandleBounce();
        }
        SubscribeToActuators();
    }
    private void SubscribeToActuators()
    {
        foreach (var actuator in _listofActuators)
        {
            if (actuator is Horizontal_Actuator horizontalActuator)
            {
                if(horizontalActuator.GetBouncing() && _canFlipX)
                 horizontalActuator.OnBounce += HandleBounce;
                else if(horizontalActuator.GetDestroying())
                    horizontalActuator.OnDestroy += HandleDestroy;
                //añadir el de morir para el destroy
            }
        }
    }
    private void UnsubscribeFromActuators()
    {
        foreach (var actuator in _listofActuators)
        {
            switch (actuator)
            {
                case Horizontal_Actuator horizontalActuator:
                    if (horizontalActuator.GetBouncing() && _canFlipX)
                        horizontalActuator.OnBounce += HandleBounce;
                    else if (horizontalActuator.GetDestroying())
                        horizontalActuator.OnDestroy += HandleDestroy;
                    break;

                case Vertical_Actuator verticalActuator:
                    if (verticalActuator.GetBouncing() && _canFlipX)
                        verticalActuator.OnBounce += HandleBounce;
                    else if (verticalActuator.GetDestroying())
                        verticalActuator.OnDestroy += HandleDestroy;
                    // Añadir el de morir para el destroy
                    break;
            }
        }
          
    }
    

    private void Update()
    {
        UpdateAnimationState();
    }
    private void UpdateAnimationState()
    {
        if (_animator == null || _rigidbody == null)
            return;
        float velX = _rigidbody.velocity.x;
        float velY = _rigidbody.velocity.y;
        _animator.SetFloat("XSpeed", Mathf.Abs(velX));
        _animator.SetFloat("YSpeed", Mathf.Abs(velY));
      
       
    }

    private void OnDestroy()
    {
        UnsubscribeFromActuators();
    }

    private void HandleBounce()
    {
        //_animator.SetTrigger("Bounce");
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    private void HandleDestroy()
    {
        _animator.SetTrigger("Die");
       
    }
}

