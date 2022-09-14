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
    private bool blocking = false;

    public void Block()
    {
        animator.SetTrigger("block");
        animator.SetBool("moving", false);
        move.StartDeceleration();
        onBlockBegin.Invoke();
        blockTimestamp = Time.time;
        blocking = true;
    }

    protected override void AIUpdate()
    {
        if (blocking && Time.time - blockTimestamp >= blockLength)
        {
            onBlockEnd.Invoke();
            blocking = false;
        }
    }
}