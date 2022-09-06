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
        move.Pause(0.1f);
        jump.Pause(0.1f);
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out AnimatorExtender anim))
            anim.StartPause(0.1f);
    }
}
