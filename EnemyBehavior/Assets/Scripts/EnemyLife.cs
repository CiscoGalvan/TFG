using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyLife : Life
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Collision.OnCollisionSensor += ReceiveMessage;

    }
    private void OnDestroy()
    {
        Collision.OnCollisionSensor -= ReceiveMessage;
    }
    void ReceiveMessage(Collision2D mensaje)
    {
        if(mensaje.gameObject.GetComponent<PlayerMovement>() != null)
        {
            base.DecreaseLife(1.0f);
        }
       

    }
   
}
