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
    bool findañopersistente;


    // Stores the latest collision event
    Collider2D m_collisionobj;

    [SerializeField, Range(0, 2)]
    private int damageType = 0; // 0 = Instantáneo, 1 = Persistente, 2 = Residual
    [SerializeField]
    private float amountofdamage= 0; //para tipo 0,1 y 2
    [SerializeField]
    private float damageCooldown = 1f; // para tipo 1 y 2
    private float lastDamageTime; // Último momento en que recibió daño
    [SerializeField]
    private int numofdamage = 2; //cuantas veces haces daño

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_isChecking)
        {
            switch (damageType) //0 = Instantáneo, 1 = Persistente, 2 = Residual
            {
                case 0:
                    col = true;
                    m_collisionobj = collision;
                    EventDetected(); // Call the event handler method
                    break;
                case 1: //daño que persiste mientras está dentro del trigger
                    col = true;
                    EventDetected(); //mirar de que tipo es al recivir el evento
                    //si es de tipo persistente
                    break;
                case 2: //permanece activo
                        //Representa el daño aplicado tras un impacto inicial,
                        //pero que permanece activo durante un corto período
                        //, infligiendo unas pequeñas cantidades de daño,
                        //incluso si el volumen de colisión ya no está en contacto
                        //con el volumen que inició el daño.
                    col = true;
                    EventDetected();
                    break;
                default:
                    break;

            }
                     
            // instantaneo
            // ini residual
           
        }

        //if (collision.CompareTag("CCED")) // Si colisiona con una Caja de Colisión Emitir Daño
        //{
        //    lastDamageTime = Time.time; // Actualiza el último tiempo de daño
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
            findañopersistente = true;
        
        }
    }
    public override bool CanTransition()
    {
        //la transicion depende del tipo de daño, 
            //intantaneo al inicio
            //persistente cuando salga
            //resudual al aplicar el primer daño

        if (damageType == 0 && col)
        {
            m_isChecking = false;
            return true;
        }
        else if (damageType == 1 && findañopersistente)
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
        findañopersistente = false;
    }
    // Getters
    public bool IsChecking() => m_isChecking;
    public bool HasCollisionOccurred() => col;
    public bool EndPersistentDamage() => findañopersistente;
    public Collider2D GetCollisionObject() => m_collisionobj;
    public int GetDamageType() => damageType;
    public float GetAmountOfDamage() => amountofdamage;
    public float GetDamageCooldown() => damageCooldown;
    public float GetLastDamageTime() => lastDamageTime;
    public int GetNumOfDamage() => numofdamage;
}
