using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    [Header("Jump Info")]
    [SerializeField] private float velocityScalar;
    [SerializeField] private float terminalVelocity;

    [SerializeField] private AnimationCurve risingCurve;
    [SerializeField] private AnimationCurve fallingCurve;
    //Extra variables
    private float timeStamp = -100;
    private bool jumping, starting;
    private AnimationCurve curve;
    private float risingTime, fallingTime, maxTime;
    private float pauseTime;

    void Start() {
        risingTime = risingCurve[risingCurve.length - 1].time;
        fallingTime = fallingCurve[fallingCurve.length - 1].time;
    }

    void FixedUpdate()
    {
        if (pauseTime > 0) {
            pauseTime -= Time.fixedDeltaTime;
            if (pauseTime <= 0)
                rbody.gravityScale = 20f;
            else
                return;
        }
        if (starting && (InputHandler.Instance.jump.released || Time.time - timeStamp > risingTime))
            EndJump();
        if (Time.time - timeStamp <= maxTime)     
            rbody.velocity = new Vector2(rbody.velocity.x, velocityScalar * curve.Evaluate(Time.time - timeStamp));

        if (rbody.velocity.y < -terminalVelocity)
            rbody.velocity = new Vector2(rbody.velocity.x, -terminalVelocity);
    }

    public void StartJump() {
        timeStamp = Time.time;
        starting = true;
        curve = risingCurve;
        maxTime = risingTime;
    }

    private void EndJump() {
        timeStamp = Time.time;
        starting = false;
        curve = fallingCurve;
        maxTime = fallingTime;
    }

    public void ForceLanding() {
        starting = false;
        timeStamp = -100;
    }

    public void Pause(float amt) {
        if (pauseTime > amt)
            return;
        pauseTime = amt;
        rbody.velocity = new Vector2(rbody.velocity.x, 0);
        timeStamp += amt - pauseTime;
        rbody.gravityScale = 0;
    }

}
