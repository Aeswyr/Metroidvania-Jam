using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayActionBehaviour : AIBehaviour
{
    [SerializeField] private float delayLength;
    [SerializeField] private UnityEvent onDelay;

    private float delayTimestamp;

    void OnEnable()
    {
        delayTimestamp = Time.time;   
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