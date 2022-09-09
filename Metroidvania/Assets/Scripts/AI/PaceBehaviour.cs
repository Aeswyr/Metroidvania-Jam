using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceBehaviour : AIBehaviour
{
    [SerializeField] private float paceDistance;
    [SerializeField] private float pacePauseTime;
    private float paceX;
    private float pauseTimestamp;
    private bool paused = false;
    private int paceDir = 1;

    protected override void AIStart()
    {
        paceX = transform.position.x;
    }

    protected override void AIUpdate()
    {
        if (paused)
        {
            if (Time.time - pauseTimestamp >= pacePauseTime)
            {
                paused = false;
                paceX = transform.position.x;
                paceDir *= -1;
            }
        }
        else
        {
            if (Mathf.Abs(paceX - transform.position.x) < paceDistance)
            {
                AIMove(paceDir);
            }
            else
            {
                pauseTimestamp = Time.time;
                paused = true;
            }
        }
    }
}