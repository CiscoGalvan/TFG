using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Animator _animator;
    Rigidbody2D _rigidbody;
    void Start()
    {
        if(_animator == null)
        {
            Debug.LogErrorFormat("no Animator is attached");
        }
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            Debug.LogErrorFormat("no Rigidbody2D is attached");
        }
    }

    // Update is called once per frame
    void Update()
    {
         _animator.SetFloat("XSpeed", Mathf.Abs(_rigidbody.velocity.x));
         _animator.SetFloat("YSpeed", Mathf.Abs(_rigidbody.velocity.y));
         _animator.SetBool("LookingUP",false);
         _animator.SetBool("LookingDown", false);
         _animator.SetBool("LookingLeft", false);
         _animator.SetBool("LookingRight", false);
         _animator.SetBool("Rotating", false);
         _animator.SetBool("Spawn", false);
         _animator.SetBool("Damaged", false);
         _animator.SetBool("Collisioned", false);
         _animator.SetBool("Die", false);
         _animator.SetBool("Delete", false);
        //_animator.SetBool("Distanced", false);
        //_animator.SetBool("Timer", false); //nssi lo vamos a usar para algo 

    }
}
