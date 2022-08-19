using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rbody;
    [Header("Acceleration Info")]
    [SerializeField] private AnimationCurve accelerationCurve;
    [SerializeField] private float accelerationTime;
    [Header("Deceleration Info")]
    [SerializeField] private AnimationCurve decelerationCurve;
    [SerializeField] private float decelerationTime;
    private float timestamp;
    private float dir;
    private float decelSpeed;
    bool moving = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time < timestamp) {
            if (moving)
                rbody.velocity = new Vector2(speed * dir * accelerationCurve.Evaluate(Time.time - timestamp + accelerationTime), rbody.velocity.y);
            else
                rbody.velocity = new Vector2(decelSpeed * dir * decelerationCurve.Evaluate(Time.time - timestamp + decelerationTime), rbody.velocity.y);
        } else {
            if (moving)
                rbody.velocity = new Vector2(speed * dir, rbody.velocity.y);
        }
    }

    public void StartDeceleration() {
        moving = false;
        timestamp = Time.time + decelerationTime;
        decelSpeed = speed;
        if (Mathf.Abs(rbody.velocity.x) < decelSpeed)
            decelSpeed = Mathf.Abs(rbody.velocity.x);
    }

    public void StartAcceleration(float dir) {
        moving = true;
        timestamp = Time.time + accelerationTime;
    }

    public void UpdateMovement(float dir) {
        this.dir = dir;
        moving = true;
    }


    


}
