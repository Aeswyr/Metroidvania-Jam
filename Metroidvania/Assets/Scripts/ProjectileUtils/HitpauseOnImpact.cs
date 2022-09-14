using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitpauseOnImpact : MonoBehaviour
{
    bool triggered = false;
    private AnimatorExtender animator;
    private MovementHandler move;
    private JumpHandler jump;
    public HitpauseOnImpact Init(AnimatorExtender animator, MovementHandler move, JumpHandler jump) {
        this.animator = animator;
        this.move = move;
        this.jump = jump;
        return this;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        animator.StartPause(0.1f);
        if (move != null)
            move.Pause(0.1f);
        if (jump != null)
            jump.Pause(0.1f);
        if (other.transform.parent != null) {
            if (other.transform.parent.TryGetComponent(out AnimatorExtender anim))
                anim.StartPause(0.1f);
            if (other.transform.parent.TryGetComponent(out MovementHandler mov))
                mov.Pause(0.1f);
            if (other.transform.parent.TryGetComponent(out JumpHandler jum))
                jum.Pause(0.1f);
        }
            
    }
}
