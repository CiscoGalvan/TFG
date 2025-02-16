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
        Debug.Log(m_update);
        if (m_update && m_amount !=-1)
        {
           
            base.DecreaseLife(m_amount);
            Debug.Log(sensor);
            if (sensor!= null)
            {
                Debug.Log("update = " + sensor.EndPersistentDamage());
                m_update = !sensor.EndPersistentDamage(); //se sigue actualizando hasta que termine el daño persistente
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
        }
    }

    void Residual(Sensors s)
    {
        Damage_Sensor sensor = s as Damage_Sensor;
        if (sensor != null)
        {
            base.DecreaseLife(sensor.GetAmountOfDamage());
        }
    }

}
