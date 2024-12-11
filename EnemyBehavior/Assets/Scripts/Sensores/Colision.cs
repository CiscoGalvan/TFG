using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision : MonoBehaviour
{
    public static event Action<string> OnMensajeEnviado;
     private void OnCollisionEnter2D(Collision2D collision)
    {
        OnMensajeEnviado?.Invoke("mensaje");
        Debug.Log("mensaje enviado");
    }

}
