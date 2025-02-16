using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyLife : Life
{
    // Start is called before the first frame update
    [SerializeField]
    private List<Damage_Sensor> m_eventsToReact;

    bool m_update=false;
    float m_amount = -1;
    Damage_Sensor sensor;
    private float lastDamageTime; // Último momento en que recibió daño
    
    private float damageCooldown =-1;
    private int numofdamage ;
    private bool persistent;
    new void Start()
    {
        base.Start();
        // Collision.OnCollisionSensor += ReceiveMessage;
        foreach (var sensor in m_eventsToReact)
        {
            int type = sensor.GetDamageType();
            switch (type)
            {
                case 0: // instantáneo
                    sensor.onEventDetected += Instantaneo;
                    break;
                case 1: //persistente
                    sensor.onEventDetected += Persistente;
                    break;
                case 2: //residual
                    sensor.onEventDetected += Residual;
                    break;
                default:
                    break;
            }
       
        }

    }
    public void Update()
    {
        if (m_update && m_amount !=-1)
        {
            if (persistent)  //si es de tipo persistente
            {
                base.DecreaseLife(m_amount);
               
                if (sensor != null)
                {
                    m_update = !sensor.EndPersistentDamage(); //se sigue actualizando hasta que termine el daño persistente
                }
            }
            else if(numofdamage>0)//si es detipo residual (son las unicas 2 que se actualizan)
            {
              

                lastDamageTime += Time.deltaTime;
                Debug.Log(lastDamageTime);
                if (lastDamageTime> damageCooldown)
                {
                    numofdamage--;
                    lastDamageTime-= damageCooldown;
                    base.DecreaseLife(m_amount);
                }
                
            }
           
        }
    }
    private void OnDestroy()
    {
        //Collision.OnCollisionSensor -= ReceiveMessage;
        foreach (var sensor in m_eventsToReact)
        {
           
            int type = sensor.GetDamageType();
            switch (type)
            {
                case 0: // instantáneo
                    sensor.onEventDetected -= Instantaneo;
                    break;
                case 1: //persistente
                    sensor.onEventDetected -= Persistente;
                    break;
                case 2: //residual
                    sensor.onEventDetected -= Residual;
                    break;
                default:
                    break;
            }
        }
    }
    void Instantaneo(Sensors s)
    {

        Damage_Sensor sensor = s as Damage_Sensor;
        if (sensor != null)
        {
            base.DecreaseLife(sensor.GetAmountOfDamage());
        }

    }
    
    void Persistente(Sensors s)
    {
        Damage_Sensor sensor = s as Damage_Sensor;
        if (sensor != null)
        {
            m_amount = sensor.GetAmountOfDamage();
            m_update = true;
            this.sensor = sensor;
            persistent = true;
        }
    }

    void Residual(Sensors s)
    {
        Damage_Sensor sensor = s as Damage_Sensor;
        if (sensor != null)
        {
            m_amount = sensor.GetAmountOfDamage();
            m_update = true;
            this.sensor = sensor;
            damageCooldown = sensor.GetDamageCooldown();
            numofdamage = sensor.GetNumOfDamage();
            base.DecreaseLife(m_amount);
            numofdamage--;
            lastDamageTime = 0;
            persistent = false;

        }
    }

}
