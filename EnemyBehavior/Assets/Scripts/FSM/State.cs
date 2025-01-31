using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    [SerializeField]
     public List<Actuator> actuatorList = new List<Actuator>();
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        foreach (var actuator in actuatorList)
        {
            if (actuator != null)
            {
               // actuator.Initialize();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (var actuator in actuatorList)
        {
            if (actuator != null)
            {
              //  actuator.Actuate(rb);
            }
        }
    }

}
