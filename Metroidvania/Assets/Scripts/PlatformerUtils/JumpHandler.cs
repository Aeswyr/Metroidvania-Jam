using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    private InputHandler input;
    [Header("Jump Info")]
    [SerializeField] private float velocityScalar;
    [SerializeField] private float maxRisingTime, maxTime;
    [SerializeField] private AnimationCurve curve;
    //Extra variables
    private float timeStamp = -100;
    private bool jumping, starting;

    public void LinkInputs(InputHandler input) {
        this.input = input;
    }
    void FixedUpdate()
    {
        if (starting && (input.jump.released || Time.time - timeStamp > maxRisingTime))
            EndJump();
        if (Time.time - timeStamp <= maxTime)     
            rbody.velocity = new Vector2(rbody.velocity.x, velocityScalar * curve.Evaluate(Time.time - timeStamp));
    }

    public void StartJump() {
        timeStamp = Time.time;
        starting = true;
    }

    private void EndJump() {
        timeStamp = Time.time - maxRisingTime;
        starting = false;
    }

    public void ForceLanding() {
        starting = false;
        timeStamp = -100;
    }

}
