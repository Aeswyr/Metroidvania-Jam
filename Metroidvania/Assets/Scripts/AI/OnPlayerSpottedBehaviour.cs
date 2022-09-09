using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPlayerSpottedBehaviour : AIBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float sightRange;
    [SerializeField] private UnityEvent onPlayerSpotted;

    private PlayerHandler player;

    protected override void AIAwake()
    {
        player = GameHandler.Instance.GetPlayer();
    }

    protected override void AIUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * facing, sightRange, playerLayer.value);
        if (hit.collider != null)
        {
            if (hit.collider.transform.parent.gameObject == player.gameObject && hit.distance <= sightRange);
            {
                enabled = false;
                onPlayerSpotted.Invoke();
            }
        }
    }
}