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
    private bool _isSpriteWellOrientedX = true; //esto podria estar en un enun de como esta orientada la animacion y este bool ser privado
    [SerializeField]
    private bool _isSpriteWellOrientedY = true; //esto podria estar en un enun de como esta orientada la animacion y este bool ser privado
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
        if (!_isSpriteWellOrientedX) //si no esta correctamente orientado al inicio rota el obj
        {
            RotatesrpiteX();
        }
        if (!_isSpriteWellOrientedY) //si no esta correctamente orientado al inicio rota el obj
        {
            RotatesrpiteY();
        }
       // SubscribeToActuators();
    }
    private void SubscribeToActuators()
    {
        //foreach (var actuator in _listofActuators)
        //{
        //    switch (actuator)
        //    {
        //        case Horizontal_Actuator horizontalActuator:
        //            if (horizontalActuator.GetBouncing() && _canFlipX)
        //                horizontalActuator.OnBounce += RotatesrpiteX;
        //            else if (horizontalActuator.GetDestroying())
        //                horizontalActuator.OnDestroy += HandleDestroy;
        //            break;

        //        case Vertical_Actuator verticalActuator:
        //            if (verticalActuator.GetBouncing() && _canFlipY)
        //                verticalActuator.OnBounce += RotatesrpiteY;
        //            else if (verticalActuator.GetDestroying())
        //                verticalActuator.OnDestroy += HandleDestroy;
        //            // Añadir el de morir para el destroy
        //            break;
        //    }
        //}
    }
    private void UnsubscribeFromActuators()
    {
        //foreach (var actuator in _listofActuators)
        //{
        //    switch (actuator)
        //    {
        //        case Horizontal_Actuator horizontalActuator:
        //            if (horizontalActuator.GetBouncing() && _canFlipX)
        //                horizontalActuator.OnBounce -= RotatesrpiteX;
        //            else if (horizontalActuator.GetDestroying())
        //                horizontalActuator.OnDestroy -= HandleDestroy;
        //            break;

        //        case Vertical_Actuator verticalActuator:
        //            if (verticalActuator.GetBouncing() && _canFlipY)
        //                verticalActuator.OnBounce -= RotatesrpiteY;
        //            else if (verticalActuator.GetDestroying())
        //                verticalActuator.OnDestroy -= HandleDestroy;
        //            // Añadir el de morir para el destroy
        //            break;
        //    }
        //}
          
    }
    

    //private void Update()
    //{
    //    UpdateAnimationState();
    //}
    private void UpdateAnimationState() //esto se debería de actualizar desde los actuators
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

    public void RotatesrpiteX()
    {
        //_animator.SetTrigger("Bounce");
        if (!_canFlipX) return;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    public void RotatesrpiteY()
    {
        //_animator.SetTrigger("Bounce");
        if (!_canFlipY) return;
        Vector3 localScale = transform.localScale;
        localScale.y *= -1;
        transform.localScale = localScale;
    }
    public void HandleDestroy()
    {
        _animator.SetTrigger("Die");
       
    }
    public void ChangeState()
    {
        _animator.SetTrigger("ChangeState");

    }
    public void SpawnEvent()
    {
        _animator.SetTrigger("SpawnEvent");

    }
}

