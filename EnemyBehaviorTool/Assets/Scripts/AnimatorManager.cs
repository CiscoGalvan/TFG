using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimatorManager : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private bool _isSpriteWellOrientedX = true;
    [SerializeField]
    private bool _isSpriteWellOrientedY = true;
    [SerializeField]
    private bool _canFlipX = true;
    [SerializeField]
    private bool _canFlipY = true;
    [SerializeField]
    private bool _canRotate = true;
    SpriteRenderer _spriteRenderer;

#if UNITY_EDITOR
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
  #endif

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("NO ANIMATOR IS ATTACHED");
            return;
        }

        if (!_isSpriteWellOrientedX)
        {
            RotateSpriteX();
        }
        if (!_isSpriteWellOrientedY)
        {
            RotateSpriteY();
        }
    }

    public void RotateSpriteX()
    {
        if (!_canFlipX) return;
        if (_spriteRenderer != null)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }
    }

    public void RotateSpriteY()
    {
        if (!_canFlipY) return;
        if (_spriteRenderer != null)
        {
            _spriteRenderer.flipY = !_spriteRenderer.flipY;
        }
    }

    public void Destroy()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetTrigger("Die");
        this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void Damage()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetTrigger("Damage");
    }

    public void ChangeState()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetTrigger("ChangeState");
        _animator.SetBool("Left", false);
        _animator.SetBool("Right", false);
        _animator.SetBool("Up", false);
        _animator.SetBool("Down", false);
        _animator.SetBool("Follow", false);
    }

    public void SpawnEvent()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetTrigger("Spawn");
    }

    public void LeftDirection()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetBool("Left", true);
        _animator.SetBool("Right", false);
    }

    public void RightDirection()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetBool("Left", false);
        _animator.SetBool("Right", true);
    }

    public void DownDirection()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetBool("Up", false);
        _animator.SetBool("Down", true);
    }

    public void UpDirection()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetBool("Down", false);
        _animator.SetBool("Up", true);
    }

    public void XLeftChangeAndFlip()
    {
        if (_animator == null || !_animator.enabled) return;
        if (_animator.GetBool("Right"))
        {
            RotateSpriteX();
        }
        LeftDirection();
    }

    public void XRightChangeAndFlip()
    {
        if (_animator == null || !_animator.enabled) return;
        if (_animator.GetBool("Left"))
        {
            RotateSpriteX();
        }
        RightDirection();
    }

    public void ChangeSpeedX(float speed)
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetFloat("XSpeed", speed);
    }

    public void ChangeSpeedY(float speed)
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetFloat("YSpeed", speed);
    }

    public void ChangeSpeedRotation(float speed)
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetFloat("RotationSpeed", speed);
    }

    public void Follow()
    {
        if (_animator == null || !_animator.enabled) return;
        _animator.SetBool("Follow", true);
    }

    public void OnDieEvent()
    {
        Destroy(gameObject);
    }
}
