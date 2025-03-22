using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
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


    Rigidbody2D _rigidbody;


    private void Start()
    {
       
        _animator = GetComponent<Animator>(); 
        if(_animator == null)
        {
            Debug.LogError("NO ANIMATOR IS ATTACHED");
        }
        _rigidbody = GetComponent<Rigidbody2D>();
        if (!_isSpriteWellOrientedX) //si no esta correctamente orientado al inicio rota el obj
        {
            RotatesrpiteX();
        }
        if (!_isSpriteWellOrientedY) //si no esta correctamente orientado al inicio rota el obj
        {
            RotatesrpiteY();
        }
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
    public void Destroy()
    {
        _animator.SetTrigger("Die");
       
    }
    public void ChangeState()
    {
        _animator.SetTrigger("ChangeState");

    }
    public void SpawnEvent()
    {
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
    public void OnDieEvent()
    {
        // Debug.Log("DIE + " + this.gameObject.ToString());
        Destroy(gameObject);
    }

}

