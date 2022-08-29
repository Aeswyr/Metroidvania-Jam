using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private float wallDetectDistance = 1f;
    [SerializeField] private Vector2 bodySideOffset;
    [SerializeField] private LayerMask wallDetectMask;
    [Header("Jump Info")]
    [SerializeField] private float velocityScalar;
    [SerializeField] private float terminalVelocity;
    [SerializeField] private AnimationCurve slidingCurve;

    private float timeStamp = -100;
    private float slidingTime;
    private bool wasSliding;
    private float gravityScale;
    
    void Start()
    {
        slidingTime = slidingCurve[slidingCurve.length - 1].time;
        wasSliding = false;
        gravityScale = rbody.gravityScale;
    }

    void FixedUpdate()
    {
        if (IsWallSliding())
        {
            if (!wasSliding)
            {
                timeStamp = Time.time;
                rbody.gravityScale = 0f;
            }
            rbody.velocity = new Vector2(0, -velocityScalar * slidingCurve.Evaluate(Mathf.Min(Time.time - timeStamp, slidingTime)));
            if (rbody.velocity.y < -terminalVelocity)
                rbody.velocity = new Vector2(rbody.velocity.x, -terminalVelocity);
            wasSliding = true;
        }
        else
        {
            if (wasSliding)
                rbody.gravityScale = gravityScale;
            wasSliding = false;
        }
    }

    public bool IsWallSliding()
    {
        RaycastHit2D left = Utils.Raycast(transform.position + new Vector3(-bodySideOffset.x, bodySideOffset.y, 0), Vector2.left, wallDetectDistance, wallDetectMask);
        RaycastHit2D right = Utils.Raycast(transform.position + new Vector3 (bodySideOffset.x, bodySideOffset.y, 0), Vector2.right, wallDetectDistance, wallDetectMask);
        return left&&InputHandler.Instance.move.down&&InputHandler.Instance.dir==-1 || right&&InputHandler.Instance.move.down&&InputHandler.Instance.dir==1;
    }
}