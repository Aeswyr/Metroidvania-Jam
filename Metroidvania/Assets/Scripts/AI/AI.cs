using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private MovementHandler move;
    

    protected int facing = 1;
    protected int dir = 0;
    protected int lastDir = 0;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        AIUpdate();
        if (dir != lastDir)
        {
            if (lastDir == 0) {
                //animator.SetBool("moving", true);
                move.StartAcceleration(dir);
            }
            else if (dir == 0)
            {
                move.StartDeceleration();
            }
        }
        else
        {
            //animator.SetBool("moving", false);
            move.UpdateMovement(dir);
        }
        
        lastDir = dir;
        dir = 0;
    }

    protected virtual void AIUpdate()
    {

    }

    protected void AIMove(int dir)
    {
        this.dir = dir;
        sprite.flipX = dir < 0;
        facing = sprite.flipX ? -1 : 1;
    }
}