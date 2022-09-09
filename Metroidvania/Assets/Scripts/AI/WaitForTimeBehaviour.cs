using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitForTimeBehaviour : AIBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private UnityEvent onWaitOver;

    float timer;

    void OnEnable()
    {
        timer = waitTime;
    }

    protected override void AIUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            enabled = false;
            onWaitOver.Invoke();
        }
    }
}