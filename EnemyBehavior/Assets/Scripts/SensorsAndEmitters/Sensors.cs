using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sensors : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public State destinationState;

    [SerializeField]
    bool m_transition;
    public abstract void Start();

    // Update is called once per frame
    public abstract bool CanTransition();
   // public abstract void Event();
}
