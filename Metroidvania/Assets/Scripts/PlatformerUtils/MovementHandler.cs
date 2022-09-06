using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private AnimationCurve accelerationCurve;
    private float accelerationTime;
    [SerializeField] private AnimationCurve decelerationCurve;
    private float decelerationTime;
    private float timestamp;
    private float dir;
    private float decelSpeed;
    bool moving = false, cleanupSpeed = false;
    float pauseTime = 0;

    // Update is called once per frame


    void Awake() {
        accelerationTime = accelerationCurve[accelerationCurve.length - 1].time;
        decelerationTime = decelerationCurve[decelerationCurve.length - 1].time;
    }

    void FixedUpdate()
    {
        if (pauseTime > 0) {
            pauseTime -= Time.fixedDeltaTime;
            return;
        }
        if (Time.time < timestamp) {
            if (moving)
                rbody.velocity = new Vector2(speed * dir * accelerationCurve.Evaluate(Time.time - timestamp + accelerationTime), rbody.velocity.y);
            else
                rbody.velocity = new Vector2(decelSpeed * dir * decelerationCurve.Evaluate(Time.time - timestamp + decelerationTime), rbody.velocity.y);
        } else {
            if (moving)
                rbody.velocity = new Vector2(speed * dir, rbody.velocity.y);
            else if (cleanupSpeed) {
                cleanupSpeed = false;
                rbody.velocity = new Vector2(0, rbody.velocity.y);
            }
        }
    }

    public void StartDeceleration() {
        cleanupSpeed = true;
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

    public void ResetMovement() {
        cleanupSpeed = false;
        moving = false;
        timestamp = 0;
        decelSpeed = 0;
        rbody.velocity = Vector2.zero;
    }

    public void Pause(float amt) {
        if (pauseTime > amt)
            return;
        pauseTime = amt;
        rbody.velocity = new Vector2(0, rbody.velocity.y);
        timestamp += amt - pauseTime;
    }
    


}
