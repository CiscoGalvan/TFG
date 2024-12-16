using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyLife : Life
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Colision.OnMensajeEnviado += RecibirMensaje;

    }
    private void OnDestroy()
    {
        Colision.OnMensajeEnviado -= RecibirMensaje;
    }
    void RecibirMensaje(Collision2D mensaje)
    {
        if(mensaje.gameObject.GetComponent<PlayerMovement>() != null)
        {
            base.DecreaseLife(1.0f);
        }
       

    }
   
}
