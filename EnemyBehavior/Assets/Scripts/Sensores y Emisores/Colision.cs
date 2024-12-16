using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision : MonoBehaviour
{
    public static event Action<Collision2D> OnMensajeEnviado;
     private void OnCollisionEnter2D(Collision2D collision)
    {
        OnMensajeEnviado?.Invoke(collision);
        Debug.Log("mensaje enviado");
    }

}
