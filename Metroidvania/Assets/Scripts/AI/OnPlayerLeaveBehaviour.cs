using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPlayerLeaveBehaviour : AIBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float boredRange;
    [SerializeField] private UnityEvent onPlayerLeave;

    protected override void AIUpdate()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, boredRange, playerLayer.value);
        if (col != null)
        {
            enabled = false;
            onPlayerLeave.Invoke();
        }
    }
}