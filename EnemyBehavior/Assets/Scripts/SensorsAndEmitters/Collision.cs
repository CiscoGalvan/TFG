using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Collision : Sensors
{
    public static event Action<Collision2D> OnCollisionSensor;
    bool col;
    Collision2D m_collisionobj;
    int it = 0;
     private void OnCollisionEnter2D(Collision2D collision)
     {
        col =true;
        m_collisionobj = collision;
     }
    ////public override void Update() { 
    ////     if collision{
    ////        OnCollisionSensor?.Invoke(collision);
    ////        Debug.Log("message sent");
    ////    }
    ////}
    public override bool CanTransition()
    {
      
            bool aux = col;
            col = false;
            return aux;
        
        
    }
    public override void Start() { it = 0;col = false; }
}
