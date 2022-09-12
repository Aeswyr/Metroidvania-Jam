using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlockBehaviour : AIBehaviour
{
    [SerializeField] private float blockLength;
    [SerializeField] private UnityEvent onBlockBegin;
    [SerializeField] private UnityEvent onBlockEnd;

    private float blockTimestamp;

    void OnEnable()
    {
        animator.SetBool("block", true);
        animator.SetBool("moving", false);
        move.StartDeceleration();
        onBlockBegin.Invoke();
        blockTimestamp = Time.time;
        
    }

    protected override void AIUpdate()
    {
        if (Time.time - blockTimestamp >= blockLength)
        {
            animator.SetBool("block", false);
            onBlockEnd.Invoke();
            enabled = false;
        }
    }
}