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
    private bool _canFlip = true; //esto deberia mostrarse solo si el movhorizonal permite bounce
    [SerializeField]
    private bool _canRotate = false;


    Rigidbody2D _rigidbody;



    private bool _isMoving = false;
    
    private int _facingDirection = 1; // 1 = derecha, -1 = izquierda

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_canFlip && !_isanimrihgtflip) //si no esta correctamente orientado al inicio rota el obj
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
                horizontalActuator.OnBounce += HandleBounce;
                //añadir el de morir para el destroy
            }
        }
    }
    private void OnDestroy()
    {
        UnsubscribeFromActuators();
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
       if(_animator== null || _rigidbody ==null)
            return;
        _animator.SetFloat("XSpeed", Mathf.Abs(_rigidbody.velocity.x));
        _animator.SetFloat("YSpeed", Mathf.Abs(_rigidbody.velocity.y));

        if (_canFlip)
        {
            
        }
    }

   

   

    private void UnsubscribeFromActuators()
    {
        foreach (var actuator in _listofActuators)
        {
            if (actuator is Horizontal_Actuator horizontalActuator)
            {
                horizontalActuator.OnBounce -= HandleBounce;
                //añadir el de morir para el destroy
            }
        }
    }

    private void HandleBounce()
    {
        //_animator.SetTrigger("Bounce");
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}

