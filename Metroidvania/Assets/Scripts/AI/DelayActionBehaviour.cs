using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayActionBehaviour : AIBehaviour
{
    [SerializeField] private float delayLength;
    [SerializeField] private UnityEvent onDelay;

    private float delayTimestamp;

    public void Delay()
    {
        delayTimestamp = Time.time;  
        enabled = true; 
    }

    protected override void AIUpdate()
    {
        if (Time.time - delayTimestamp >= delayLength)
        {
            onDelay.Invoke();
            enabled = false;
        }
    }
}