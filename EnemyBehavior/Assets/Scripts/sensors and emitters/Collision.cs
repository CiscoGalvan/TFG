using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Collision : MonoBehaviour
{
    public static event Action<Collision2D> OnMessageSent;
     private void OnCollisionEnter2D(Collision2D collision)
    {
        OnMessageSent?.Invoke(collision);
        Debug.Log("message sent");
    }

}
