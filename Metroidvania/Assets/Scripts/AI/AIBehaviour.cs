using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MovementHandler))]
public class AIBehaviour : MonoBehaviour
{
    [SerializeField] private bool controlMovement;

    protected Animator animator;
    protected SpriteRenderer sprite;
    private MovementHandler move;
    

    protected int facing = 1;
    protected int dir = 0;
    protected int lastDir = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        move = GetComponent<MovementHandler>();
        AIAwake();
    }

    protected virtual void AIAwake()
    {

    }

    void Start()
    {
        AIStart();
    }

    protected virtual void AIStart()
    {

    }

    void FixedUpdate()
    {
        AIUpdate();
        if (!controlMovement) return;
        if (dir != lastDir)
        {
            if (lastDir == 0) {
                animator.SetBool("moving", true);
                move.StartAcceleration(dir);
            }
            else if (dir == 0)
            {
                animator.SetBool("moving", false);
                move.StartDeceleration();
            }
        }
        else if (dir != 0)
        {
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