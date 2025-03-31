using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator _animator;
    //[SerializeField]
   // List<Actuator> _listofActuators;
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
    SpriteRenderer _spriteRenderer;

    private void Start()
    {

        Debug.Log(_animator);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        Debug.Log(_animator + "despues");
        if (_animator == null)
        {
            Debug.LogError("NO ANIMATOR IS ATTACHED");
        }
       
        if (!_isSpriteWellOrientedX) //si no esta correctamente orientado al inicio rota el obj
        {
            RotateSpriteX();
        }
        if (!_isSpriteWellOrientedY) //si no esta correctamente orientado al inicio rota el obj
        {
            RotateSpriteY();
        }
    }
     public void RotateSpriteX()
    {
        //_animator.SetTrigger("Bounce");
        if (!_canFlipX) return;
        if (_spriteRenderer != null)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX; 
        }
       
    }
    public void RotateSpriteY()
    {
        //_animator.SetTrigger("Bounce");
        if (!_canFlipY) return;
        if (_spriteRenderer != null)
        {
            _spriteRenderer.flipY = !_spriteRenderer.flipY;
        }
    }
    public void Destroy()
    {
        _animator.SetTrigger("Die");
       this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

    }
    public void Damage()
    {
        _animator.SetTrigger("Damage");

    }
    public void ChangeState()
    {
        _animator.SetTrigger("ChangeState");
        _animator.SetBool("Left", false);
        _animator.SetBool("Right", false);
        _animator.SetBool("Up", false);
        _animator.SetBool("Down", false);
        _animator.SetBool("Follow", false);

    }
    public void SpawnEvent()
    {
        Debug.Log(_animator + " " + this.gameObject);
        _animator.SetTrigger("Spawn");

    }
    public void LeftDirection()
    {
        _animator.SetBool("Left", true);
        _animator.SetBool("Right", false);

    }
    public void RightDirection()
    {
        _animator.SetBool("Left", false);
        _animator.SetBool("Right",true );

    }
    public void DownDirection()
    {
        _animator.SetBool("Up", false);
        _animator.SetBool("Down", true);

    }
    public void UpDirection()
    {
        _animator.SetBool("Down", false);
        _animator.SetBool("Up", true);

    }
    public void XLeftChangeAndFlip()
    {

        if (_animator.GetBool("Right")) // Solo volteamos el sprite si antes iba a la derecha
        {
            RotateSpriteX();
        }
        LeftDirection(); // Cambiamos la direccion a la izquierda
    }
    public void XRightChangeAndFlip()
    {

        if (_animator.GetBool("Left")) // Solo volteamos el sprite si antes iba a la derecha
        {
            RotateSpriteX();
        }
        RightDirection(); // Cambiamos la direccion a la izquierda
    }
    public void ChangeSpeedX(float speed)
    {
        _animator.SetFloat("XSpeed", speed);
    }
    public void ChangeSpeedY(float speed)
    {
        _animator.SetFloat("YSpeed", speed);

    }
    public void ChangeSpeedRotation(float speed)
    {
        _animator.SetFloat("RotationSpeed", speed);
        
    }
    public void Follow()
    {
        _animator.SetBool("Follow", true);
    }

    public void OnDieEvent()
    {
        // Debug.Log("DIE + " + this.gameObject.ToString());
        Destroy(gameObject);
    }
    private void OnValidate()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer != null)
        {
            
            _spriteRenderer.flipX = !_isSpriteWellOrientedX;
            _spriteRenderer.flipY = !_isSpriteWellOrientedY;
        }
            
    }
}

