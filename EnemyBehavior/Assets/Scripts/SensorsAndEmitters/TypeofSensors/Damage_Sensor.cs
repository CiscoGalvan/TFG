using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damage_Sensor : Sensors
{
    // Boolean to track whether the sensor is actively checking for collisions
    bool m_isChecking;
    // Boolean to track if a collision has occurred
    bool col;
    bool finda�opersistente;


    // Stores the latest collision event
    Collider2D m_collisionobj;

    [SerializeField, Range(0, 2)]
    private int damageType = 0; // 0 = Instant�neo, 1 = Persistente, 2 = Residual
    [SerializeField]
    private float amountofdamage= 0; //para tipo 0,1 y 2
    [SerializeField]
    private float damageCooldown = 1f; // para tipo 1 y 2
    private float lastDamageTime; // �ltimo momento en que recibi� da�o
    [SerializeField]
    private int numofdamage = 2; //cuantas veces haces da�o

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_isChecking)
        {
            switch (damageType) //0 = Instant�neo, 1 = Persistente, 2 = Residual
            {
                case 0:
                    col = true;
                    m_collisionobj = collision;
                    EventDetected(); // Call the event handler method
                    break;
                case 1: //da�o que persiste mientras est� dentro del trigger
                    col = true;
                    EventDetected(); //mirar de que tipo es al recivir el evento
                    //si es de tipo persistente
                    break;
                case 2: //permanece activo
                        //Representa el da�o aplicado tras un impacto inicial,
                        //pero que permanece activo durante un corto per�odo
                        //, infligiendo unas peque�as cantidades de da�o,
                        //incluso si el volumen de colisi�n ya no est� en contacto
                        //con el volumen que inici� el da�o.
                    col = true;
                    EventDetected();
                    break;
                default:
                    break;

            }
                     
            // instantaneo
            // ini residual
           
        }

        //if (collision.CompareTag("CCED")) // Si colisiona con una Caja de Colisi�n Emitir Da�o
        //{
        //    lastDamageTime = Time.time; // Actualiza el �ltimo tiempo de da�o
        //    EventDetected(); // Call the event handler method
        //}
        //else if (collision.CompareTag("CCRD"))
        //{
        //    lastDamageTime = Time.time;

        //}
        //else if (collision.CompareTag("CCND"))
        //{
        //    lastDamageTime = Time.time;
        //    EventDetected();
        //}
        //else

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_isChecking && damageType==1)
        {
          
            col = false;
            finda�opersistente = true;
        
        }
    }
    public override bool CanTransition()
    {
        //la transicion depende del tipo de da�o, 
            //intantaneo al inicio
            //persistente cuando salga
            //resudual al aplicar el primer da�o

        if (damageType == 0 && col)
        {
            m_isChecking = false;
            return true;
        }
        else if (damageType == 1 && finda�opersistente)
        {
            m_isChecking = false; 
            return true;
        }
        else if (damageType == 2)
        {
            return true;
        }
        return false; 
    }

    public override void StartSensor()
    {
        col = false;
        m_isChecking = true;
        finda�opersistente = false;
    }
    // Getters
    public bool IsChecking() => m_isChecking;
    public bool HasCollisionOccurred() => col;
    public bool EndPersistentDamage() => finda�opersistente;
    public Collider2D GetCollisionObject() => m_collisionobj;
    public int GetDamageType() => damageType;
    public float GetAmountOfDamage() => amountofdamage;
    public float GetDamageCooldown() => damageCooldown;
    public float GetLastDamageTime() => lastDamageTime;
    public int GetNumOfDamage() => numofdamage;
}
